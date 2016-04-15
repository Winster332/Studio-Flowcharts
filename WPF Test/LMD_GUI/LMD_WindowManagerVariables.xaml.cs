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
	/// Логика взаимодействия для LMD_WindowManagerVariables.xaml
	/// </summary>
	public partial class LMD_WindowManagerVariables : Window
	{
		#region variables

		#endregion
		#region LMD_WindowManagerVariables
		public LMD_WindowManagerVariables()
		{
			InitializeComponent();

			#region add command
			this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));
			#endregion

			UpDateInterface();
		}
		#endregion
		#region Click buttons UI
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			String tag = ((Button)sender).Tag.ToString();

			switch (tag)
			{
				case "CREATE": CreateVariable(); break;
				case "DELETE": DeleteVariable(); break;
				case "CLOSE": this.Close(); break;
			}
		}
		#endregion
		#region DeleteVariable
		private void DeleteVariable()
		{
			if (listBoxAllVar.SelectedItem != null)
			{
				String name = listBoxAllVar.SelectedItem.ToString();
				LMD_Compiling.ManagerVariables.Remove(name);

				UpDateInterface();
			}
		}
		#endregion
		#region CreateVariable
		private void CreateVariable()
		{
			LMD_GUI.LMD_WindowCreateVariables wcv = new LMD_WindowCreateVariables(null);
			wcv.Show();

			wcv.Closed += (o, e) =>
				{
					UpDateInterface();
				};
		}
		#endregion
		#region UpDateInterface
		public void UpDateInterface()
		{
			listBoxAllVar.Items.Clear();

			List<LMD_Compiling.INFO_VARIABLE> list = LMD_Compiling.ManagerVariables.GetAllVariables();

			for (int i = 0; i < list.Count; i++)
			{
				listBoxAllVar.Items.Add(list[i].name);
			}
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
