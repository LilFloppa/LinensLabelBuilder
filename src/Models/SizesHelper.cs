using ReactiveUI;
using System;
using System.Collections.Generic;

namespace LabelBuilder.Models
{
	public class LinenSpecs : ReactiveObject
	{
		public int DuvetCoverWidth { get; set; }
		public int DuvetCoverHeight { get; set; }
		public int BedsheetWidth { get; set; }
		public int BedsheetHeight { get; set; }
		public int PillowcaseWidth { get; set; }
		public int PillowcaseHeight { get; set; }

		public int DuvetCoverCount { get; set; } = 1;
		public int BedsheetCount { get; set; } = 1;
		public int PillowcaseCount { get; set; } = 2;
	}

	public static class SizesHelper
	{
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
					return new LinenSpecs 
					{ 
						DuvetCoverWidth = 180, DuvetCoverHeight = 200, 
						BedsheetWidth = 220, BedsheetHeight = 220, 
						PillowcaseWidth = 70, PillowcaseHeight = 70 
					};
				case "1.5-спальный": 
					return new LinenSpecs 
					{ 
						DuvetCoverWidth = 150, DuvetCoverHeight = 200,
						BedsheetWidth = 150, BedsheetHeight = 220,
						PillowcaseWidth = 70, PillowcaseHeight = 70
					};
				case "евро 1": 
					return new LinenSpecs 
					{
						DuvetCoverWidth = 200, DuvetCoverHeight = 220,
						BedsheetWidth = 240, BedsheetHeight = 220,
						PillowcaseWidth = 70, PillowcaseHeight = 70
					};
				case "семейный": 
					return new LinenSpecs 
					{
						DuvetCoverWidth = 150, DuvetCoverHeight = 200,
						BedsheetWidth = 220, BedsheetHeight = 220,
						PillowcaseWidth = 70, PillowcaseHeight = 70,
						DuvetCoverCount = 2 
					};
				default:
					throw new ArgumentException("Unknown bed size");
			};
		}
	}
}
