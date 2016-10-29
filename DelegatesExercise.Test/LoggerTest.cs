using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DelegatesExercise.Test 
{
	[TestClass]
	public class LoggerTest
	{
		[TestMethod]
		public void TestTableAndLogger() 
		{
			var table = new TableModel(50, 10);
			table.InsertRow(2);
			table.InsertColumn(4);
			table.Put(0,0,5);
			Assert.AreEqual(table.Get(0, 0), 5);
			
			var logger = new TableLoggerExample(table);
			table.InsertColumn(4);
			table.Put(1, 2, 11);
			table.InsertRow(8);
			table.Get(0, 0); // just to check if the logger logs Get-events

			logger.EndLogging();

			Assert.AreEqual(logger.log[0], "NewColumn:4");
			Assert.AreEqual(logger.log[1], "NewValue:11(1;2)");
			Assert.AreEqual(logger.log[2], "NewRow:8");
		}
	}

	internal class TableLoggerExample
	{
		public readonly List<string> log;
		public List<IDisposable> observers;

		public TableLoggerExample(TableModel table)
		{
			log = new List<string>();
			observers = new List<IDisposable>
			{
				table.AddPutValueObserver(LogPutValue),
				table.AddColumnInsertedObserver(LogInsertedColumn),
				table.AddRowInsertedObserver(LogInsertedRow)
			};
		}

		private void LogPutValue(CellArgument cell) => 
			log.Add($"NewValue:{cell.value}({cell.rowIndex};{cell.columnIndex})");

		private void LogInsertedColumn(ColumnArgument column) => 
			log.Add($"NewColumn:{column.columnIndex}");

		private void LogInsertedRow(RowArgument row) => 
			log.Add($"NewRow:{row.rowIndex}");

		public void EndLogging()
		{
			if (observers != null && observers.Count > 0)
				foreach (var observer in observers)
					observer.Dispose();
		}
	}
}
