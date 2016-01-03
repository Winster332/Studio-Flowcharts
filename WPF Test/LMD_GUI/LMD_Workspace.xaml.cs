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
	/// Логика взаимодействия для LMD_Workspace.xaml
	/// </summary>
	public partial class LMD_Workspace : UserControl
	{
		#region variables
		public Boolean joint_select = false;
		public LMD_BLOCKS.BlockJoint selectJoint;
		private Boolean date_select = false;
		private UIElement SelectUIElement;
		private TYPE_BLOCK selectTypeBlock;

		public delegate void CreateNewBlock(Object block, TYPE_BLOCK type);
		public event CreateNewBlock createNewBlock;
		public delegate void CreateNewLine(LMD_BLOCKS.BlockJoint joint);
		public event CreateNewLine createNewLine;
		public delegate void ClickItemsExpander(LMD_BLOCKS.EXPANDER_FUNCTION function, UIElement element);
		public event ClickItemsExpander clickItemsExpander;
		#endregion
		#region LMD_Workspace
		public LMD_Workspace()
		{
			InitializeComponent();

			this.selectTypeBlock = TYPE_BLOCK.none;

			this.MouseMove += LMD_Workspace_MouseMove;
			this.MouseUp += LMD_Workspace_MouseUp;
		}
		#endregion
		#region Mouse
		private void LMD_Workspace_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (date_select)
				CreateSelectBlock(e.GetPosition(this).X, e.GetPosition(this).Y);
			if (joint_select)
			{
				if (selectJoint != null)
				{
					if (selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.condition)
					{
						selectJoint.CreateOldXY(this.canvas, e.GetPosition(this).X, e.GetPosition(this).Y);
						joint_select = false;
					}
					else if (selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.marshrut)
					{
						CreateSelectJoint(e.GetPosition(this).X, e.GetPosition(this).Y);
						//	selectJoint.CreateOldXY(this.canvas, e.GetPosition(this).X, e.GetPosition(this).Y);
						joint_select = false;
					}
					else 
					{
						selectJoint.CreateOldXY(this.canvas, e.GetPosition(this).X, e.GetPosition(this).Y);
						joint_select = false;
					}
				}
			}
		}

		private void LMD_Workspace_MouseMove(object sender, MouseEventArgs e)
		{
			if (date_select)
			{
				MoveSelectBlock(e.GetPosition(this).X, e.GetPosition(this).Y);
			}
		}
		#endregion
		#region AddBlock
		public void AddBlock(TYPE_BLOCK type)
		{
			canvas.Children.Remove(SelectUIElement);
			SelectUIElement = null;
			selectTypeBlock = TYPE_BLOCK.none;
			date_select = true;
			selectTypeBlock = type;

			switch (type)
			{
				case TYPE_BLOCK.begin: 
					LMD_BLOCKS.BlockBegin bb = new LMD_BLOCKS.BlockBegin(); 
					bb.expanderClickItem += bb_expanderClickItem; 
					SelectUIElement = bb; break;
				case TYPE_BLOCK.end: 
					LMD_BLOCKS.BlockEnd be = new LMD_BLOCKS.BlockEnd();
					be.expanderClickItem += bb_expanderClickItem;
					SelectUIElement = be; break;
				case TYPE_BLOCK.input: 
					LMD_BLOCKS.BlockInput bi = new LMD_BLOCKS.BlockInput();
					bi.expanderClickItem += bb_expanderClickItem;
					SelectUIElement = bi; break;
				case TYPE_BLOCK.output: 
					LMD_BLOCKS.BlockOutput bo = new LMD_BLOCKS.BlockOutput(); 
					bo.expanderClickItem += bb_expanderClickItem;
					SelectUIElement = bo; break;
				case TYPE_BLOCK.calculate: 
					LMD_BLOCKS.BlockCalculate bca = new LMD_BLOCKS.BlockCalculate();
					bca.expanderClickItem += bb_expanderClickItem;
					SelectUIElement = bca; break;
				case TYPE_BLOCK.condition: 
					LMD_BLOCKS.BlockCondition bco = new LMD_BLOCKS.BlockCondition();
					bco.expanderClickItem += bb_expanderClickItem;
					bco.clickJoint += bco_clickJoint;
					SelectUIElement = bco; break;
				case TYPE_BLOCK.camera:  break;
				case TYPE_BLOCK.joint: joint_select = true; break;
			}

			SelectUIElement.Opacity = 0.5;
			canvas.Children.Add(SelectUIElement);
			
			Canvas.SetLeft(SelectUIElement, -500);
			Canvas.SetTop(SelectUIElement, -500);
		}		
		#endregion
		#region MoveSelectBlock
		public void MoveSelectBlock(double x, double y)
		{
			if (SelectUIElement != null)
			{
				Canvas.SetLeft(SelectUIElement, x - 200);
				Canvas.SetTop(SelectUIElement, y - 100);
			}
		}
		#endregion
		#region CreateSelectBlock
		public void CreateSelectBlock(double x, double y)
		{
			date_select = false;

			if (SelectUIElement != null)
			{
				SelectUIElement.Opacity = 1;

				Canvas.SetLeft(SelectUIElement, x - 200);
				Canvas.SetTop(SelectUIElement, y - 100);

				if (createNewBlock != null)
					createNewBlock(SelectUIElement, selectTypeBlock);

				selectTypeBlock = TYPE_BLOCK.none;
				SelectUIElement = null;
			}
		}
		#endregion

		#region Expander click item
		private void bb_expanderClickItem(LMD_BLOCKS.EXPANDER_FUNCTION function, UIElement element)
		{
			if (clickItemsExpander != null)
				clickItemsExpander(function, element);
		}
		/// <summary>
		/// Происходит когда в блоке condition выбирается соединение
		/// </summary>
		/// <param name="element">Сам блок</param>
		/// <param name="num">Номер связи</param>
		private void bco_clickJoint(UIElement element, int num)
		{
			if (num == 0) // минус
			{
				joint_select = true;


				if (selectJoint == null)
				{
					selectJoint = new LMD_BLOCKS.BlockJoint(this);

					selectJoint.type_line = LMD_BLOCKS.TYPE_LINE.condition;
					selectJoint.SelectXY(Canvas.GetLeft(element) + 10, Canvas.GetTop(element) + 50);

					selectJoint.createNewJoint += selectJoint_createNewJoint;
				}
				else if (selectJoint != null)
				{
				}
			}
			else if (num == 1) // плюс
			{
				joint_select = true;

				if (selectJoint == null)
				{
					selectJoint = new LMD_BLOCKS.BlockJoint(this);

					selectJoint.type_line = LMD_BLOCKS.TYPE_LINE.condition;
					selectJoint.SelectXY(Canvas.GetLeft(element) + 340, Canvas.GetTop(element) + 50);

					selectJoint.createNewJoint += selectJoint_createNewJoint;
				}
				else if (selectJoint != null)
				{
				}
			}
		}
		#endregion

		public void selectJoint_createNewJoint(LMD_BLOCKS.BlockJoint joint)
		{
			createNewLine(joint);
		}

		#region AddJoint
		public void AddJoint(UIElement element)
		{
			joint_select = true;

			if (selectJoint == null)
			{
				selectJoint = new LMD_BLOCKS.BlockJoint(this);
				selectJoint.createNewJoint += selectJoint_createNewJoint;
				selectJoint.type_line = LMD_BLOCKS.TYPE_LINE.marshrut;
			}
			if (selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.marshrut)
			{
				if (selectJoint.element_begin == null)
					selectJoint.element_begin = element;
				else if (selectJoint.element_end == null)
				{
					selectJoint.element_end = element;
					CreateJoint2();
				}
			}
			else if (selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.line_block || selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.condition)
			{
				selectJoint.CreateOldXYToBlock(this.canvas, element);
				joint_select = false;
			}
		}
		#endregion
		#region CreateJoint2
		public void CreateJoint2()
		{
			if (selectJoint != null)
			{
				selectJoint.Create(this.canvas);

				selectJoint = null;		
			}
			joint_select = false;
		}
		#endregion
		#region CreateSelectJoint
		public void CreateSelectJoint(double x, double y)
		{
			if (selectJoint != null)
			{
				selectJoint.Create(this.canvas, x, y);

				selectJoint = null;
				joint_select = false;
			}
			joint_select = false;
		}
		#endregion
		#region RemoveBlock
		public void RemoveBlock(UIElement element)
		{
			canvas.Children.Remove(element);
		}
		#endregion
	}
}
