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
		public string ClothName { get; set; }
		public int Price { get; set; }

		public bool HasElasticBedsheet { get; set; }
		public int ElasticBedsheetWidth { get; set; }
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

			FormatSpecs = new FormatSpecs { FontSize = 24, Margin = 20, SizesOffset = 300 };
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

		private void DrawText(GeometryGroup group, FormattedText formatted, double x, double y)
		{
			Geometry geometry = formatted.BuildGeometry(new Point(x, y));
			group.Children.Add(geometry);
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
			LinenSpecs linenSpecs = SizesHelper.GetLinenSpecs(contentSpec.Size);

			GeometryGroup group = BeginDraw();

			double yOffset = 0.0;
			FormattedText text = PrepareText("ИВАНОВСКИЙ ТЕКСТИЛЬ", "Calibri", FormatSpecs.FontSize);
			DrawText(group, text, 0.0, 0.0);

			yOffset += text.Height;
			text = PrepareText("Постельное белье \"Российский хлопок\"", "Calibri", FormatSpecs.FontSize);
			DrawText(group, text, 0.0, yOffset);

			yOffset += text.Height;
			text = PrepareText($"КПБ \"{ contentSpec.Name }\" { contentSpec.Size }", "Calibri", FormatSpecs.FontSize);
			DrawText(group, text, 0.0, yOffset);

			if (contentSpec.HasElasticBedsheet)
			{
				yOffset += text.Height;
				text = PrepareText("Простынь на резинке:", "Calibri", FormatSpecs.FontSize);
				DrawText(group, text, 0.0, yOffset);

				text = PrepareText($"{ contentSpec.ElasticBedsheetWidth } x 200", "Calibri", FormatSpecs.FontSize);
				DrawText(group, text, FormatSpecs.SizesOffset, yOffset);
			}
			else
			{
				yOffset += text.Height;
				text = PrepareText("Пододеяльник:", "Calibri", FormatSpecs.FontSize);
				DrawText(group, text, 0.0, yOffset);

				string line = $"{ linenSpecs.DuvetCoverSize.width } x { linenSpecs.DuvetCoverSize.height }";
				if (linenSpecs.DuvetCoverCount > 1)
					line += $" - { linenSpecs.DuvetCoverCount } шт.";

				text = PrepareText(line, "Calibri", FormatSpecs.FontSize);
				DrawText(group, text, FormatSpecs.SizesOffset, yOffset);

				yOffset += text.Height;
				text = PrepareText("Простынь:", "Calibri", FormatSpecs.FontSize);
				DrawText(group, text, 0.0, yOffset);

				text = PrepareText($"{ linenSpecs.BedsheetSize.width } x { linenSpecs.BedsheetSize.height }", "Calibri", FormatSpecs.FontSize);
				DrawText(group, text, FormatSpecs.SizesOffset, yOffset);
			}

			yOffset += text.Height;
			text = PrepareText("Наволочки:", "Calibri", FormatSpecs.FontSize);
			DrawText(group, text, 0.0, yOffset);

			text = PrepareText(
				$"{ linenSpecs.PillowcaseSize.width } x { linenSpecs.PillowcaseSize.height } - { linenSpecs.PillowcaseCount } шт.", 
				"Calibri", 
				FormatSpecs.FontSize);
			DrawText(group, text, FormatSpecs.SizesOffset, yOffset);

			yOffset += text.Height;
			text = PrepareText($"Ткань: { contentSpec.ClothName }", "Calibri", FormatSpecs.FontSize);
			DrawText(group, text, 0.0, yOffset);

			yOffset += text.Height;
			text = PrepareText($"Цена: { contentSpec.Price }руб.", "Calibri", FormatSpecs.FontSize);
			DrawText(group, text, 0.0, yOffset);

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
