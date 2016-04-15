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
	/// <summary>
	/// Логика взаимодействия для BlockInput.xaml
	/// </summary>
	public partial class BlockOutput : UserControl
	{
		#region variables
		public delegate void ExpanderClickItems(EXPANDER_FUNCTION function, UIElement element);
		public event ExpanderClickItems expanderClickItem;
		#endregion
		#region BlockOutput
		public BlockOutput()
		{
			InitializeComponent();
		}
		#endregion
		#region Button click to menu
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
		#region Button mouse event
		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			UpDateComboBox();
		}
		#endregion
		#region UpDateComboBox
		public void UpDateComboBox()
		{
			String present_text = comboBox_variable.Text;
			comboBox_variable.Items.Clear();

			List<LMD_Compiling.INFO_VARIABLE> list = LMD_Compiling.ManagerVariables.GetAllVariables();
			for (int i = 0; i < LMD_Compiling.ManagerVariables.Count; i++)
				comboBox_variable.Items.Add(list[i].name);

			comboBox_variable.Text = present_text;
		}
		#endregion
		#region GetText
		public String GetText()
		{
			return txtTextView.Text;
		}
		#endregion
		#region GetVariable
		public String GetVariable()
		{
			return comboBox_variable.Text;
		}
		#endregion
	}
}
