using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.FileManager
{
	public enum TYPE_PROJECT { DLL, TXT, EXE, NONE }
	public class INFO_PROJECT
	{
		public LMD_Compiling.Language.ManagerLanguage.LANGUAGE Language;
		public int count_block;
		public String name;
		public String path;
		public String path_icon;
		public TYPE_PROJECT Type;
		public float workspace_width;
		public float workspace_height;
	}
}
