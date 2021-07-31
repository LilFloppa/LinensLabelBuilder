using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Windows.Controls;
using LabelBuilder.Models;

namespace LabelBuilder
{

	class ViewModel : ReactiveObject
	{
		public Model Model { get; set; }

		public FormatViewModel FormatViewModel { get; set; }

		public ObservableCollection<ContentSpec> ContentSpecs { get; set; }

		public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
		private string _name = "";

		public string Size { get => _size; set => this.RaiseAndSetIfChanged(ref _size, value); }
		private string _size = "";

		public bool HasElasticBedsheet { get => _hasElasticBedsheet; set => this.RaiseAndSetIfChanged(ref _hasElasticBedsheet, value); }
		private bool _hasElasticBedsheet = false;

		public string ElasticBedsheetWidth { get => _elasticBedsheetWidth; set => this.RaiseAndSetIfChanged(ref _elasticBedsheetWidth, value); }
		private string _elasticBedsheetWidth = "";

		public string ClothName { get => _clothName; set => this.RaiseAndSetIfChanged(ref _clothName, value); }
		private string _clothName = "";

		public string Price { get => _price; set => this.RaiseAndSetIfChanged(ref _price, value); }
		private string _price = "";

		public ContentSpec CurrentContentSpec =>
			new ContentSpec
			{
				Name = Name,
				Size = Size,
				ClothName = ClothName,
				Price = Price,
				HasElasticBedsheet = HasElasticBedsheet,
				ElasticBedsheetWidth = ElasticBedsheetWidth
			};

		public ReactiveCommand<Unit, Unit> AddSpec { get; set; }
		public ReactiveCommand<Unit, Unit> DeleteSelectedSpecs { get; set; }

		public ReactiveCommand<Unit, Unit> Exit { get; set; }
		public ReactiveCommand<Unit, Unit> Format { get; set; }
		public ReactiveCommand<Unit, Unit> Print { get; set; }

		public ViewModel(MainWindow window)
		{
			Model = new Model(window);
			FormatViewModel = new FormatViewModel(this);
			ContentSpecs = Model.ContentSpecs;

			var addSpecCanExecute = this.WhenAnyValue(
				vm => vm.Name,
				vm => vm.Size,
				vm => vm.HasElasticBedsheet,
				vm => vm.ElasticBedsheetWidth,
				vm => vm.ClothName,
				vm => vm.Price,
				(name, size, hasElastic, elasticWidth, clothName, price) =>
				{
					bool output = name != "" && size != "" && clothName != "" && int.TryParse(price, out _);

					if (hasElastic)
						output = output && ElasticBedsheetWidth != "";

					return output;
				});

			AddSpec = ReactiveCommand.Create(() => Model.ContentSpecs.Add(CurrentContentSpec), addSpecCanExecute);

			DeleteSelectedSpecs = ReactiveCommand.Create(() =>
			{
				List<ContentSpec> selected = new();
				foreach (var item in window.SpecsListBox.SelectedItems)
					selected.Add((ContentSpec)item);

				foreach (var item in selected)
					ContentSpecs.Remove(item);
			});

			Exit = ReactiveCommand.Create(() =>
			{
				window.Close();
				App.Current.Shutdown();
			});

			Format = ReactiveCommand.Create(() =>
			{
				FormatWindow formatWindow = new FormatWindow();
				formatWindow.DataContext = FormatViewModel;
				formatWindow.Show();
				FormatViewModel.SetSpecs.Subscribe(_ => formatWindow.Close());
			});

			Print = ReactiveCommand.Create(() =>
			{
				List<DrawingVisual> images = Model.BuildImages();

				PrintDialog dialog = new PrintDialog();
				if (dialog.ShowDialog() == true)
				{
					foreach (var image in images)
						dialog.PrintVisual(image, "Visual");
				}
			});
		}
	}

	class FormatViewModel : ReactiveObject
	{
		public FormatViewModel(ViewModel parent)
		{
			var setSpecsCanExecute = this.WhenAnyValue(
				vm => vm.FontSize,
				vm => vm.Margin,
				vm => vm.SizesOffset,
				(fontSize, margin, offset) => ValidateField(fontSize) && ValidateField(margin) && ValidateField(offset));
			SetSpecs = ReactiveCommand.Create(() =>
			{
				parent.Model.FormatSpecs = new FormatSpecs { FontSize = int.Parse(FontSize), Margin = int.Parse(Margin), SizesOffset = int.Parse(SizesOffset) };
			}, setSpecsCanExecute);

			FontSize = parent.Model.FormatSpecs.FontSize.ToString();
			Margin = parent.Model.FormatSpecs.Margin.ToString();
			SizesOffset = parent.Model.FormatSpecs.SizesOffset.ToString();
		}

		public string FontSize { get => _fontSize; set => this.RaiseAndSetIfChanged(ref _fontSize, value); }
		private string _fontSize;

		public string Margin { get => _margin; set => this.RaiseAndSetIfChanged(ref _margin, value); }
		private string _margin;

		public string SizesOffset { get => _sizesOffset; set => this.RaiseAndSetIfChanged(ref _sizesOffset, value); }
		private string _sizesOffset;

		public ReactiveCommand<Unit, Unit> SetSpecs { get; set; }

		private bool ValidateField(string numberString) => int.TryParse(numberString, out _);
	}
}
