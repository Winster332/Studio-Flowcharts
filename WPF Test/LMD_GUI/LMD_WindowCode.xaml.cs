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
	/// Логика взаимодействия для LMD_WindowCode.xaml
	/// </summary>
	public partial class LMD_WindowCode : Window
	{
		#region variables
		private static Color COLOR_BLUE = Color.FromRgb(0, 140, 220);
		private static Color COLOR_GREEN = Color.FromArgb(150, 0, 220, 140);
		private static Color COLOR_NUM = Color.FromRgb(220, 140, 120);
		private static Color COLOR_TEXT = Color.FromRgb(192, 192, 192);
		private String[] CODE_BLUE = new String[] { "public", "private", "class", "partial", "static", "int", 
			"string", "double", "float", "bool", "void", "using" };
		private String[] CODE_GREEN = new String[] { "String", "Double", "Boolean", "[STAThread]", "Program", "Console"};
		private String[] CODE_NUM = new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
		#endregion
		#region LMD_WindowCode
		public LMD_WindowCode()
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
		#region SetText
		public void SetText(List<String> val)
		{
			txt.Text = "";
			String text = "";

			foreach(String str in val)
			{
				text += str + "\n";
			}

			ParseText(text);
		}
		#endregion
		#region ParseText
		public void ParseText(String code)
		{
			List<Run> runs = new List<Run>();

			String word = "";
			for (int i = 0; i < code.Length; i++)
			{
				String prev_word = word;
				prev_word = prev_word.Replace(" ", "").Replace("\n", "").Replace("	", "");
			
				if (code[i].ToString() == " " || code[i].ToString() == ";" || code[i].ToString() == "\n" || code[i].ToString() == ".")
				{
					Boolean cont = false;

					#region CODE_BLUE
					for (int j = 0; j < CODE_BLUE.Length; j++)
					{
						if (prev_word == CODE_BLUE[j])
						{
							Run r = new Run();
							r.Foreground = new SolidColorBrush(COLOR_BLUE);
							r.Text = word + code[i];
							runs.Add(r);
							
							word = "";
							cont = true;

							continue;
						}
					}
					#endregion
					#region CODE_GREEN
					if (cont)
						continue;

					for (int j = 0; j < CODE_GREEN.Length; j++)
					{
						if (prev_word == CODE_GREEN[j])
						{
							Run r = new Run();
							r.Foreground = new SolidColorBrush(COLOR_GREEN);
							r.Text = word + code[i];
							runs.Add(r);

							word = "";
							cont = true;

							continue;
						}
					}
					#endregion
					#region CODE_NUM
					if (cont)
						continue;


					#endregion
					#region CODE_TEXT
					if (cont)
						continue;

					// Второстепенные слова
					Run re = new Run();
					re.Foreground = new SolidColorBrush(COLOR_TEXT);
					re.Text = word;
					runs.Add(re);

					word = "";
					#endregion
				}

				word += code[i];
			}

			for (int i = 0; i < runs.Count; i++)
			{
				paragraph.Inlines.Add(runs[i]);
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
