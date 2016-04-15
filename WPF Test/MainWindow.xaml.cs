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
		private LMD_Compiling.CompilingManager mCompiling;
		private Boolean b_mouseDown;
		private Point pos_mouseDown;
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
			workspace.RemoveLine += workspace_RemoveLine;
			workspace.onMouseBlock += Workspace_onMouseBlock;
			workspace.MouseDown += workspace_MouseDown;
			workspace.MouseUp += workspace_MouseUp;
			workspace.MouseMove += Workspace_MouseMove;
			topPanel.run += topPanel_run;

			mCompiling = new LMD_Compiling.CompilingManager();

			this.Visibility = Visibility.Hidden;
			LMD_GUI.LMD_StartWindow sw = new LMD_GUI.LMD_StartWindow(this);
			sw.Show();
		}
		#endregion
		#region StartWindowEnable
		public void StartWindowEnable()
		{
			this.Visibility = System.Windows.Visibility.Hidden;
			LMD_GUI.LMD_StartWindow sw = new LMD_GUI.LMD_StartWindow(this);
			sw.Show();
		}
		#endregion
		#region InitializeProject
		public void InitializeProject(FileManager.INFO_PROJECT info)
		{
			FileManager.ManagerProjects.INFO_ALL_PROJECTS info_project = new FileManager.ManagerProjects.INFO_ALL_PROJECTS();
			info_project.Name = info.name;
			info_project.Path = info.path;
			FileManager.ManagerProjects.LoadProject(info_project);

			this.Title = info.name + " - Studio Flowcharts";
		}
		#endregion
		#region LoadingProjectToWorkspace
		public void LoadingProjectToWorkspace(FileManager.ManagerProjects.INFO_CONFIG config)
		{
			
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

		#region События верхнего бара
		/// <summary>
		/// Начало компиляции
		/// </summary>
		private void topPanel_run(String cmd)
		{
			switch (cmd)
			{
				#region СОЗДАТЬ
				case "СОЗДАТЬ":
				//	LMD_GUI.LMD_WindowCreateProject wcp = new LMD_GUI.LMD_WindowCreateProject();
				//	wcp.ShowDialog();
					break;
				#endregion
				#region ОТКРЫТЬ
				case "ОТКРЫТЬ": break;
				#endregion
				#region СОХРАНИТЬ
				case "СОХРАНИТЬ": //FileManager.ManagerProjects.SaveProject(mCompiling, workspace.blocks, info, fastInfo); 
					break;
				#endregion
				#region НАСТРОЙКИ
				case "НАСТРОЙКИ":
					//	LMD_GUI.LMD_WindowSetting ws = new LMD_GUI.LMD_WindowSetting();
					//	ws.Show();
					break;
				#endregion
				#region ПРОЕКТ
				case "ПРОЕКТ":
					LMD_GUI.LMD_WindowPropertiesProject wpp = new LMD_GUI.LMD_WindowPropertiesProject();
					wpp.Show();
					break;
				#endregion
				#region ЗАПУСТИТЬ
				case "ЗАПУСТИТЬ": mCompiling.Run(workspace.blocks, info, fastInfo); break;
				#endregion
				#region ГЛАВНАЯ
				case "ГЛАВНАЯ":
					LMD_GUI.LMD_StartWindow sw = new LMD_GUI.LMD_StartWindow(this);
					sw.Show();
					break;
				#endregion
				#region ПЕРЕМЕННЫЕ
				case "ПЕРЕМЕННЫЕ":
					LMD_GUI.LMD_WindowManagerVariables wmv = new LMD_GUI.LMD_WindowManagerVariables();
					wmv.Show();
					break;
				#endregion
				#region КОД
				case "КОД": 
					LMD_GUI.LMD_WindowCode wc = new LMD_GUI.LMD_WindowCode();
					wc.SetText(mCompiling.PrevTextCode);
					wc.Show(); break;
				#endregion
				#region СПРАВКА
				case "СПРАВКА":  break;
				#endregion
				#region ВЫХОД
				case "ВЫХОД":
					LMD_GUI.LMD_WindowClose wcc = new LMD_GUI.LMD_WindowClose();
					wcc.Height = 130;
					Canvas.SetLeft(wcc, this.Width / 2 - wcc.Width / 2);
					Canvas.SetTop(wcc, this.Height / 2 - wcc.Height / 2);
					wcc.ShowDialog();
				//Environment.Exit(0);
					break;
				#endregion
			}
		}
		#endregion
		#region Workspace Mouse
		#region Enter mouse fast panel
		/// <summary>
		/// Происходит когда мышь находится над блоком
		/// </summary>
		/// <param name="enter">Находится ли мышь над блоком. Если находится, то возвращает true</param>
		/// <param name="block">Сам блок</param>
		private void Workspace_onMouseBlock(bool enter, Object block)
		{
			if (enter)
			{
				fastInfo.SetBlock(block.GetType().ToString());
			}
			else
			{
				fastInfo.SetBlock("none");
			}
		}
		#endregion
		#region Workspace mouse down
		void workspace_MouseDown(object sender, MouseButtonEventArgs e)
		{
			b_mouseDown = true;
			pos_mouseDown = e.GetPosition(workspace);
		}
		#endregion
		#region Workspace mouse move
		/// <summary>
		/// Передвижение мыши по рабочей области
		/// </summary>
		/// <param name="sender">Объект UI по которой передвигается мышь</param>
		/// <param name="e">Объект event мыши</param>
		private void Workspace_MouseMove(object sender, MouseEventArgs e)
		{
			Point p = e.GetPosition((UIElement)sender);
			fastInfo.SetMouse(p.X - 10, p.Y);

			if (b_mouseDown)
			{
			//	workspace.MoveWorkspaceBlocks(p.X, p.Y);
			}
		}
		#endregion
		#region Workspace mouse up
		void workspace_MouseUp(object sender, MouseButtonEventArgs e)
		{
			b_mouseDown = false;
		}
		#endregion
		#endregion
		#region Add new joint
		/// <summary>
		/// Происходит при добавлении любой новой связи
		/// </summary>
		/// <param name="block_joint">Связь</param>
		private void workspace_createNewLine(LMD_BLOCKS.BlockJoint block_joint)
		{
			
		}
		#endregion
		#region Remove joint
		/// <summary>
		/// Происходит при удалении связи
		/// </summary>
		/// <param name="block_joint"></param>
		private void workspace_RemoveLine(LMD_BLOCKS.BlockJoint block_joint)
		{

		}
		#endregion
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
			LMD_GUI.LMD_WindowClose wcc = new LMD_GUI.LMD_WindowClose();
			wcc.Height = 130;
			Canvas.SetLeft(wcc, this.Width / 2 - wcc.Width / 2);
			Canvas.SetTop(wcc, this.Height / 2 - wcc.Height / 2);
			wcc.ShowDialog();
		//	SystemCommands.CloseWindow(this);
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
