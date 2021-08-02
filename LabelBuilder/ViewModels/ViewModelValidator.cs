using FluentValidation;

namespace LabelBuilder.ViewModels
{
	class ViewModelValidator : AbstractValidator<ViewModel>
	{
		public ViewModelValidator()
		{
			RuleFor(vm => vm.Name).NotEmpty();
			RuleFor(vm => vm.Size).NotEmpty();
			RuleFor(vm => vm.ClothName).NotEmpty();
			RuleFor(vm => vm.Price).Must(IsNumber);
			RuleFor(vm => vm.ElasticBedsheetWidth).Must(IsNumber).When(vm => vm.HasElasticBedsheet);
		}

		private bool IsNumber(string number) => int.TryParse(number, out _);
	}
}
