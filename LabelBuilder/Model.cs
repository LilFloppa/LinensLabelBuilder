using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelBuilder
{
	public class ContentSpecs
	{
		public string Name { get; set; }
		public string Size { get; set; }
		public (int, int) DuvetCoverSize { get; set; }
		public (int, int) BedsheetSize { get; set; }
		public (int, int) PillowcaseSize { get; set; }
		public string ClothName { get; set; }
		public int Price { get; set; }
	}

	public class FormatSpecs
	{
		public int FontSize { get; set; }
		public int Margin { get; set; }
		public int SizesOffset { get; set; }
	}

	public class BitmapSpecsPair
	{
		public DrawingVisual Bitmap { get; set; }
		public ContentSpecs Specs { get; set; }
	}

	public class Model
	{

		public FormatSpecs FormatSpecs { get; set; }
		public MainWindow Window { get; set; }

		public readonly ObservableCollection<BitmapSpecsPair> BitmapSpecsPairs = new();

		public Model(MainWindow window)
		{
			Window = window;
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

		public void AddContentSpecs(ContentSpecs specs)
		{
			BitmapSpecsPairs.Add(new BitmapSpecsPair { Specs = specs, Bitmap = null });
		}

		public void BuildImage(ContentSpecs contentSpecs)
		{
			var group = BeginDraw();

			double yOffset = 0.0;
			var text = PrepareText("ИВАНОВСКИЙ ТЕКСТИЛЬ", "Calibri", 24);
			DrawText(group, text, 0.0, 0.0);

			yOffset += text.Height;
			text = PrepareText("Постельное белье \"Российский хлопок\"", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			yOffset += text.Height;
			text = PrepareText($"КПБ \"{ contentSpecs.Name }\" { contentSpecs.Size }", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			yOffset += text.Height;
			text = PrepareText("Пододеяльник:", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			text = PrepareText("180 x 200", "Calibri", 24);
			DrawText(group, text, FormatSpecs.SizesOffset, yOffset);

			yOffset += text.Height;
			text = PrepareText("Простынь:", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			text = PrepareText("220 x 220", "Calibri", 24);
			DrawText(group, text, FormatSpecs.SizesOffset, yOffset);

			yOffset += text.Height;
			text = PrepareText("Наволочки:", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			text = PrepareText("70 x 70 - 2 шт.", "Calibri", 24);
			DrawText(group, text, FormatSpecs.SizesOffset, yOffset);

			yOffset += text.Height;
			text = PrepareText("Ткань: бязь премиум", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			yOffset += text.Height;
			text = PrepareText("Цена: 2800 руб.", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			RenderTargetBitmap bitmap = new RenderTargetBitmap(550, (int)yOffset, 96, 96, PixelFormats.Pbgra32);

			var visual = EndDraw(group);

			BitmapSpecsPairs.Add(new BitmapSpecsPair { Specs = contentSpecs, Bitmap = visual });
		}

		public DrawingVisual SaveResult()
		{
			double height = 0.0;
			double width = BitmapSpecsPairs[0].Bitmap.ContentBounds.Width + FormatSpecs.Margin * 2;

			foreach (var pair in BitmapSpecsPairs)
				height += pair.Bitmap.ContentBounds.Height + FormatSpecs.Margin * 2;

			DrawingVisual merged = new DrawingVisual();

			double y = 0.0;
			using (var dc = merged.RenderOpen())
			{
				foreach (var pair in BitmapSpecsPairs)
				{
					y += FormatSpecs.Margin;
					var visualBrush = new VisualBrush(pair.Bitmap);
					var elementSize = new Size(pair.Bitmap.ContentBounds.Width, pair.Bitmap.ContentBounds.Height);
					dc.DrawRectangle(visualBrush, null, new Rect(new Point(FormatSpecs.Margin, y), elementSize));
					y += pair.Bitmap.ContentBounds.Height + FormatSpecs.Margin;
				}
			}

			return merged;
		}
	}
}
