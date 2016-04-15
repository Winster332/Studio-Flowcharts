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

namespace WPF_Test.LMD_BLOCKS
{
	public enum EXPANDER_FUNCTION { rename, delete, properties, joint, cut, copy, none }
	/// <summary>
	/// Логика взаимодействия для BlockBegin.xaml
	/// </summary>
	public partial class BlockBegin : UserControl
	{
		#region variables
		public delegate void ExpanderClickItems(EXPANDER_FUNCTION function, UIElement element);
		public event ExpanderClickItems expanderClickItem;
		#endregion 
		#region BlockBegin
		public BlockBegin()
		{
			InitializeComponent();
		}
		#endregion
		#region Button click
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			expander.IsExpanded = false;
			EXPANDER_FUNCTION func = EXPANDER_FUNCTION.none;
			String msg = ((Button)sender).Content.ToString();

			switch (msg)
			{
				case "Задать имя": func = EXPANDER_FUNCTION.rename; break;
				case "Связать": func = EXPANDER_FUNCTION.joint; break;
				case "Свойства": func = EXPANDER_FUNCTION.properties; break;
				case "Копировать": func = EXPANDER_FUNCTION.copy; break;
				case "Вырезать": func = EXPANDER_FUNCTION.cut; break;
				case "Удалить": func = EXPANDER_FUNCTION.delete; break;
				default: func = EXPANDER_FUNCTION.none; break;
			}

			if (expanderClickItem != null)
				expanderClickItem(func, this);
		}
		#endregion
	}
}
