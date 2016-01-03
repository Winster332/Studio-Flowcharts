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
	public enum TYPE_BLOCK { begin, end, cycle, input, output, calculate, condition, subroutine, joint, camera, none };
	/// <summary>
	/// Логика взаимодействия для LMD_ListBox.xaml
	/// </summary>
	public partial class LMD_ListBox : UserControl
	{		
		public delegate void ClickButtons(TYPE_BLOCK type);
		public event ClickButtons clickButtons;
		public LMD_ListBox()
		{
			InitializeComponent();

			
		}

		public void AddItemText(String text)
		{
			Button but = new Button();
			but.Height = 40;
			but.Width = this.Width;
			but.Style = (Style)this.FindResource("MoveButtonStyle");

			stackPanel.Children.Add(but);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			TYPE_BLOCK type = TYPE_BLOCK.none;
			String tag = ((Button)sender).Tag.ToString();

			switch (tag)
			{
				case "begin":		type = TYPE_BLOCK.begin;		break;
				case "end":			type = TYPE_BLOCK.end;			break;
				case "calculate":	type = TYPE_BLOCK.calculate;	break;
				case "cycle":		type = TYPE_BLOCK.cycle;		break;
				case "input":		type = TYPE_BLOCK.input;		break;
				case "output":		type = TYPE_BLOCK.output;		break;
				case "joint":		type = TYPE_BLOCK.joint;		break;
				case "condition":	type = TYPE_BLOCK.condition;	break;
				case "subroutine":	type = TYPE_BLOCK.subroutine;	break;
				case "camera": type = TYPE_BLOCK.camera; break;
			}

			if (clickButtons != null)
				clickButtons(type);
		}
	}
}
