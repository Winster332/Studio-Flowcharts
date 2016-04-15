using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_Test.LMD_GUI
{
	/// <summary>
	/// Логика взаимодействия для LMD_WindowClaculate.xaml
	/// </summary>
	public partial class LMD_WindowClaculate : Window
	{
		#region variables
		public String Text
		{
			get
			{
				return txtView.Text;
			}
			set
			{
				txtView.Text = value;
			}
		}
		#endregion
		#region LMD_WindowCalculate
		public LMD_WindowClaculate()
		{
			InitializeComponent();

			#region add command
			this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));
			#endregion
		}
		#endregion
		#region Click buttons F(X)
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			String fx = ((Button)sender).Tag.ToString();
			String t = "";

			switch (fx)
			{
				case "cos": t = "Cos()"; break;
				case "sin": t = "Sin()"; break;
				case "sqrt": t = "Sqrt()"; break;
				case "arccos": t = "Arccos()"; break;
				case "arctg": t = "Arctg()"; break;
				case "exp": t = "Exp()"; break;
				case "fabs": t = "Fabs()"; break;
				case "log": t = "Log()"; break;
				case "log10": t = "Log10()"; break;
				case "tan": t = "Tan()"; break;
				case "asin": t = "Asin()"; break;
				case "atan": t = "Atan()"; break;
				case "pow": t = "Pow()"; break;
				case "+": t = "+"; break;
				case "-": t = "-"; break;
				case "*": t = "*"; break;
				case "/": t = "/"; break;
				case "and": t = "&&"; break;
				case "or": t = "||"; break;
				case "=": t = "="; break;
				case "==": t = "=="; break;
			}

			txtView.Text += t;
		}
		#endregion
		#region Button click save F(x)
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		#endregion
		#region Button click close manager
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		#endregion
		#region CreateVariables
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			LMD_GUI.LMD_WindowManagerVariables wmv = new LMD_WindowManagerVariables();
			wmv.ShowDialog();
		}
		#endregion

		#region Commands
		private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
		}

		private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
		}

		private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}

		private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
		{
			if (this.WindowState == System.Windows.WindowState.Normal)
			{
				SystemCommands.MaximizeWindow(this);
			}
			else if (this.WindowState == System.Windows.WindowState.Maximized)
			{
				SystemCommands.RestoreWindow(this);
			}
		}

		private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MinimizeWindow(this);
		}

		private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
		{
			SystemCommands.RestoreWindow(this);
		}
		#endregion
	}
}
