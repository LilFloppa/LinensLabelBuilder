using System;
using System.Collections.Generic;

namespace LabelBuilder.Models
{
	public class LinenSpecs
	{
		public (int width, int height) DuvetCoverSize;
		public (int width, int height) BedsheetSize;
		public (int width, int height) PillowcaseSize;

		public int DuvetCoverCount = 1;
		public int BedsheetCount = 1;
		public int PillowcaseCount = 2;
	}

	public static class SizesHelper
	{
		public static IEnumerable<string> ElasticBedsheetWidth { get; set; } = new List<string>() { "100", "140", "160", "180" };

		public static IEnumerable<string> BedSizes { get; set; } = new List<string>()
		{
			"2-спальный",
			"1.5-спальный",
			"евро 1",
			"семейный"
		};

		public static LinenSpecs GetLinenSpecs(string bedSize)
		{
			switch (bedSize)
			{
				case "2-спальный": 
					return new LinenSpecs { DuvetCoverSize = (180, 200), BedsheetSize = (220, 220), PillowcaseSize = (70, 70) };
				case "1.5-спальный": 
					return new LinenSpecs { DuvetCoverSize = (150, 200), BedsheetSize = (150, 220), PillowcaseSize = (70, 70) };
				case "евро 1": 
					return new LinenSpecs { DuvetCoverSize = (200, 220), BedsheetSize = (240, 220), PillowcaseSize = (70, 70) };
				case "семейный": 
					return new LinenSpecs { DuvetCoverSize = (150, 200), BedsheetSize = (220, 220), PillowcaseSize = (70, 70), DuvetCoverCount = 2 };
				default:
					throw new ArgumentException("Unknown bed size");
			};
		}
	}
}
