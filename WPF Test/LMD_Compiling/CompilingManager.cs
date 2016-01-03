using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_Compiling
{
	public class CompilingManager
	{
		public CompilingManager()
		{
		}

		public int Run(List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK> blocks, LMD_GUI.LMD_BottomPanel info, LMD_GUI.LMD_FastInformationPanel fast_info)
		{
			int count_block = blocks.Count;
			info.AddItemText("Начата компиляция проекта...");
			info.AddItemText("Колличество блоков: " + blocks.Count);

			if (count_block <= 1)
			{
				info.AddItemText("Завершение компиляции. Причина: отсутствие блоков");
			}

			return 0;
		}
	}
}
