using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_BLOCKS.BLOCK_JOINT
{
	public enum STATE_BLOCK { none, base_condition_true, 
		base_condition_false, base_condition_true_end, base_condition_false_end }
	public class BASE_BLOCK
	{
		public int ID;
		public LMD_GUI.TYPE_BLOCK Type;
		public STATE_BLOCK Mode;
		public BASE_BLOCK next_1;
		public BASE_BLOCK next_2;

		public System.Windows.UIElement element;

		public BASE_BLOCK()
		{
			Mode = STATE_BLOCK.none;
		}
	}
}
