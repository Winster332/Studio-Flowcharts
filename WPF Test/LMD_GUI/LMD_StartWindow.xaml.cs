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
	/// Логика взаимодействия для LMD_StartWindow.xaml
	/// </summary>
	public partial class LMD_StartWindow : Window
	{
		#region variables
		private Window window_parent;
		#endregion
		#region LMD_StartWindow
		public LMD_StartWindow(Window window)
		{
			InitializeComponent();
			this.window_parent = window;
			LeftPanel.clickButton += LeftPanel_clickButton;
			LeftPanel.clickItems += LeftPanel_clickItems;

			#region add command
			this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));
			#endregion
		}
		#endregion
		#region Click button from left panel
		private void LeftPanel_clickButton(LMD_ListBoxOldProjects.TYPE_CMD type_cmd)
		{
			switch (type_cmd)
			{
				case LMD_ListBoxOldProjects.TYPE_CMD.CREATE: 
					LMD_GUI.LMD_WindowCreateProject wcp = new LMD_WindowCreateProject();
					wcp.Left = this.Width / 2 - wcp.Width / 2;
					wcp.Top = this.Height / 2 - wcp.Height / 2;

					wcp.onNewCreateProject += (info) =>
					{
						wcp.Close();
						this.Close();

						MainWindow mw = new MainWindow();
						mw.InitializeProject(info);
						mw.Visibility = System.Windows.Visibility.Visible;
						mw.Show();

						window_parent.Close();			
					};

					wcp.ShowDialog();
					
					break;
				case LMD_ListBoxOldProjects.TYPE_CMD.OPEN:
					Microsoft.Win32.OpenFileDialog o = new Microsoft.Win32.OpenFileDialog();
					o.InitialDirectory = @"Projects\";
					bool? b = o.ShowDialog();

					if (b == true)
					{
						MessageBox.Show("" + o.FileName);
					//	wcp.Close();
					//	this.Close();


						MainWindow mw = new MainWindow();
					//	FileManager.ManagerProjects.INFO_CONFIG conf = FileManager.ManagerProjects.LoadProject();
					//	mw.LoadingProjectToWorkspace();
					//	mw.InitializeProject(info);
					//	mw.Visibility = System.Windows.Visibility.Visible;
					//	mw.Show();	
					}
					else if (b == false)
					{
					//	MessageBox.Show("false");
					}
					break;
			}
		}
		#endregion
		#region Click button form left panel items
		private void LeftPanel_clickItems(Button button, FileManager.ManagerProjects.INFO_ALL_PROJECTS info)
		{
			Boolean result = FileManager.ManagerProjects.LoadProject(info);

			if (result)
			{

			}
			else if (!result)
			{
				String error = FileManager.ManagerProjects.GetError();
				LMD_GUI.LMD_WindowError we = new LMD_WindowError(error);
				we.ShowDialog();
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
			window_parent.Close();
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
