using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_Compiling.Language
{
	public class BLOCK_CODE_CONTROL_ERROR
	{
		public String[] code;
		public LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block;
		public System.Windows.UIElement element
		{
			get { return this.block.element; }
			set { this.block.element = value; }
		}
		public List<int> lines; // Отвечает за хранение идентификаторов строк кода

		public BLOCK_CODE_CONTROL_ERROR()
		{
			lines = new List<int>();
		}
	}
}
