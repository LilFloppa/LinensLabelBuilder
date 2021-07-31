using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Windows.Controls;

namespace LabelBuilder
{
	class ViewModel : ReactiveObject
	{
		public Model Model { get; set; }

		public FormatViewModel FormatViewModel { get; set; }

		public IEnumerable<string> Sizes { get; set; } = new List<string>()
		{
			"2-спальный",
			"1.5-спальный",
			"евро",
			"семейный"
		};

		public ObservableCollection<BitmapSpecsPair> BitmapSpecsPairs { get; set; }

		public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
		private string _name;

		public string Size { get => _size; set => this.RaiseAndSetIfChanged(ref _size, value); }
		private string _size;

		public ContentSpecs ContentSpecs => new ContentSpecs { Name = Name, Size = Size };

		public ReactiveCommand<Unit, Unit> Build { get; set; }
		public ReactiveCommand<Unit, Unit> DeleteSelectedSpec { get; set; }

		public ReactiveCommand<Unit, Unit> Exit { get; set; }
		public ReactiveCommand<Unit, Unit> Format { get; set; }
		public ReactiveCommand<Unit, Unit> Print { get; set; }

		public ViewModel(MainWindow window)
		{
			Model = new Model(window);
			FormatViewModel = new FormatViewModel(this);
			BitmapSpecsPairs = Model.BitmapSpecsPairs;

			Build = ReactiveCommand.Create(() => Model.BuildImage(ContentSpecs));
			DeleteSelectedSpec = ReactiveCommand.Create(() =>
			{
				List<BitmapSpecsPair> selected = new();
				foreach (var item in window.SpecsListBox.SelectedItems)
					selected.Add((BitmapSpecsPair)item);

				foreach (var item in selected)
					BitmapSpecsPairs.Remove(item);
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
				DrawingVisual visual = Model.SaveResult();

				PrintDialog dialog = new PrintDialog();
				if (dialog.ShowDialog() == true)
					dialog.PrintVisual(visual, "Visual");
			});
		}
	}

	class FormatViewModel : ReactiveObject
	{
		public FormatViewModel(ViewModel parent)
		{
			SetSpecs = ReactiveCommand.Create(() =>
			{
				parent.Model.FormatSpecs = new FormatSpecs { FontSize = FontSize, Margin = Margin, SizesOffset = SizesOffset };
			});

			SetSpecs.Execute().Subscribe();
		}

		public int FontSize { get => _fontSize; set => this.RaiseAndSetIfChanged(ref _fontSize, value); }
		private int _fontSize = 24;

		public int Margin { get => _margin; set => this.RaiseAndSetIfChanged(ref _margin, value); }
		private int _margin = 20;

		public int SizesOffset { get => _sizesOffset; set => this.RaiseAndSetIfChanged(ref _sizesOffset, value); }
		private int _sizesOffset = 200;

		public ReactiveCommand<Unit, Unit> SetSpecs { get; set; }
	}
}
