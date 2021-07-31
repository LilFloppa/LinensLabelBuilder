using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelBuilder
{
	public class ContentSpecs
	{
		public string Name;
		public string Size;
		public (int, int) DuvetCoverSize;
		public (int, int) BedsheetSize;
		public (int, int) PillowcaseSize;
		public string ClothName;
		public int Price;
	}

	public class FontSpecs
	{
		public string FontName;
		public int FontSize;
	}

	public class ImageSpecs
	{
		public int Width;
		public int Height;
	}

	public class Model
	{
		public MainWindow Window { get; set; }

		private List<RenderTargetBitmap> bitmaps = new();

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

		public void EndDraw(GeometryGroup group, RenderTargetBitmap bitmap)
		{
			DrawingVisual visual = new();
			using (DrawingContext dc = visual.RenderOpen())
				dc.DrawGeometry(Brushes.Black, new Pen(Brushes.Black, 1.0), group);

			bitmap.Render(visual);
		}

		public void BuildImage(ContentSpecs specs)
		{
			RenderTargetBitmap bitmap = new RenderTargetBitmap(550, 300, 96, 96, PixelFormats.Pbgra32);

			var group = BeginDraw();

			double yOffset = 0.0;
			var text = PrepareText("ИВАНОВСКИЙ ТЕКСТИЛЬ", "Calibri", 24);
			DrawText(group, text, 0.0, 0.0);

			yOffset += text.Height;
			text = PrepareText("Постельное белье \"Российский хлопок\"", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			yOffset += text.Height;
			text = PrepareText($"КПБ \"{ specs.Name }\" { specs.Size }", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			yOffset += text.Height;
			text = PrepareText("Пододеяльник:", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			text = PrepareText("180 x 200", "Calibri", 24);
			DrawText(group, text, 200.0, yOffset);

			yOffset += text.Height;
			text = PrepareText("Простынь:", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			text = PrepareText("220 x 220", "Calibri", 24);
			DrawText(group, text, 200.0, yOffset);

			yOffset += text.Height;
			text = PrepareText("Наволочки:", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			text = PrepareText("70 x 70 - 2 шт.", "Calibri", 24);
			DrawText(group, text, 200.0, yOffset);

			yOffset += text.Height;
			text = PrepareText("Ткань: бязь премиум", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			yOffset += text.Height;
			text = PrepareText("Цена: 2800 руб.", "Calibri", 24);
			DrawText(group, text, 0.0, yOffset);

			EndDraw(group, bitmap);

			bitmaps.Add(bitmap);
			bitmaps.Add(bitmap);

			using (var stream = new FileStream(@"C:/repos/result.png", FileMode.Create))
			{
				BitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmap));
				encoder.Save(stream);
			}

			SaveResult();
		}

		public void SaveResult()
		{
			RenderTargetBitmap merged = new RenderTargetBitmap(550, 600, 96.0, 96.0, PixelFormats.Pbgra32);

			double y = 0.0;
			foreach (var bitmap in bitmaps)
			{
				var visual = new DrawingVisual();

				using (var dc = visual.RenderOpen())
				{
					var visualBrush = new ImageBrush(bitmap);
					var elementSize = new Size(bitmap.Width, bitmap.Height);
					dc.DrawRectangle(visualBrush, null, new Rect(new Point(0.0, y), elementSize));
					y += bitmap.Height;
				}
				merged.Render(visual);
			}

			using (var stream = new FileStream(@"C:/repos/merged.png", FileMode.Create))
			{
				BitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(merged));
				encoder.Save(stream);
			}
		}
	}
}
