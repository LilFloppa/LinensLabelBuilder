using FluentValidation;
using LabelBuilder.Extensions;

namespace LabelBuilder.ViewModels
{
	class ViewModelValidator : AbstractValidator<ViewModel>
	{
		public ViewModelValidator()
		{
			RuleFor(vm => vm.Name).NotEmpty();
			RuleFor(vm => vm.Size).NotEmpty();
			RuleFor(vm => vm.ClothName).NotEmpty();
			RuleFor(vm => vm.Price).Must(price => price.IsInt32());
			RuleFor(vm => vm.ElasticBedsheetWidth).Must(width => width.IsInt32()).When(vm => vm.HasElasticBedsheet);
		}
	}
}
