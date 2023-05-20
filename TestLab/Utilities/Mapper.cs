#nullable disable
namespace TestLab.Utilities;

public class DataPool
{
	public String Parameter { get; set; }

	public String Value { get; set; }

	public String Description { get; set; }
}

public class Mapper
{
	public static List<DataPool> MapData(Byte[] testData)
	{
		var memoryStream = new MemoryStream(testData);
		var workBook = new XLWorkbook(memoryStream);
		var workSheet = workBook.Worksheet(1);
		var dataList = new List<DataPool>();

		foreach (var cell in workSheet.RangeUsed().RowsUsed())
		{
			var dataPool = new DataPool()
			{
				Parameter = cell.Cell(1).GetString(),
				Value = cell.Cell(2).GetString(),
				Description = cell.Cell(3).GetString()
			};

			dataList.Add(dataPool);
		}

		return dataList;
	}
}