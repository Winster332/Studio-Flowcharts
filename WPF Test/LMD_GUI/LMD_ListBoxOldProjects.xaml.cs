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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Test.LMD_GUI
{
	/// <summary>
	/// Логика взаимодействия для LMD_ListBoxOldProjects.xaml
	/// </summary>
	public partial class LMD_ListBoxOldProjects : UserControl
	{
		#region variables
		public delegate void EventClickItems(Button button, FileManager.ManagerProjects.INFO_ALL_PROJECTS info);
		public event EventClickItems clickItems;
		public enum TYPE_CMD { OPEN, CREATE, NONE }
		public delegate void EventClickButton(TYPE_CMD type_cmd);
		public event EventClickButton clickButton;
		#endregion
		#region LMD_ListBoxOldProjections
		public LMD_ListBoxOldProjects()
		{
			InitializeComponent();

			if (!System.IO.Directory.Exists("Projects"))
				System.IO.Directory.CreateDirectory("Projects");

			List<FileManager.ManagerProjects.INFO_ALL_PROJECTS> list = new List<FileManager.ManagerProjects.INFO_ALL_PROJECTS>();//FileManager.ManagerProjects.GetListProjects();

			String[] files = System.IO.Directory.GetDirectories("Projects\\");
			for (int i = 0; i < files.Length; i++)
			{
				AddItem(files[i].Substring(9, files[i].Length - 9), files[i]);
			}
		}
		#endregion
		#region Penel button click
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			String tag = ((Button)sender).Tag.ToString();
			TYPE_CMD cmd = TYPE_CMD.CREATE;

			switch (tag)
			{
				case "OPEN": cmd = TYPE_CMD.OPEN; break;
				case "CREATE": cmd = TYPE_CMD.CREATE; break;
				case "CLEAR": cmd = TYPE_CMD.NONE; FileManager.ManagerProjects.Clear(); stackPanel.Children.Clear(); break;
			}

			if (clickButton != null)
				clickButton(cmd);
		}
		#endregion
		#region AddItem
		public void AddItem(String name, String path)
		{
			TextBlock t_n = new TextBlock();
			t_n.Text = name;
			t_n.Margin = new Thickness(0, -5, 0, 0);
			TextBlock t_p = new TextBlock();
			t_p.Text = path;
			t_p.FontSize = 12;
			t_p.Foreground = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150));
			t_p.Margin = new Thickness(0, 0, 0, 0);

			StackPanel sp = new StackPanel();
			sp.Children.Add(t_n);
			sp.Children.Add(t_p);

			Grid grid = new Grid();
			grid.Children.Add(sp);

			FileManager.ManagerProjects.INFO_ALL_PROJECTS info = new FileManager.ManagerProjects.INFO_ALL_PROJECTS();
			info.Name = name;
			info.Path = path;

			Button but = new Button();
			but.Tag = path;
			but.Content = grid;
			but.Height = 40;
			but.Style = (Style)this.FindResource("MoveButtonStyle");
			but.Tag = info;
			but.Click += but_Click;

			stackPanel.Children.Add(but);
		}
		#endregion
		#region Buttons click
		public void but_Click(object sender, RoutedEventArgs e)
		{
			Button but = (Button)sender;
			FileManager.ManagerProjects.INFO_ALL_PROJECTS info = (FileManager.ManagerProjects.INFO_ALL_PROJECTS)but.Tag;

			if (clickItems != null)
				clickItems(but, info);
		}
		#endregion
	}
}
