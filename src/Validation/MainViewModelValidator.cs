using FluentValidation;
using LabelBuilder.Extensions;
using LabelBuilder.ViewModels;

namespace LabelBuilder.Validation
{
	class MainViewModelValidator : AbstractValidator<MainViewModel>
	{
		public MainViewModelValidator()
		{
			RuleFor(vm => vm.Name).NotEmpty();
			RuleFor(vm => vm.Size).NotEmpty();
			RuleFor(vm => vm.ClothName).NotEmpty();
			RuleFor(vm => vm.Price).Must(price => price.IsInt32());
			RuleFor(vm => vm.ElasticBedsheetWidth).Must(width => width.IsInt32()).When(vm => vm.HasElasticBedsheet);

			RuleFor(vm => vm.DuvetCoverWidth).Must(width => width.IsInt32()).When(vm => vm.HasCustomSize);
			RuleFor(vm => vm.DuvetCoverHeight).Must(height => height.IsInt32()).When(vm => vm.HasCustomSize);
			RuleFor(vm => vm.BedsheetWidth).Must(width => width.IsInt32()).When(vm => vm.HasCustomSize);
			RuleFor(vm => vm.BedsheetHeight).Must(height => height.IsInt32()).When(vm => vm.HasCustomSize);
			RuleFor(vm => vm.PillowcaseWidth).Must(width => width.IsInt32()).When(vm => vm.HasCustomSize);
			RuleFor(vm => vm.PillowcaseHeight).Must(height => height.IsInt32()).When(vm => vm.HasCustomSize);
			RuleFor(vm => vm.DuvetCoverCount).Must(count => count.IsInt32()).When(vm => vm.HasCustomSize);
			RuleFor(vm => vm.BedsheetCount).Must(count => count.IsInt32()).When(vm => vm.HasCustomSize);
			RuleFor(vm => vm.PillowcaseCount).Must(count => count.IsInt32()).When(vm => vm.HasCustomSize);
		}
	}
}
