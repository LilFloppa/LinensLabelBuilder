using FluentValidation;
using LabelBuilder.Extensions;
using LabelBuilder.ViewModels;

namespace LabelBuilder.Validation
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
