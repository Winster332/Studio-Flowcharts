﻿using System;
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
	/// Логика взаимодействия для LMD_TopPanel.xaml
	/// </summary>
	public partial class LMD_TopPanel : UserControl
	{
		public delegate void Run();
		public event Run run;
		public LMD_TopPanel()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			run();
		}
	}
}
