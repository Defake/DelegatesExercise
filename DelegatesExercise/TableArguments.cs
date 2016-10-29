using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesExercise 
{

	public class CellArgument
	{
		public int rowIndex { get; private set; }
		public int columnIndex { get; private set; }
		public int value { get; private set; }

		public CellArgument(int rowIndex, int columnIndex, int value)
		{
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.value = value;
		}
	}

	public class ColumnArgument
	{
		public int columnIndex { get; private set; }

		public ColumnArgument(int columnIndex)
		{
			this.columnIndex = columnIndex;
		}
	}

	public class RowArgument
	{
		public int rowIndex { get; private set; }

		public RowArgument(int rowIndex)
		{
			this.rowIndex = rowIndex;
		}
	}
}
