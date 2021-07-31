using ReactiveUI;
using System.Collections.Generic;
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

		public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
		private string _name;

		public string Size { get => _size; set => this.RaiseAndSetIfChanged(ref _size, value); }
		private string _size;

		public ReactiveCommand<Unit, Unit> Build { get; set; }

		public ViewModel(MainWindow window)
		{
			model = new Model(window);

			Build = ReactiveCommand.Create(() => model.BuildImage(new ContentSpecs { Name = Name, Size = Size }));
		}
	}
}
