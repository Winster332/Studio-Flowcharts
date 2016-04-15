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
	/// Логика взаимодействия для LMD_FastInformationPanel.xaml
	/// </summary>
	public partial class LMD_FastInformationPanel : UserControl
	{
		#region variables
		public ProgressBar ProgressBar
		{
			get { return pb; }
			set { pb = value; }
		}
		#endregion
		#region LMD_FastInformationPanel
		public LMD_FastInformationPanel()
		{
			InitializeComponent();
		}
		#endregion
		#region SetInfo
		public void SetInfo(String text)
		{
			tb_info.Text = text;
		}
		#endregion
		#region SetMouse
		public void SetMouse(double x, double y)
		{
			tb_mouse.Text = "Mouse { " + x + "; " + y + " }";
		}
		#endregion
		#region SetInfo
		public void SetBlock(String text)
		{
			tb_block.Text = "Block: " + text;
		}
		#endregion
		#region SetCurrentText
		public void SetCurrentText(String text)
		{

		}
		#endregion
	}
}
