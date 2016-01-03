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
	/// Логика взаимодействия для LMD_TextBox.xaml
	/// </summary>
	public partial class LMD_TextBox : UserControl
	{
		public String Text
		{
			get
			{
				return textBox.Text;
			}
			set
			{
				textBox.Text = value;
			}
		}
		public LMD_TextBox()
		{
			InitializeComponent();
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ClearText();
			textBox.Focus();
		}
		public void ClearText()
		{
			textBox.Text = "";
		}
	}
}
