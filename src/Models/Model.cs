using System;
using System.Collections.Generic;
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
		public LinenSpecs SizeSpecs { get; set; }
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

			FormatSpecs = new FormatSpecs { FontSize = 24, Margin = 10, SizesOffset = 240 };
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

		private void DrawBorder(GeometryGroup group, double x, double y, double width, double height)
		{
			StreamGeometry geomerty = new StreamGeometry();

			using (var context = geomerty.Open())
			{
				context.BeginFigure(new Point(x, y), false, true);
				context.LineTo(new Point(x + width, y), true, false);
				context.LineTo(new Point(x + width, y + height), true, false);
				context.LineTo(new Point(x, y + height), true, false);
				context.LineTo(new Point(x, y), true, false);
			}
			group.Children.Add(geomerty);
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
			LinenSpecs linenSpecs = contentSpec.SizeSpecs;

			GeometryGroup group = BeginDraw();

			double margin = FormatSpecs.Margin;
			double xOffset = 0.0;
			double yOffset = margin;

			FormattedText drawn = DrawText(group, "ИВАНОВСКИЙ ТЕКСТИЛЬ", margin, yOffset);
			yOffset += drawn.Height;
			xOffset = Math.Max(xOffset, drawn.Width);

			drawn = DrawText(group, "Постельное белье \"Российский хлопок\"", margin, yOffset);
			yOffset += drawn.Height;
			xOffset = Math.Max(xOffset, drawn.Width);

			drawn = DrawText(group, $"КПБ \"{ contentSpec.Name }\" { contentSpec.Size }", margin, yOffset);
			yOffset += drawn.Height;
			xOffset = Math.Max(xOffset, drawn.Width);

			if (linenSpecs.DuvetCoverCount > 0)
			{
				drawn = DrawText(group, "Пододеяльник:", margin, yOffset);

				string duvetLine = $"{ linenSpecs.DuvetCoverWidth } x { linenSpecs.DuvetCoverHeight }";
				if (linenSpecs.DuvetCoverCount > 1)
					duvetLine += $" - { linenSpecs.DuvetCoverCount } шт.";

				drawn = DrawText(group, duvetLine, FormatSpecs.SizesOffset, yOffset);
				yOffset += drawn.Height;
				xOffset = Math.Max(xOffset, drawn.Width);
			}

			if (linenSpecs.BedsheetCount > 0)
			{
				string bedsheetLine = "";
				if (contentSpec.HasElasticBedsheet)
				{
					drawn = DrawText(group, "Простынь на резинке:", margin, yOffset);
					bedsheetLine = $"{ contentSpec.ElasticBedsheetWidth } x 200 x 38";
				}
				else
				{
					drawn = DrawText(group, "Простынь:", margin, yOffset);
					bedsheetLine = $"{ linenSpecs.BedsheetWidth } x { linenSpecs.BedsheetHeight }";
				}

				if (linenSpecs.BedsheetCount > 1)
					bedsheetLine += $" - { linenSpecs.BedsheetCount } шт.";

				drawn = DrawText(group, bedsheetLine, FormatSpecs.SizesOffset, yOffset);
				yOffset += drawn.Height;
				xOffset = Math.Max(xOffset, drawn.Width);
			}

			if (linenSpecs.PillowcaseCount > 0)
			{
				drawn = DrawText(group, "Наволочки:", margin, yOffset);

				string pillowLine = $"{ linenSpecs.PillowcaseWidth } x { linenSpecs.PillowcaseHeight }";
				if (linenSpecs.PillowcaseCount > 1)
					pillowLine += $" - { linenSpecs.PillowcaseCount } шт.";

				drawn = DrawText(group, pillowLine, FormatSpecs.SizesOffset, yOffset);
				yOffset += drawn.Height;
				xOffset = Math.Max(xOffset, drawn.Width);
			}

			drawn = DrawText(group, $"Ткань: { contentSpec.ClothName }", margin, yOffset);
			yOffset += drawn.Height;
			xOffset = Math.Max(xOffset, drawn.Width);

			drawn = DrawText(group, $"Цена: { contentSpec.Price }руб.", margin, yOffset);
			yOffset += drawn.Height;
			xOffset = Math.Max(xOffset, drawn.Width);

			DrawBorder(group, 0.0, 0.0, xOffset + 2 * margin, yOffset + margin);
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
