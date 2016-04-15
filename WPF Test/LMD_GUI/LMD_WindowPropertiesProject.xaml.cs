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
	/// Логика взаимодействия для LMD_WindowPropertiesProject.xaml
	/// </summary>
	public partial class LMD_WindowPropertiesProject : Window
	{
		#region variables
		private FileManager.INFO_PROJECT info;
		#endregion
		#region LMD_WindowPropertiesProject
		public LMD_WindowPropertiesProject()
		{
			InitializeComponent();

			#region add command
			this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));
			#endregion

			this.Title = FileManager.ManagerProjects.GetPresentProjectInfo().name + " - параметры проекта";

			cb_language.Items.Add("C#");
			cb_language.Items.Add("Java");
			cb_language.Items.Add("C++");
			cb_language.Items.Add("Pascal");
			cb_language.Items.Add("Java Script");
			cb_language.Items.Add("PHP");
			cb_language.Items.Add("Python");
			cb_language.Items.Add("Basic");
			cb_language.Items.Add("Псевдокод");

			cb_type.Items.Add("exe");
			cb_type.Items.Add("dll");
			cb_type.Items.Add("txt");
			cb_type.Items.Add("none");

			
		}
		#endregion
		#region Loaded
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			FileManager.INFO_PROJECT pip = FileManager.ManagerProjects.GetPresentProjectInfo();
			w_txt.Text = pip.workspace_width.ToString();
			h_txt.Text = pip.workspace_height.ToString();
			txt_name.Text = pip.name;
			txt_path.Text = pip.path;
			txt_path_ico.Text = pip.path_icon;
			cb_language.Text = "" + pip.Language;
			cb_type.Text = "" + pip.Type;
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

		#region size workspace all
		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}
		#endregion
		#region generic name
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{

		}
		#endregion
		#region path project
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{

		}
		#endregion
		#region standart path project
		private void Button_Click_3(object sender, RoutedEventArgs e)
		{

		}
		#endregion
		#region language project standart
		private void Button_Click_4(object sender, RoutedEventArgs e)
		{

		}
		#endregion
		#region type project standart
		private void Button_Click_5(object sender, RoutedEventArgs e)
		{

		}
		#endregion
		#region save project
		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			info = new FileManager.INFO_PROJECT();
			info.name = txt_name.Text;
			info.path = txt_path.Text;
			info.path_icon = txt_path_ico.Text;

			try
			{
				info.workspace_width = float.Parse(w_txt.Text);
				info.workspace_height = float.Parse(h_txt.Text);
			}
			catch { MessageBox.Show("Не корректно введены параметры размера поля"); }

			info.Language = GetLanguage();
			info.Type = GetTypeProject();

			FileManager.ManagerProjects.ApplySettings(info);

			this.Close();
		}
		#endregion
		#region cancle
		private void Button_Click_7(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		#endregion
		#region GetLanguage
		public LMD_Compiling.Language.ManagerLanguage.LANGUAGE GetLanguage()
		{
			LMD_Compiling.Language.ManagerLanguage.LANGUAGE res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.C_SHARP;

			switch (cb_language.Text)
			{
				case "C#": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.C_SHARP; break;
				case "Java": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.JAVA; break;
				case "C++": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.C_PLUS_PLUS; break;
				case "Pascal": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.PASCALABC; break;
				case "Java Script": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.JS; break;
				case "PHP": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.PHP; break;
				case "Python": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.Python; break;
				case "Basic": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.Basic; break;
				case "Псевдокод": res = LMD_Compiling.Language.ManagerLanguage.LANGUAGE.TextCode; break;
			}

			return res;
		}
		#endregion
		#region GetTypeProject
		public FileManager.TYPE_PROJECT GetTypeProject()
		{
			FileManager.TYPE_PROJECT type = FileManager.TYPE_PROJECT.NONE;

			switch (cb_type.Text)
			{
				case "exe": type = FileManager.TYPE_PROJECT.EXE; break;
				case "dll": type = FileManager.TYPE_PROJECT.DLL; break;
				case "txt": type = FileManager.TYPE_PROJECT.TXT; break;
				case "none": type = FileManager.TYPE_PROJECT.NONE; break;
			}

			return type;
		}
		#endregion
	}
}
