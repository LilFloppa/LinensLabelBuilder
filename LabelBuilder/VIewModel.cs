using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace LabelBuilder
{
	class ViewModel : ReactiveObject
	{
		Model model;

		public IEnumerable<string> Sizes { get; set; } = new List<string>()
		{
			"2-спальный"
		};

		public ObservableCollection<BitmapSpecsPair> BitmapSpecsPairs { get; set; }

		public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
		private string _name;

		public string Size { get => _size; set => this.RaiseAndSetIfChanged(ref _size, value); }
		private string _size;

		public ReactiveCommand<Unit, Unit> Build { get; set; }
		public ReactiveCommand<Unit, Unit> DeleteSelectedSpec { get; set; }

		public ViewModel(MainWindow window)
		{
			model = new Model(window);
			BitmapSpecsPairs = model.BitmapSpecsPairs;

			Build = ReactiveCommand.Create(() => model.BuildImage(new ContentSpecs { Name = Name, Size = Size }));
			DeleteSelectedSpec = ReactiveCommand.Create(() => 
			{
				List<BitmapSpecsPair> selected = new();
				foreach (var item in window.SpecsListBox.SelectedItems)
					selected.Add((BitmapSpecsPair)item);

				foreach (var item in selected)
					BitmapSpecsPairs.Remove(item); 
			});
		}
	}
}
