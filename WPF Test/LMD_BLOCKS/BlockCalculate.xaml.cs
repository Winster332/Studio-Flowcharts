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
	/// Логика взаимодействия для BlockCalculate.xaml
	/// </summary>
	public partial class BlockCalculate : UserControl
	{
		#region variables
		public delegate void ExpanderClickItems(EXPANDER_FUNCTION function, UIElement element);
		public event ExpanderClickItems expanderClickItem;
		public List<LMD_Compiling.INFO_VARIABLE> variables;
		#endregion
		#region BlockCalculate
		public BlockCalculate()
		{
			InitializeComponent();

			this.variables = new List<LMD_Compiling.INFO_VARIABLE>();
		}
		#endregion
		#region Button click menu
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
		#region Button click create variable
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			LMD_GUI.LMD_WindowCreateVariables wcv = new LMD_GUI.LMD_WindowCreateVariables(this);
			wcv.ShowDialog();

			if (wcv.result_var != null)
			{
				variables.Add(wcv.result_var);

				txtView.Text += wcv.result_var.name;
			//	contentFX.Add("var " + wcv.result_var.name, LMD_GUI.LMD_MinimizeViewPanel.TYPE.VAR, wcv.result_var.name);
			}
		}
		#endregion
		#region Button ckick add f(x)
		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			LMD_GUI.LMD_WindowClaculate wc = new LMD_GUI.LMD_WindowClaculate();
			wc.ShowDialog();
			this.txtView.Text = wc.Text;

		}
		#endregion
		#region GetText
		public String GetText()
		{
			return txtView.Text;
		}
		#endregion
	}
}
