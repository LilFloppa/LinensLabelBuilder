﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace LabelBuilder.Models
{
	public class ContentSpec
	{
		public string Name { get; set; }
		public string Size { get; set; }

		public bool HasCustomSize { get; set; }
		public LinenSpecs CustomSizeSpecs { get; set; }

		public string ClothName { get; set; }
		public string Price { get; set; }

		public bool HasElasticBedsheet { get; set; }
		public string ElasticBedsheetWidth { get; set; }
	}

	public class FormatSpecs
	{
		public int FontSize { get; set; }
		public int Margin { get; set; }
		public int SizesOffset { get; set; }
	}

	public class Model
	{
		public FormatSpecs FormatSpecs { get; set; }
		public MainWindow Window { get; set; }
		public ObservableCollection<ContentSpec> ContentSpecs = new();

		public Model(MainWindow window)
		{
			Window = window;

			FormatSpecs = new FormatSpecs { FontSize = 24, Margin = 20, SizesOffset = 240 };
		}

		private FormattedText PrepareText(string text, string font, int size)
		{
			Typeface typeface = new Typeface(new FontFamily(font), FontStyles.Normal, FontWeights.Thin, FontStretches.Normal);
			FormattedText formatted = new FormattedText(
				text,
				CultureInfo.InvariantCulture,
				FlowDirection.LeftToRight,
				typeface,
				size,
				Brushes.Black,
				VisualTreeHelper.GetDpi(Window).PixelsPerDip);

			return formatted;
		}

		private GeometryGroup BeginDraw() => new GeometryGroup();

		private FormattedText DrawText(GeometryGroup group, string text, double x, double y)
		{
			FormattedText formatted = PrepareText(text, "Calibri", FormatSpecs.FontSize);
			Geometry geometry = formatted.BuildGeometry(new Point(x, y));
			group.Children.Add(geometry);

			return formatted;
		}

		public DrawingVisual EndDraw(GeometryGroup group)
		{
			DrawingVisual visual = new();
			using (DrawingContext dc = visual.RenderOpen())
				dc.DrawGeometry(Brushes.Black, new Pen(Brushes.Black, 1.0), group);

			return visual;
		}

		public List<DrawingVisual> BuildImages()
		{
			List<DrawingVisual> images = new();

			foreach (var spec in ContentSpecs)
				images.Add(BuildImage(spec));

			return MergeImages(images);
		}

	   DrawingVisual BuildImage(ContentSpec contentSpec)
		{
			LinenSpecs linenSpecs = contentSpec.HasCustomSize ? contentSpec.CustomSizeSpecs : SizesHelper.GetLinenSpecs(contentSpec.Size);

			GeometryGroup group = BeginDraw();

			double yOffset = 0.0;

			FormattedText drawn = DrawText(group, "ИВАНОВСКИЙ ТЕКСТИЛЬ", 0.0, 0.0);
			yOffset += drawn.Height;
			drawn = DrawText(group, "Постельное белье \"Российский хлопок\"", 0.0, yOffset);
			yOffset += drawn.Height;

			drawn = DrawText(group, $"КПБ \"{ contentSpec.Name }\" { contentSpec.Size }", 0.0, yOffset);

			if (contentSpec.HasElasticBedsheet)
			{
				yOffset += drawn.Height;
				drawn = DrawText(group, "Простынь на резинке:", 0.0, yOffset);
				drawn = DrawText(group, $"{ contentSpec.ElasticBedsheetWidth } x 200 x 38", FormatSpecs.SizesOffset, yOffset);
			}
			else
			{
				yOffset += drawn.Height;
				drawn = DrawText(group, "Пододеяльник:", 0.0, yOffset);

				string duvetLine = $"{ linenSpecs.DuvetCoverSize.width } x { linenSpecs.DuvetCoverSize.height }";
				if (linenSpecs.DuvetCoverCount > 1)
					duvetLine += $" - { linenSpecs.DuvetCoverCount } шт.";

				drawn = DrawText(group, duvetLine, FormatSpecs.SizesOffset, yOffset);

				yOffset += drawn.Height;
				drawn = DrawText(group, "Простынь:", 0.0, yOffset);
				drawn = DrawText(group, $"{ linenSpecs.BedsheetSize.width } x { linenSpecs.BedsheetSize.height }", FormatSpecs.SizesOffset, yOffset);
			}

			yOffset += drawn.Height;
			drawn = DrawText(group, "Наволочки:", 0.0, yOffset);

			string pillowLine = $"{ linenSpecs.PillowcaseSize.width } x { linenSpecs.PillowcaseSize.height }";
			if (linenSpecs.PillowcaseCount > 1)
				pillowLine += $" - { linenSpecs.PillowcaseCount } шт.";

			drawn = DrawText(group, pillowLine, FormatSpecs.SizesOffset, yOffset);

			yOffset += drawn.Height;
			drawn = DrawText(group, $"Ткань: { contentSpec.ClothName }", 0.0, yOffset);

			yOffset += drawn.Height;
			drawn = DrawText(group, $"Цена: { contentSpec.Price }руб.", 0.0, yOffset);

			DrawingVisual visual = EndDraw(group);

			return visual;
		}

		List<DrawingVisual> MergeImages(List<DrawingVisual> images)
		{
			List<DrawingVisual> mergedImages = new();

			var chunks = images.ChunkBy(4);

			foreach (var chunk in chunks)
			{
				DrawingVisual merged = new();
				double y = 0.0;
				using (var dc = merged.RenderOpen())
				{
					foreach (var image in chunk)
					{
						y += FormatSpecs.Margin;
						var visualBrush = new VisualBrush(image);
						var elementSize = new Size(image.ContentBounds.Width, image.ContentBounds.Height);
						dc.DrawRectangle(visualBrush, null, new Rect(new Point(FormatSpecs.Margin, y), elementSize));
						y += image.ContentBounds.Height + FormatSpecs.Margin;
					}
				}

				mergedImages.Add(merged);
			}

			return mergedImages;
		}
	}
}
