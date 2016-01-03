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

namespace WPF_Test
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region variables
		private List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK> blocks;
		private LMD_Compiling.CompilingManager mCompiling;
		#endregion
		#region MainWindow
		public MainWindow()
		{
			InitializeComponent();

			#region add command
			this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnCloseWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, this.OnMaximizeWindow, this.OnCanResizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, this.OnMinimizeWindow, this.OnCanMinimizeWindow));
			this.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, this.OnRestoreWindow, this.OnCanResizeWindow));
			#endregion

			this.blocks = new List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK>();
			this.SizeChanged += MainWindow_SizeChanged;
			info.onHidden += (type) =>
				{
					if (type == LMD_GUI.LMD_BottomPanel.STATE_VISIBLY.hide)
						mainGrid.RowDefinitions[2].Height = new GridLength(20);
					else if (type == LMD_GUI.LMD_BottomPanel.STATE_VISIBLY.show)
						mainGrid.RowDefinitions[2].Height = new GridLength(220);
				};

			blockPanel.clickButtons += blockPanel_clickButtons;
			workspace.clickItemsExpander += workspace_clickItemsExpander;
			workspace.createNewLine += workspace_createNewLine;
			topPanel.run += topPanel_run;

			mCompiling = new LMD_Compiling.CompilingManager();
		}
		#endregion
		#region MainWindow_SizeChanged
		void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.WindowState == System.Windows.WindowState.Normal)
				info.grid.Width = this.Width;
			else info.grid.Width = SystemParameters.PrimaryScreenWidth;
		}
		#endregion

		private void topPanel_run()
		{
			mCompiling.Run(blocks, info, fastInfo);
		}

		private void workspace_createNewLine(LMD_BLOCKS.BlockJoint joint)
		{
			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK base_block = new LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK();

			MessageBox.Show("begin: "+joint.element_begin.GetType() + "\nend: "+ joint.element_end);

			blocks.Add(base_block);
		}

		#region click item blocks expander
		/// <summary>
		/// Происходит когда юзер выбирает функцию в блоке
		/// </summary>
		/// <param name="function">Тип функции</param>
		/// <param name="element">Выбранный блок</param>
		void workspace_clickItemsExpander(LMD_BLOCKS.EXPANDER_FUNCTION function, UIElement element)
		{
			switch (function)
			{
				case LMD_BLOCKS.EXPANDER_FUNCTION.joint:
					workspace.AddJoint(element); break;
				case LMD_BLOCKS.EXPANDER_FUNCTION.rename: break;
				case LMD_BLOCKS.EXPANDER_FUNCTION.properties: break;
				case LMD_BLOCKS.EXPANDER_FUNCTION.delete: workspace.RemoveBlock(element); break;
				case LMD_BLOCKS.EXPANDER_FUNCTION.cut: break;
				case LMD_BLOCKS.EXPANDER_FUNCTION.copy: break;
			}
		}
		#endregion
		#region Select block
		/// <summary>
		/// Происходит когда юзер выбирает блок
		/// </summary>
		/// <param name="type">Тип выранного блока</param>
		private void blockPanel_clickButtons(LMD_GUI.TYPE_BLOCK type)
		{
			workspace.AddBlock(type);
		}
		#endregion

		#region Commands
		private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.ResizeMode == ResizeMode.CanResize || this.ResizeMode == ResizeMode.CanResizeWithGrip;
		}

		private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.ResizeMode != ResizeMode.NoResize;
		}

		private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}

		private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
		{
			if (this.WindowState == System.Windows.WindowState.Normal)
			{
				SystemCommands.MaximizeWindow(this);
			}
			else if (this.WindowState == System.Windows.WindowState.Maximized)
			{
				SystemCommands.RestoreWindow(this);
			}
		}

		private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
		{
			SystemCommands.MinimizeWindow(this);
		}

		private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
		{
			SystemCommands.RestoreWindow(this);
		}
		#endregion
	}
}
