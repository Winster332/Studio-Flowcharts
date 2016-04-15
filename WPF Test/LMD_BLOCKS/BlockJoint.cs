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
	public enum TYPE_LINE { standart, marshrut, standart_end, condition, line_line, line_block, end_condition }
	public class BlockJoint
	{
		#region variables
		private LMD_GUI.LMD_CircleExpander expander;
		private Line line;
		private Canvas parent_canvas;
		private LMD_GUI.LMD_Workspace workspace;
		private double _x;
		private double _y;
		private BlockJoint block_next;
	//	public LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK joint;
		public TYPE_LINE type_line;
		public UIElement element_begin;
		public UIElement element_end;
		public LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK State;

		public delegate void CreateNewJoint(BlockJoint joint);
		public event CreateNewJoint createNewJoint;
		public delegate void RemoveNewJoint(BlockJoint joint);
		public event RemoveNewJoint removeJoint;
		#endregion
		#region BlockJoint
		public BlockJoint(LMD_GUI.LMD_Workspace workspace)
		{
			this.workspace = workspace;
			this.State = BLOCK_JOINT.STATE_BLOCK.none;
		}
		#endregion
		#region Create
		/// <summary>
		/// Создает связь на готовых соединениях
		/// </summary>
		/// <param name="canvas">Контейнер</param>
		public void Create(Canvas canvas)
		{
			this.parent_canvas = canvas;
			line = new Line();
			expander = new LMD_GUI.LMD_CircleExpander();
			expander.expanderClickItem += expander_expanderClickItem;

			#region begin line
			if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockBegin) ||
				element_begin.GetType() == typeof(LMD_BLOCKS.BlockEnd))
			{
				line.X1 = Canvas.GetLeft(element_begin) + 75;
				line.Y1 = Canvas.GetTop(element_begin) + 35;
			}
			else if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockInput) ||
				element_begin.GetType() == typeof(LMD_BLOCKS.BlockOutput) ||
				element_begin.GetType() == typeof(LMD_BLOCKS.BlockCalculate))
			{
				line.X1 = Canvas.GetLeft(element_begin) + 140;
				line.Y1 = Canvas.GetTop(element_begin) + 35;
			}
			#endregion
			#region end line
			if (element_end.GetType() == typeof(LMD_BLOCKS.BlockBegin) ||
				element_end.GetType() == typeof(LMD_BLOCKS.BlockEnd))
			{
				line.X2 = Canvas.GetLeft(element_end) + 75;
				line.Y2 = Canvas.GetTop(element_end);
			}
			else if (element_end.GetType() == typeof(LMD_BLOCKS.BlockInput) ||
				element_end.GetType() == typeof(LMD_BLOCKS.BlockOutput) ||
				element_end.GetType() == typeof(LMD_BLOCKS.BlockCalculate))
			{
				line.X2 = Canvas.GetLeft(element_end) + 140;
				line.Y2 = Canvas.GetTop(element_end);
			}
			else if (element_end.GetType() == typeof(LMD_BLOCKS.BlockCondition))
			{
				line.X2 = Canvas.GetLeft(element_end) + 175;
				line.Y2 = Canvas.GetTop(element_end);
			}
			#endregion

			line.StrokeThickness = 2;
			line.Stroke = new SolidColorBrush(Color.FromRgb(30, 150, 90));
			canvas.Children.Add(line);

			Canvas.SetLeft(expander, line.X2 - 10);
			Canvas.SetTop(expander, line.Y2 - 10);
			canvas.Children.Add(expander);

			workspace.selectJoint = null;

			/*
			if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockBegin))
				joint.Type = LMD_GUI.TYPE_BLOCK.begin;
			else if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockEnd))
				joint.Type = LMD_GUI.TYPE_BLOCK.end;
			else if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockCalculate))
				joint.Type = LMD_GUI.TYPE_BLOCK.calculate;
			else if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockCondition))
				joint.Type = LMD_GUI.TYPE_BLOCK.condition;
			else if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockInput))
				joint.Type = LMD_GUI.TYPE_BLOCK.input;
			else if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockOutput))
				joint.Type = LMD_GUI.TYPE_BLOCK.output;

			joint.element_begin = element_begin;
			joint.element_end = element_end;
			*/
			createNewJoint(this);

			expander.Tag = this;
		}
		/// <summary>
		/// Создает связь на частичных данных
		/// </summary>
		/// <param name="canvas">Контейнер</param>
		/// <param name="x">Позиция X</param>
		/// <param name="y">Позиция Y</param>
		public void Create(Canvas canvas, double x, double y)
		{
			this.parent_canvas = canvas;
			line = new Line();
			expander = new LMD_GUI.LMD_CircleExpander();
			expander.expanderClickItem += expander_expanderClickItem;

			#region begin line
			if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockBegin) ||
				element_begin.GetType() == typeof(LMD_BLOCKS.BlockEnd))
			{
				line.X1 = Canvas.GetLeft(element_begin) + 75;
				line.Y1 = Canvas.GetTop(element_begin) + 35;
			}
			else if (element_begin.GetType() == typeof(LMD_BLOCKS.BlockInput) ||
				element_begin.GetType() == typeof(LMD_BLOCKS.BlockOutput) ||
				element_begin.GetType() == typeof(LMD_BLOCKS.BlockCalculate))
			{
				line.X1 = Canvas.GetLeft(element_begin) + 140;
				line.Y1 = Canvas.GetTop(element_begin) + 35;
			}
			//	else if (selectJoint.element_begin.GetType() == typeof(LMD_BLOCKS.BlockCondition))
			//	{
			//		line.X1 = Canvas.GetLeft(selectJoint.element_begin) + 125;
			//		line.Y1 = Canvas.GetTop(selectJoint.element_begin) + 100;
			//	}
			#endregion
			#region end line
			line.X2 = x;
			line.Y2 = y;
			#endregion

			line.StrokeThickness = 2;
			line.Stroke = new SolidColorBrush(Color.FromRgb(30, 150, 90));
			canvas.Children.Add(line);

			Canvas.SetLeft(expander, line.X2 - 10);
			Canvas.SetTop(expander, line.Y2 - 10);
			canvas.Children.Add(expander);

			workspace.selectJoint = null;

			createNewJoint(this);
			expander.Tag = this;

		}
		/// <summary>
		/// Создает связь основаную на старых координатах связи
		/// </summary>
		/// <param name="canvas">Контейнер</param>
		/// <param name="x2">Новые координаты x</param>
		/// <param name="y2">Новые координаты y</param>
		public void CreateOldXY(Canvas canvas, double x2, double y2)
		{
			this.parent_canvas = canvas;
			line = new Line();
			expander = new LMD_GUI.LMD_CircleExpander();
			expander.expanderClickItem += expander_expanderClickItem;

			#region begin line
			line.X1 = _x;
			line.Y1 = _y;
			#endregion
			#region end line
			line.X2 = x2;
			line.Y2 = y2;
			#endregion

			line.StrokeThickness = 2;
			line.Stroke = new SolidColorBrush(Color.FromRgb(30, 150, 90));
			canvas.Children.Add(line);

			Canvas.SetLeft(expander, line.X2 - 10);
			Canvas.SetTop(expander, line.Y2 - 10);
			canvas.Children.Add(expander);

			workspace.selectJoint = null;

			createNewJoint(this);
			expander.Tag = this;
		}
		/// <summary>
		/// Создает связь на старых координатах основаную на новом объекте
		/// </summary>
		/// <param name="canvas">Контейнер</param>
		/// <param name="element_end">Новый элемент</param>
		public void CreateOldXYToBlock(Canvas canvas, UIElement element_end)
		{
			this.parent_canvas = canvas;
			line = new Line();
			expander = new LMD_GUI.LMD_CircleExpander();
			expander.expanderClickItem += expander_expanderClickItem;

			#region begin line
			line.X1 = _x;
			line.Y1 = _y;
			#endregion
			#region end line
			#region begin line
			if (element_end.GetType() == typeof(LMD_BLOCKS.BlockBegin) ||
				element_end.GetType() == typeof(LMD_BLOCKS.BlockEnd))
			{
				line.X2 = Canvas.GetLeft(element_end) + 75;
				line.Y2 = Canvas.GetTop(element_end);
			}
			else if (element_end.GetType() == typeof(LMD_BLOCKS.BlockInput) ||
				element_end.GetType() == typeof(LMD_BLOCKS.BlockOutput) ||
				element_end.GetType() == typeof(LMD_BLOCKS.BlockCalculate))
			{
				line.X2 = Canvas.GetLeft(element_end) + 140;
				line.Y2 = Canvas.GetTop(element_end);
			}
			else if (element_end.GetType() == typeof(LMD_BLOCKS.BlockCondition))
			{
				line.X2 = Canvas.GetLeft(element_end) + 175;
				line.Y2 = Canvas.GetTop(element_end);
			}
			#endregion
			#endregion

			line.StrokeThickness = 2;
			line.Stroke = new SolidColorBrush(Color.FromRgb(30, 150, 90));
			canvas.Children.Add(line);

			Canvas.SetLeft(expander, line.X2 - 10);
			Canvas.SetTop(expander, line.Y2 - 10);
			canvas.Children.Add(expander);

			workspace.selectJoint = null;

			createNewJoint(this);
			expander.Tag = this;
		}
		#endregion
		#region SelectXY
		public void SelectXY(double x, double y)
		{
			this._x = x;
			this._y = y;
		}
		#endregion
		#region expander click items
		private void expander_expanderClickItem(EXPANDER_FUNCTION function, UIElement element)
		{
			BlockJoint bj = (BlockJoint)((LMD_GUI.LMD_CircleExpander)element).Tag; // Блок следующей/предидущей связи
			switch(function)
			{
				case EXPANDER_FUNCTION.joint:
					if (workspace.selectJoint == null)
					{
						workspace.selectJoint = new BlockJoint(workspace);
						workspace.selectJoint.element_begin = bj.element_begin;
						workspace.selectJoint.createNewJoint += workspace.selectJoint_createNewJoint;
						workspace.selectJoint.removeJoint += workspace.selectJoint_removeJoint;
						workspace.selectJoint.SelectXY(Canvas.GetLeft(element) + 10, Canvas.GetTop(element) + 10);
						workspace.selectJoint.type_line = TYPE_LINE.line_block;
						workspace.joint_select = true;
					}
					else if (workspace.selectJoint != null) // Линия к линии
					{
						SelectXY(Canvas.GetLeft(workspace.selectJoint.element_begin) + 10,
							Canvas.GetTop(workspace.selectJoint.element_begin) + 10);
						workspace.selectJoint.type_line = TYPE_LINE.end_condition;
						workspace.selectJoint.element_end = bj.element_begin;
						workspace.selectJoint.Create(workspace.canvas, Canvas.GetLeft(element) + 10, Canvas.GetTop(element) + 10);
						workspace.joint_select = false;

						createNewJoint(this);
					}
					break;
				case EXPANDER_FUNCTION.properties: break;
				case EXPANDER_FUNCTION.delete: ThisRemove(); break;
			}
		}
		#endregion
		#region ThisRemove
		public void ThisRemove()
		{
			removeJoint(this);

			parent_canvas.Children.Remove(expander);
			parent_canvas.Children.Remove(line);
			element_begin = null;
			element_end = null;
		}
		#endregion
		#region GetNext
		public BlockJoint GetNext()
		{
			return this.block_next;
		}
		#endregion
		#region SetNext
		public void SetNext(BlockJoint block)
		{
			this.block_next = block;
		}
		#endregion
		#region GetLine
		public UIElement GetLine()
		{
			return line;
		}
		#endregion
		#region GetExpanderCircle
		public UIElement GetExpanderCircle()
		{
			return expander;
		}
		#endregion
	}
}
