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
	/// Логика взаимодействия для LMD_BottomPanel.xaml
	/// </summary>
	public partial class LMD_BottomPanel : UserControl
	{
		#region variables
		public enum STATE_VISIBLY { show, hide }
		public STATE_VISIBLY state_visibly;
		public delegate void OnHidden(STATE_VISIBLY state);
		public event OnHidden onHidden;
		#endregion
		#region LMD_BottomPanel
		public LMD_BottomPanel()
		{
			InitializeComponent();
			grid.Width = SystemParameters.PrimaryScreenWidth;

			state_visibly = STATE_VISIBLY.show;

			AddItemText("-----------------------------------Hello World-----------------------------------");
		}
		#endregion
		#region AddItemText
		public void AddItemText(String text)
		{
			Button but = new Button();
			but.Width = grid.Width;
			but.Style = (Style)this.FindResource("MyListBoxItemStyle");
			but.Height = 20;
			but.Content = text;
			stackPanel.Children.Add(but);
		}
		#endregion
		#region Button click hiden
		// Hidden
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (state_visibly == STATE_VISIBLY.show)
			{
				grid.Height = 20;
				textBlockButtonHide.Text = "5";
				state_visibly = STATE_VISIBLY.hide;
			}
			else if (state_visibly == STATE_VISIBLY.hide)
			{
				grid.Height = 220;
				textBlockButtonHide.Text = "6";
				state_visibly = STATE_VISIBLY.show;
			}

			if (onHidden != null)
				onHidden(state_visibly);
		}
		#endregion
		#region button click clear
		// clear
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			ClearDate();
		}
		#endregion
		#region ClearDate
		/// <summary>
		/// Очищает все данные
		/// </summary>
		public void ClearDate()
		{
			stackPanel.Children.Clear();
			AddItemText("-----------------------------------Hello World-----------------------------------");
		}
		#endregion
		#region button click close
		// close
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			grid.Visibility = System.Windows.Visibility.Hidden;
		}
		#endregion
	}
}
