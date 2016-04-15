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
	/// Логика взаимодействия для LMD_MinimizeViewPanel.xaml
	/// </summary>
	public partial class LMD_MinimizeViewPanel : UserControl
	{
		#region variables
		public enum TYPE { FX, VAR }
		public TYPE Type;
		public String Text
		{
			get
			{
				return butF.Content.ToString();
			}
			set
			{
				butF.Content = value;
			}
		}
		public String sub_text;
		public delegate void DeleteButton(LMD_MinimizeViewPanel mvp);
		public event DeleteButton deleteButton;
		#endregion
		#region LMD_MinimizeViewPanel
		public LMD_MinimizeViewPanel()
		{
			InitializeComponent();
		}
		#endregion
		#region Button click delete
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (deleteButton != null)
				deleteButton(this);
		}
		#endregion
	}
}
