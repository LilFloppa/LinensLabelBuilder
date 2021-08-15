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

		#region Common Specs
		public string Name { get => _name; set { this.RaiseAndSetIfChanged(ref _name, value); Validate(); } }
		private string _name = "";

		public string Size { get => _size; set { this.RaiseAndSetIfChanged(ref _size, value); SetSizes(); Validate(); } }
		private string _size = "";

		public string ClothName { get => _clothName; set { this.RaiseAndSetIfChanged(ref _clothName, value); Validate(); } }
		private string _clothName = "";

		public string Price { get => _price; set { this.RaiseAndSetIfChanged(ref _price, value); Validate(); } }
		private string _price = "";
		#endregion

		#region Sizes
		public string DuvetCoverWidth { get => _duvetCoverWidth; set { this.RaiseAndSetIfChanged(ref _duvetCoverWidth, value); Validate(); } }
		private string _duvetCoverWidth = "";

		public string DuvetCoverHeight { get => _duvetCoverHeight; set { this.RaiseAndSetIfChanged(ref _duvetCoverHeight, value); Validate(); } }
		private string _duvetCoverHeight = "";

		public string BedsheetWidth { get => _bedsheetWidth; set { this.RaiseAndSetIfChanged(ref _bedsheetWidth, value); Validate(); } }
		private string _bedsheetWidth = "";

		public string BedsheetHeight { get => _bedsheetHeight; set { this.RaiseAndSetIfChanged(ref _bedsheetHeight, value); Validate(); } }
		private string _bedsheetHeight = "";

		public string PillowcaseWidth { get => _pillowcaseWidth; set { this.RaiseAndSetIfChanged(ref _pillowcaseWidth, value); Validate(); } }
		private string _pillowcaseWidth = "";

		public string PillowcaseHeight { get => _pillowcaseHeight; set { this.RaiseAndSetIfChanged(ref _pillowcaseHeight, value); Validate(); } }
		private string _pillowcaseHeight = "";

		public string DuvetCoverCount { get => _duvetCoverCount; set { this.RaiseAndSetIfChanged(ref _duvetCoverCount, value); Validate(); } }
		private string _duvetCoverCount = "";

		public string BedsheetCount { get => _bedsheetCount; set { this.RaiseAndSetIfChanged(ref _bedsheetCount, value); Validate(); } }
		private string _bedsheetCount = "";

		public string PillowcaseCount { get => _pillowcaseCount; set { this.RaiseAndSetIfChanged(ref _pillowcaseCount, value); Validate(); } }
		private string _pillowcaseCount = "";
		#endregion

		#region Elastic Bedsheet
		public bool HasElasticBedsheet { get => _hasElasticBedsheet; set { this.RaiseAndSetIfChanged(ref _hasElasticBedsheet, value); Validate(); } }
		private bool _hasElasticBedsheet = false;

		public string ElasticBedsheetWidth { get => _elasticBedsheetWidth; set { this.RaiseAndSetIfChanged(ref _elasticBedsheetWidth, value); Validate(); } }
		private string _elasticBedsheetWidth = "";
		#endregion

		public bool IsValid { get => _isValid; set => this.RaiseAndSetIfChanged(ref _isValid, value); }
		private bool _isValid = false;

		private void Validate() => IsValid = validator.Validate(this).IsValid;

		private void SetSizes()
		{
			if (Size != "")
			{
				LinenSpecs specs = SizesHelper.GetLinenSpecs(Size);
				DuvetCoverWidth = specs.DuvetCoverWidth.ToString();
				DuvetCoverHeight = specs.DuvetCoverHeight.ToString();
				BedsheetWidth = specs.BedsheetWidth.ToString();
				BedsheetHeight = specs.BedsheetHeight.ToString();
				PillowcaseWidth = specs.PillowcaseWidth.ToString();
				PillowcaseHeight = specs.PillowcaseHeight.ToString();
				DuvetCoverCount = specs.DuvetCoverCount.ToString();
				BedsheetCount = specs.BedsheetCount.ToString();
				PillowcaseCount = specs.PillowcaseCount.ToString();
			}
		}

		private ContentSpec CurrentContentSpec()
		{
			var spec = new ContentSpec
			{
				Name = Name,
				Size = Size,
				ClothName = ClothName,
				Price = Price,
				HasElasticBedsheet = HasElasticBedsheet,
				ElasticBedsheetWidth = ElasticBedsheetWidth
			};

			spec.SizeSpecs = new LinenSpecs()
			{
				DuvetCoverWidth = int.Parse(DuvetCoverWidth),
				DuvetCoverHeight = int.Parse(DuvetCoverHeight),
				BedsheetWidth = int.Parse(BedsheetWidth),
				BedsheetHeight = int.Parse(BedsheetHeight),
				PillowcaseWidth = int.Parse(PillowcaseWidth),
				PillowcaseHeight = int.Parse(PillowcaseHeight),
				DuvetCoverCount = int.Parse(DuvetCoverCount),
				BedsheetCount = int.Parse(BedsheetCount),
				PillowcaseCount = int.Parse(PillowcaseCount)
			};


			return spec;
		}

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

			AddSpec = ReactiveCommand.Create(
				() => Model.ContentSpecs.Add(CurrentContentSpec()),
				this.WhenAnyValue(vm => vm.IsValid).Select(isValid => isValid));

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
