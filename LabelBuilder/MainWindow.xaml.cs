using LabelBuilder.ViewModels;
using System.Windows;

namespace LabelBuilder
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new ViewModel(this);
		}
	}
}
