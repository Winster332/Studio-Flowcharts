using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_Compiling.Language
{
	public class ManagerError
	{
		#region variables
		private List<BLOCK_CODE_CONTROL_ERROR> blocks = new List<BLOCK_CODE_CONTROL_ERROR>();
		#endregion
		#region AddItem
		public void AddItem(LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block, String[] code)
		{
			BLOCK_CODE_CONTROL_ERROR b = new BLOCK_CODE_CONTROL_ERROR();
			b.block = block;
			b.code = code;

			blocks.Add(b);
		}
		#endregion
		#region Clear
		public void Clear()
		{
			blocks.Clear();
		}
		#endregion
		#region GetItems
		public List<BLOCK_CODE_CONTROL_ERROR> GetItems()
		{
			return blocks;
		}
		#endregion
		#region GetBlock
		public LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK GetBlock(Exception ex)
		{
			return null;
		}
		#endregion
	}
}
