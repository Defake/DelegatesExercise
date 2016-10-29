using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
Вы разрабатываете класс, который в будущем будет использоваться в движке электронной таблицы в качестве модели данных.
Вам необходимо реализовать в этом классе следующие методы:

Put(int row, int column, int value) - помещает значение в соответствующую ячейку
InsertRow(int rowIndex) - добавляет пустую строку по указанному индексу
InsertColumn(int columnIndex) - добавляет пустую строку (kek =) ) по указанному индексу
Get(int row, int column) - возвращает хранимое в таблице значение

Далее, в движке будут внешние системы, которые будут работать с электронной таблицей: 
например, представление, отображающее ее, и система логгирования, записывающая все проведенные действия.

*/

namespace DelegatesExercise 
{
	public class TableModel
	{

		private int _width;
		private readonly List<List<int>> _tableValues;

		// Event observers lists
		private readonly List<Action<CellArgument>> _valuePut;
		private readonly List<Action<RowArgument>> _rowInserted;
		private readonly List<Action<ColumnArgument>> _columnInserted;
		private readonly List<Action<CellArgument>> _askedValue;

		public TableModel(int width, int height)
		{
			_width = width;
			_tableValues = new List<List<int>>();
			for (int i = 0; i < height; i++)
			{
				_tableValues.Add(new List<int>(_width));

				for (int j = 0; j < width; j++)
					_tableValues[i].Add(0);
			}

			_valuePut = new List<Action<CellArgument>>();
			_rowInserted = new List<Action<RowArgument>>();
			_columnInserted = new List<Action<ColumnArgument>>();
			_askedValue = new List<Action<CellArgument>>();
		}

		// Add listeners to events
		public IDisposable AddPutValueObserver(Action<CellArgument> f) {
			_valuePut.Add(f);
			return new TableEventDisposer<CellArgument>(_valuePut, f);
		}

		public IDisposable AddRowInsertedObserver(Action<RowArgument> f) {
			_rowInserted.Add(f);
			return new TableEventDisposer<RowArgument>(_rowInserted, f);
		}

		public IDisposable AddColumnInsertedObserver(Action<ColumnArgument> f) {
			_columnInserted.Add(f);
			return new TableEventDisposer<ColumnArgument>(_columnInserted, f);
		}

		public IDisposable AddAskedValueObserver(Action<CellArgument> f) {
			_askedValue.Add(f);
			return new TableEventDisposer<CellArgument>(_askedValue, f);
		}

		public void Put(int row, int column, int value)
		{
			_tableValues[row][column] = value;

			if (_valuePut == null) return;
			foreach (var action in _valuePut)
				action(new CellArgument(row, column, value));
		}

		public void InsertRow(int rowIndex)
		{
			_tableValues.Insert(rowIndex, new List<int>());
			for (int i = 0; i < _width; i++)
			{
				_tableValues[rowIndex].Add(0);
			}

			if (_rowInserted == null) return;
			foreach (var action in _rowInserted)
				action(new RowArgument(rowIndex));
		}

		public void InsertColumn(int columnIndex)
		{
			foreach (var row in _tableValues)
				row.Insert(columnIndex, 0);

			_width++;

			if (_columnInserted == null) return;
			foreach (var action in _columnInserted)
				action(new ColumnArgument(columnIndex));
		}

		public int Get(int row, int column)
		{
			var value = _tableValues[row][column];

			if (_askedValue != null)
				foreach (var action in _askedValue)
					action(new CellArgument(row, column, value));

			return value;
		}

		private class TableEventDisposer<TArgument> : IDisposable {

			private readonly List<Action<TArgument>> _listenersList;
			private readonly Action<TArgument> _listener;

			public TableEventDisposer(List<Action<TArgument>> listenersList, Action<TArgument> listener) 
			{
				_listenersList = listenersList;
				_listener = listener;
			}

			public void Dispose() => 
				_listenersList?.Remove(_listener);
		}

	}
}
