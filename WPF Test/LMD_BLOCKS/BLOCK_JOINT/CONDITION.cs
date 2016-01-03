using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_BLOCKS.BLOCK_JOINT
{
	public class CONDITION : BASE_BLOCK
	{
		public List<BASE_BLOCK> line_block_1;
		public List<BASE_BLOCK> line_block_2;

		public CONDITION()
		{
			this.line_block_1 = new List<BASE_BLOCK>();
			this.line_block_2 = new List<BASE_BLOCK>();

			this.Type = LMD_GUI.TYPE_BLOCK.condition;
		}
	}
}
