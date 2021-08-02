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
		}
	}
}
