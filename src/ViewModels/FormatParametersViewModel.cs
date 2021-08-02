using LabelBuilder.Models;
using LabelBuilder.Validation;
using ReactiveUI;
using System.Reactive;

namespace LabelBuilder.ViewModels
{
	class FormatParametersViewModel : ReactiveObject
	{
		FormatParametersValidator validator = new();

		public FormatParametersViewModel(MainViewModel parent)
		{
			var setSpecsCanExecute = this.WhenAnyValue(
				vm => vm.FontSize,
				vm => vm.Margin,
				vm => vm.SizesOffset,
				(fontSize, margin, offset) => validator.Validate(this).IsValid);

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
	}
}
