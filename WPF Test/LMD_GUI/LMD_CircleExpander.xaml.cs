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
	/// Логика взаимодействия для LMD_CircleExpander.xaml
	/// </summary>
	public partial class LMD_CircleExpander : UserControl
	{
		public delegate void ClickExpanderItems(LMD_BLOCKS.EXPANDER_FUNCTION function, UIElement element);
		public event ClickExpanderItems expanderClickItem;
		public LMD_CircleExpander()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			expander.IsExpanded = false;
			LMD_BLOCKS.EXPANDER_FUNCTION func = LMD_BLOCKS.EXPANDER_FUNCTION.none;
			String msg = ((Button)sender).Content.ToString();

			switch (msg)
			{
				case "Связать": func = LMD_BLOCKS.EXPANDER_FUNCTION.joint; break;
				case "Свойства": func = LMD_BLOCKS.EXPANDER_FUNCTION.properties; break;
				case "Удалить": func = LMD_BLOCKS.EXPANDER_FUNCTION.delete; break;
				default: func = LMD_BLOCKS.EXPANDER_FUNCTION.none; break;
			}

			if (expanderClickItem != null)
				expanderClickItem(func, this);
		}
	}
}
