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
	/// Логика взаимодействия для LMD_WindowError.xaml
	/// </summary>
	public partial class LMD_WindowError : Window
	{
		#region variables
		public enum RESULT { OK, HELP }
		public RESULT Result;
		#endregion
		#region WindowError
		public LMD_WindowError(String data)
		{
			InitializeComponent();

			Result = RESULT.OK;
			text.Text = data;

			#region add command
			this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));
			#endregion
		}
		#endregion
		#region Click OK
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Result = RESULT.OK;
			this.Close();
		}
		#endregion
		#region Click Help
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Result = RESULT.HELP;
			this.Close();
		}
		#endregion

		#region Commands
		private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
		//	e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
		}

		private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
		//	e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
		}

		private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
		{
			Result = RESULT.OK;
			this.Close();
		}

		private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
		{
			if (this.WindowState == System.Windows.WindowState.Normal)
			{
			//	SystemCommands.MaximizeWindow(this);
			}
			else if (this.WindowState == System.Windows.WindowState.Maximized)
			{
			//	SystemCommands.RestoreWindow(this);
			}
		}

		private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
		{
		//	SystemCommands.MinimizeWindow(this);
		}

		private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
		{
		//	SystemCommands.RestoreWindow(this);
		}
		#endregion
	}
}
