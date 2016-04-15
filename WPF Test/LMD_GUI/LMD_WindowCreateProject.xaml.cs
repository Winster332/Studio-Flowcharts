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
	/// Логика взаимодействия для LMD_WindowCreateProject.xaml
	/// </summary>
	public partial class LMD_WindowCreateProject : Window
	{
		#region variables
		public delegate void CreateNewProject(FileManager.INFO_PROJECT info);
		public CreateNewProject onNewCreateProject;
		#endregion
		#region LMD_WindowCreateProject
		public LMD_WindowCreateProject()
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
		#region Loaded
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			cb_language.Items.Add("C#");
		//	cb_language.Items.Add("Java");
			cb_language.Items.Add("C++");
			cb_language.Items.Add("Pascal");
		//	cb_language.Items.Add("Java Script");
		//	cb_language.Items.Add("PHP");
		//	cb_language.Items.Add("Python");
		//	cb_language.Items.Add("Basic");
		//	cb_language.Items.Add("Псевдокод");

			cb_type.Items.Add("exe");
			cb_type.Items.Add("dll");
			cb_type.Items.Add("txt");
			cb_type.Items.Add("none");

			cb_language.Text = "C#";
			cb_type.Text = "exe";
		}
		#endregion
		#region Button click
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			String tag = ((Button)sender).Tag.ToString();

			switch (tag)
			{
				case "CREATE": CreateProject(); break;
				case "CANCLE": this.Close(); break;
			}
		}
		#endregion
		#region CreateProject
		private void CreateProject()
		{
			FileManager.INFO_PROJECT info = new FileManager.INFO_PROJECT();
			info.count_block = 0;
			info.Language = GetPresentLanguage();
			info.name = textBox_name.Text;
			info.Type = GetPresentType();
			info.path = textBox_path.Text;
			info.path_icon = textBox_icon.Text;
			
			Boolean result = FileManager.ManagerProjects.CreateProject(info);

			if (result)
			{
				if (onNewCreateProject != null)
					onNewCreateProject(info);			
			}
			else
			{
				String error = FileManager.ManagerProjects.GetError();
				LMD_GUI.LMD_WindowError we = new LMD_WindowError(error);
				we.ShowDialog();

				if (we.Result == LMD_WindowError.RESULT.HELP)
				{
					info.name = FileManager.ManagerProjects.CorrectName(info.name, "_");
					textBox_name.Text = info.name;
				}
			}
		}
		#endregion
		#region GetPresentLanguage
		public LMD_Compiling.Language.ManagerLanguage.LANGUAGE GetPresentLanguage()
		{
			LMD_Compiling.Language.ManagerLanguage.LANGUAGE l = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.C_SHARP;
			String str_language = cb_language.Text;

			switch (str_language)
			{
				case "C#": l = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.C_SHARP; break;
				case "Java": l = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.JAVA; break;
				case "Pascal": l = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.PASCALABC; break;
				case "C++": l = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.C_PLUS_PLUS; break;
			}

			return l;
		}
		#endregion
		#region GetPresentType
		public FileManager.TYPE_PROJECT GetPresentType()
		{
			String str_type = cb_type.Text;
			FileManager.TYPE_PROJECT type = FileManager.TYPE_PROJECT.EXE;

			switch (str_type)
			{
				case "dll": type = FileManager.TYPE_PROJECT.DLL; break;
				case "exe": type = FileManager.TYPE_PROJECT.EXE; break;
				case "none": type = FileManager.TYPE_PROJECT.NONE; break;
				case "txt": type = FileManager.TYPE_PROJECT.TXT; break;
			}

			return type;
		}
		#endregion

		#region Button click generic name
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			textBox_name.Text = FileManager.ManagerProjects.GenericNameProject();
		}
		#endregion
		#region Button click find path project
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
			sfd.ShowDialog();
		}
		#endregion
		#region Button click standart language
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			cb_language.Text = "C#";
		}
		#endregion
		#region Button click standart type
		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			cb_type.Text = "exe";
		}
		#endregion
		#region Button click find path icon
		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
			sfd.ShowDialog();
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
