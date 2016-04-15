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
		public List<UIElement> element_buffer;
		public List<LMD_BLOCKS.BlockJoint> joints;
		public List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK> blocks;
		public Boolean joint_select = false;
		public LMD_BLOCKS.BlockJoint selectJoint;
		private List<Point> pos_double_buffer_blocks;
		private List<Point> pos_double_buffer_joints_expander;
		private List<Rect> pos_double_buffer_joints;
		private Boolean date_select = false;
		private UIElement SelectUIElement;
		private TYPE_BLOCK selectTypeBlock;
		private Boolean b_mouseDown;
		private Point pos_mouseDown;

		public delegate void CreateNewBlock(Object block, TYPE_BLOCK type);
		public event CreateNewBlock createNewBlock;
		public delegate void CreateNewLine(LMD_BLOCKS.BlockJoint joint);
		public event CreateNewLine createNewLine;
		public delegate void RemoveNewLine(LMD_BLOCKS.BlockJoint joint);
		public event RemoveNewLine RemoveLine;
		public delegate void ClickItemsExpander(LMD_BLOCKS.EXPANDER_FUNCTION function, UIElement element);
		public event ClickItemsExpander clickItemsExpander;
		public delegate void EventMouseMoveBlock(Boolean enter, Object block);
		public event EventMouseMoveBlock onMouseBlock;
		#endregion
		#region LMD_Workspace
		public LMD_Workspace()
		{
			InitializeComponent();

			this.selectTypeBlock = TYPE_BLOCK.none;

			this.MouseMove += LMD_Workspace_MouseMove;
			this.MouseUp += LMD_Workspace_MouseUp;
			this.MouseDown += LMD_Workspace_MouseDown;
			blocks = new List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK>();
			joints = new List<LMD_BLOCKS.BlockJoint>();
			element_buffer = new List<UIElement>();
		}
		#endregion
		#region Mouse
		#region Block mouse
		#region Mouse enter
		void element_MouseEnter(object sender, MouseEventArgs e)
		{
			if (onMouseBlock != null)
				onMouseBlock(true, sender);
		}
		#endregion
		#region Mouse leave
		void element_MouseLeave(object sender, MouseEventArgs e)
		{
			if (onMouseBlock != null)
				onMouseBlock(false, sender);
		}
		#endregion
		#endregion
		#region Workspace mouse down
		void LMD_Workspace_MouseDown(object sender, MouseButtonEventArgs e)
		{
			b_mouseDown = true;
			pos_mouseDown = e.GetPosition(this);
			pos_double_buffer_blocks = new List<Point>();
			pos_double_buffer_joints = new List<Rect>();
			pos_double_buffer_joints_expander = new List<Point>();

			for (int i = 0; i < blocks.Count; i++)
			{
				double x = Canvas.GetLeft(blocks[i].element);
				double y = Canvas.GetTop(blocks[i].element);

				pos_double_buffer_blocks.Add(new Point(x, y));
			}

			for (int i = 0; i < joints.Count; i++)
			{
				double x = Canvas.GetLeft(joints[i].GetExpanderCircle());
				double y = Canvas.GetTop(joints[i].GetExpanderCircle());

				Line l = (Line)joints[i].GetLine();
				pos_double_buffer_joints.Add(new Rect(l.X1, l.X2, l.Y1, l.Y2));
				pos_double_buffer_joints_expander.Add(new Point(x, y));
			}
		}
		#endregion
		#region Workspace mouse up
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
					else if (selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.marshrut) // блок-линия
					{						
						CreateSelectJoint(e.GetPosition(this).X, e.GetPosition(this).Y);
						
						//	selectJoint.CreateOldXY(this.canvas, e.GetPosition(this).X, e.GetPosition(this).Y);
						joint_select = false;
					}
					else // Пустота к пустоте
					{
					//	MessageBox.Show("" + selectJoint.element_begin.GetType());
						selectJoint.CreateOldXY(this.canvas, e.GetPosition(this).X, e.GetPosition(this).Y);
						joint_select = false;
					}
				}
			}
			b_mouseDown = false;
		}
		#endregion
		#region Workspace mouse move
		private void LMD_Workspace_MouseMove(object sender, MouseEventArgs e)
		{
			Point pos = new Point(e.GetPosition(this).X, e.GetPosition(this).Y);
			if (date_select)
			{
				MoveSelectBlock(pos.X, pos.Y);
			}
			else
			{
				if (b_mouseDown)
				{
					MoveAllBlocks(pos.X - pos_mouseDown.X, pos.Y - pos_mouseDown.Y);
				}
			}
		}
		#endregion
		#endregion
		#region AddBlock
		public void AddBlock(TYPE_BLOCK type)
		{
			canvas.Children.Remove(SelectUIElement);
			SelectUIElement = null;
			selectTypeBlock = TYPE_BLOCK.none;
			date_select = true;
			selectTypeBlock = type;
			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b_b = null;

			switch (type)
			{
				case TYPE_BLOCK.begin: 
					LMD_BLOCKS.BlockBegin bb = new LMD_BLOCKS.BlockBegin(); 
					bb.expanderClickItem += bb_expanderClickItem; 
					SelectUIElement = bb; 
					b_b = new LMD_BLOCKS.BLOCK_JOINT.BEGIN(); 
					break;
				case TYPE_BLOCK.end: 
					LMD_BLOCKS.BlockEnd be = new LMD_BLOCKS.BlockEnd();
					be.expanderClickItem += bb_expanderClickItem;
					SelectUIElement = be;
					b_b = new LMD_BLOCKS.BLOCK_JOINT.END();
					break;
				case TYPE_BLOCK.input: 
					LMD_BLOCKS.BlockInput bi = new LMD_BLOCKS.BlockInput();
					bi.expanderClickItem += bb_expanderClickItem;
					SelectUIElement = bi;
					b_b = new LMD_BLOCKS.BLOCK_JOINT.INPUT();
					break;
				case TYPE_BLOCK.output: 
					LMD_BLOCKS.BlockOutput bo = new LMD_BLOCKS.BlockOutput(); 
					bo.expanderClickItem += bb_expanderClickItem;
					SelectUIElement = bo;
					b_b = new LMD_BLOCKS.BLOCK_JOINT.OUTPUT();
					break;
				case TYPE_BLOCK.calculate: 
					LMD_BLOCKS.BlockCalculate bca = new LMD_BLOCKS.BlockCalculate();
					bca.expanderClickItem += bb_expanderClickItem;
					SelectUIElement = bca;
					b_b = new LMD_BLOCKS.BLOCK_JOINT.CALCULATE();
					break;
				case TYPE_BLOCK.condition: 
					LMD_BLOCKS.BlockCondition bco = new LMD_BLOCKS.BlockCondition();
					bco.expanderClickItem += bb_expanderClickItem;
					bco.clickJoint += bco_clickJoint;
					SelectUIElement = bco;
					b_b = new LMD_BLOCKS.BLOCK_JOINT.CONDITION();
					break;
				case TYPE_BLOCK.camera:  break;
				case TYPE_BLOCK.joint: joint_select = true; break;
			}

			b_b.element = SelectUIElement;
			b_b.element.MouseEnter += element_MouseEnter;
			b_b.element.MouseLeave += element_MouseLeave;
			blocks.Add(b_b);

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
			#region -
			if (num == 0) // минус
			{
				joint_select = true;
				
				if (selectJoint == null)
				{
					selectJoint = new LMD_BLOCKS.BlockJoint(this);

					selectJoint.type_line = LMD_BLOCKS.TYPE_LINE.condition;
					selectJoint.element_begin = element;
					selectJoint.State = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false;
					selectJoint.SelectXY(Canvas.GetLeft(element) + 10, Canvas.GetTop(element) + 50);

					selectJoint.createNewJoint += selectJoint_createNewJoint;
					selectJoint.removeJoint += selectJoint_removeJoint;
				}
				else if (selectJoint != null)
				{
				}
			}
			#endregion
			#region +
			else if (num == 1) // плюс
			{
				joint_select = true;

				if (selectJoint == null)
				{
					selectJoint = new LMD_BLOCKS.BlockJoint(this);

					selectJoint.type_line = LMD_BLOCKS.TYPE_LINE.condition;
					selectJoint.element_begin = element;
					selectJoint.State = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true;
					selectJoint.SelectXY(Canvas.GetLeft(element) + 340, Canvas.GetTop(element) + 50);

					selectJoint.createNewJoint += selectJoint_createNewJoint;
					selectJoint.removeJoint += selectJoint_removeJoint;
				}
				else if (selectJoint != null)
				{
				}
			}
			#endregion
		}
		#endregion

		#region create new joint
		/// <summary>
		/// Происходит при создании нового соединения
		/// </summary>
		/// <param name="joint"></param>
		public void selectJoint_createNewJoint(LMD_BLOCKS.BlockJoint joint)
		{
			createNewLine(joint);
			joints.Add(joint);
		//	MessageBox.Show(""+);
			if (joint.type_line == LMD_BLOCKS.TYPE_LINE.end_condition)
			{
				for (int i = 0; i < blocks.Count; i++)
				{
					if (blocks[i].element == joint.element_begin || blocks[i].element == joint.element_end)
						if (blocks[i].Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false)
							blocks[i].Mode = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false_end;
						else if (blocks[i].Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true)
							blocks[i].Mode = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true_end;
				}
			//	MessageBox.Show(joint.type_line + "\n" + joint.element_begin.GetType() + " | " + joint.element_end.GetType());
			}
		}
		#endregion
		#region remove joint
		/// <summary>
		/// Происходит при удалении соединения
		/// </summary>
		/// <param name="joint"></param>
		public void selectJoint_removeJoint(LMD_BLOCKS.BlockJoint joint)
		{
			Boolean block_yes = false;
			for (int i = 0; i < blocks.Count; i++)
			{
				if (blocks[i].next_1 != null)
					if (joint.element_begin == blocks[i].next_1.element)
					{
						if (blocks[i].next_1 != null)
							blocks[i].next_1.next_1 = null;

						block_yes = true;
					}

				if (block_yes)
					break;
			}
			
			joints.Remove(joint);
			RemoveLine(joint);
		}
		#endregion

		#region AddJoint
		/// <summary>
		/// Добавляет выборочно связь. Связь зависит от selectJoint 
		/// и исходя из этого объекта сама выбирает какую форму она будет иметь
		/// </summary>
		/// <param name="element">UI связываемого блока. Привязывается к соединению</param>
		public void AddJoint(UIElement element)
		{
			joint_select = true;

			if (selectJoint == null)
			{
				selectJoint = new LMD_BLOCKS.BlockJoint(this);
				selectJoint.element_begin = element;
				selectJoint.createNewJoint += selectJoint_createNewJoint;
				selectJoint.removeJoint += selectJoint_removeJoint;
				selectJoint.type_line = LMD_BLOCKS.TYPE_LINE.marshrut;
			}
			else if (selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.marshrut) // Прямая связь
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
				if (selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.line_block)
				{
					selectJoint.element_end = element;
					LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b1 = null;
					LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b2 = null;

					for (int i = 0; i < blocks.Count; i++)
						if (blocks[i].element == selectJoint.element_begin)
							b1 = blocks[i];
					for (int i = 0; i < blocks.Count; i++)
						if (blocks[i].element == selectJoint.element_end)
							b2 = blocks[i];
					b1.next_1 = b2;

				//	MessageBox.Show(""+b1.Type + " | " + b2.Type);

					selectJoint.CreateOldXYToBlock(this.canvas, element);
				}
				else if (selectJoint.type_line == LMD_BLOCKS.TYPE_LINE.condition)
				{
					#region +
					if (selectJoint.State == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true) // +
					{
						selectJoint.element_end = element;
						LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b1 = null;
						LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b2 = null;

						for (int i = 0; i < blocks.Count; i++)
							if (blocks[i].element == selectJoint.element_begin)
								b1 = blocks[i];
						for (int i = 0; i < blocks.Count; i++)
							if (blocks[i].element == selectJoint.element_end)
								b2 = blocks[i];
						b1.next_1 = b2;
						b2.Mode = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true;

						selectJoint.CreateOldXYToBlock(this.canvas, element);
					}
					#endregion
					#region -
					else if (selectJoint.State == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false) // -
					{
						selectJoint.element_end = element;
						LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b1 = null;
						LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b2 = null;

						for (int i = 0; i < blocks.Count; i++)
							if (blocks[i].element == selectJoint.element_begin)
								b1 = blocks[i];
						for (int i = 0; i < blocks.Count; i++)
							if (blocks[i].element == selectJoint.element_end)
								b2 = blocks[i];
						b1.next_2 = b2;
						b2.Mode = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false;

						selectJoint.CreateOldXYToBlock(this.canvas, element);
					}
					#endregion
				}
				else
				{
					selectJoint.CreateOldXYToBlock(this.canvas, element);
				}
				joint_select = false;
				selectJoint = null;
			}
		}
		#endregion
		#region CreateJoint2
		public void CreateJoint2()
		{
			if (selectJoint != null)
			{
				LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b1 = null;
				LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK b2 = null;

				for (int i = 0; i < blocks.Count; i++)
					if (blocks[i].element == selectJoint.element_begin)
						b1 = blocks[i];
				for (int i = 0; i < blocks.Count; i++)
					if (blocks[i].element == selectJoint.element_end)
						b2 = blocks[i];
				b1.next_1 = b2;

			//	MessageBox.Show("b1[" + b1.Type + "] Mode[" + b1.Mode + "] b2[" + b2.Type + "] Mode[" + b2.Mode + "]");

				if (b1.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true)
				{
					b2.Mode = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true;
				}
				else if (b1.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false)
				{
					b2.Mode = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false;
				}

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
				element_buffer.Add(selectJoint.element_begin);

				if (selectJoint.element_begin != null)
				{
					for (int i = 0; i < blocks.Count; i++)
					{
						if (blocks[i].element == selectJoint.element_begin)
						{
							if (blocks[i].Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true)
								blocks[i].Mode = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true_end;
							if (blocks[i].Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false)
								blocks[i].Mode = LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false_end;
						}
					}
				}

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
		//	LMD_BLOCKS.BlockJoint joint = null;
			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK bblock = null;

			for (int i = 0; i < blocks.Count; i++)
				if (blocks[i].element == element)
				{
					bblock = blocks[i];

		//			for (int j = 0; j < joints.Count; j++)
		//				if (joints[j].element_begin == blocks[i].element)
		//				{
		//					joint = joints[j];
		//				}

					break;
				}

		//	Boolean block_yes = false;
		//	for (int i = 0; i < blocks.Count; i++)
		//	{
		//		if (blocks[i].next_1 != null)
		//			if (joint.element_end == blocks[i].next_1.element)
		//			{
		//				blocks[i].next_1 = null;
//
//						block_yes = true;
//					}
//
//				if (block_yes)
//					break;
//			}

//			joints.Remove(joint);
//			RemoveLine(joint);

			blocks.Remove(bblock);
			canvas.Children.Remove(element);
		}
		#endregion
		#region MoveAllBlocks
		public void MoveAllBlocks(double x, double y)
		{
			// Move blocks
			for (int i = 0; i < blocks.Count; i++)
			{
				Canvas.SetLeft(blocks[i].element, pos_double_buffer_blocks[i].X + x);
				Canvas.SetTop(blocks[i].element, pos_double_buffer_blocks[i].Y + y);
			}

			// Move joints
			for (int i = 0; i < joints.Count; i++)
			{
				((Line)joints[i].GetLine()).X1 = pos_double_buffer_joints[i].x + x;
				((Line)joints[i].GetLine()).X2 = pos_double_buffer_joints[i].y + x;
				((Line)joints[i].GetLine()).Y1 = pos_double_buffer_joints[i].w + y;
				((Line)joints[i].GetLine()).Y2 = pos_double_buffer_joints[i].h + y;

				Canvas.SetLeft(joints[i].GetExpanderCircle(), pos_double_buffer_joints_expander[i].X + x);
				Canvas.SetTop(joints[i].GetExpanderCircle(), pos_double_buffer_joints_expander[i].Y + y);
			}
		}
		#endregion

		#region Rect
		public struct Rect
		{
			public double x;
			public double y;
			public double w;
			public double h;

			public Rect(double x, double y, double w, double h)
			{
				this.x = x;
				this.y = y;
				this.w = w;
				this.h = h;
			}
		}
		#endregion
	}
}
