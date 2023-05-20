using System.IO.Compression;
using System.Text;

using WebMarkupMin.Core;

namespace TestExecutor.Utilities;

public class Compression
{
	public static Byte[] Minify(Byte[] testReport)
	{
		var htmlReportString = Encoding.Default.GetString(testReport);
		var htmlMinifier = new HtmlMinifier();
		var result = htmlMinifier.Minify(htmlReportString, generateStatistics: false);
		var minifiedHtmlReportString = result.MinifiedContent;

		return Encoding.Default.GetBytes(minifiedHtmlReportString);
	}

	public static Byte[] Compress(Byte[] testReport)
	{
		var output = new MemoryStream();

		using (var deflateStream = new DeflateStream(output, CompressionLevel.SmallestSize))
		{
			deflateStream.Write(testReport, 0, testReport.Length);
		}

		return output.ToArray();
	}

	public static Byte[] Decompress(Byte[] testReport)
	{
		var input = new MemoryStream(testReport);
		var output = new MemoryStream();

		using (var deflateStream = new DeflateStream(input, CompressionMode.Decompress))
		{
			deflateStream.CopyToAsync(output).Wait();
		}

		return output.ToArray();
	}
}