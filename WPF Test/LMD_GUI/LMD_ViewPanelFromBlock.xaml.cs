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
	/// Логика взаимодействия для LMD_ViewPanelFromBlock.xaml
	/// </summary>
	public partial class LMD_ViewPanelFromBlock : UserControl
	{
		#region LMD_ViewPanelFromBlock
		public LMD_ViewPanelFromBlock()
		{
			InitializeComponent();
		}
		#endregion
		#region Add
		public void Add(String text, LMD_MinimizeViewPanel.TYPE type, String sub_text)
		{
			LMD_MinimizeViewPanel mvp = new LMD_MinimizeViewPanel();
			mvp.Text = text;
			mvp.Margin = new Thickness(2, 2, 0, 2);
			mvp.deleteButton += mvp_deleteButton;
			mvp.Type = type;
			mvp.sub_text = sub_text;

			content.Children.Add(mvp);
		}
		#endregion
		#region Delete but
		void mvp_deleteButton(LMD_MinimizeViewPanel mvp)
		{
			if (mvp.Type == LMD_MinimizeViewPanel.TYPE.VAR)
				LMD_Compiling.ManagerVariables.Remove(mvp.sub_text);
			content.Children.Remove(mvp);
		}
		#endregion
	}
}
