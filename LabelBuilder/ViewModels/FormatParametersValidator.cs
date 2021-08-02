using FluentValidation;
using LabelBuilder.Extensions;

namespace LabelBuilder.ViewModels
{
	class FormatParametersValidator : AbstractValidator<FormatParametersViewModel>
	{
		public FormatParametersValidator()
		{
			RuleFor(vm => vm.FontSize).Must(size => size.IsInt32());
			RuleFor(vm => vm.Margin).Must(margin => margin.IsInt32());
			RuleFor(vm => vm.SizesOffset).Must(offset => offset.IsInt32());
		}
	}
}
