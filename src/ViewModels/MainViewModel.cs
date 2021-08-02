using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Windows.Controls;
using LabelBuilder.Models;
using LabelBuilder.Validation;

namespace LabelBuilder.ViewModels
{
	class MainViewModel : ReactiveObject
	{
		public Model Model { get; set; }

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

		MainViewModelValidator validator = new();

		public MainViewModel(MainWindow window)
		{
			Model = new Model(window);
			ContentSpecs = Model.ContentSpecs;

			var addSpecCanExecute = this.WhenAnyValue(
				vm => vm.Name,
				vm => vm.Size,
				vm => vm.HasElasticBedsheet,
				vm => vm.ElasticBedsheetWidth,
				vm => vm.ClothName,
				vm => vm.Price,
				(name, size, elastic, elasticWidth, cloth, price) => validator.Validate(this).IsValid);

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
				FormatParametersWindow formatWindow = new FormatParametersWindow();
				var vm = new FormatParametersViewModel(this);
				formatWindow.DataContext = vm;
				formatWindow.Show();
				vm.SetSpecs.Subscribe(_ => formatWindow.Close());			
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

	
}
