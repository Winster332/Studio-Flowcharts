using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_Compiling.Language
{
	public static class C_Sharp
	{
		#region variables
		public static Boolean IsComment = false;
		#endregion
		#region BEGIN
		public static String[] BEGIN()
		{
			#region Base code
			String[] bb = new String[] {
			"using System;",

			"static class Program",
			"{",
			"	[STAThread]",
			"	static void Main()",
			"	{" 
			};
			
			String[] res = new String[bb.Length + LMD_Compiling.ManagerVariables.Count];
			int i = 0;
			for (i = 0; i < bb.Length; i++)
			{
				res[i] = bb[i];
			}
			#endregion
			#region create var
			List<LMD_Compiling.INFO_VARIABLE> info = LMD_Compiling.ManagerVariables.GetAllVariables();
			
			for (int j = 0; j < info.Count; j++)
			{
				String v = "		";
				v += info[j].type + " " + info[j].name;

				if (info[j].data != "")
					v += " = " + info[j].data + ";";
				else v += ";";

				res[i+j] = v;
			}
			#endregion

			return res;
		}
		#endregion
		#region END
		public static String[] END = new String[] {
			"		Console.ReadKey();",
			"	}",
			"}"
		};
		#endregion
		#region INPUT
		public static String[] INPUT(LMD_BLOCKS.BlockInput b)
		{
			String[] code = new String[2];
			String variable = b.GetVariable();
			String view = b.GetPreviewText();
			
			if (view != "")
			{
				code[0] = "		Console.WriteLine(\"" + b.GetPreviewText() + "\");";
				if (variable != "")
				{
					INFO_VARIABLE info_variable = ManagerVariables.GetVar(variable);
					code[1] = "		" + b.GetVariable() + " = " + info_variable.type + ".Parse(Console.ReadLine());";
				}
				else code[1] = "		Console.ReadLine();";
			}
			else
			{
				if (variable != "")
				{
					INFO_VARIABLE info_variable = ManagerVariables.GetVar(variable);
					code[0] = "		" + b.GetVariable() + " = " + info_variable.type + ".Parse(Console.ReadLine());";
				}
				else code[0] = "		Console.ReadLine();";
			}

			return code;
		}
		#endregion
		#region OUTPUT
		public static String[] OUTPUT(LMD_BLOCKS.BlockOutput b)
		{
			String[] code = new String[1];
			
			if (b.GetVariable() != "")
				code[0] = "		Console.WriteLine(\"" + b.GetText() + "\"+" + b.GetVariable() + ");";
			else code[0] = "		Console.WriteLine(\"" + b.GetText() + "\");";

			return code;
		}
		#endregion
		#region CALCULATE
		public static String[] CALCULATE(LMD_BLOCKS.BlockCalculate b)
		{
			String[] code = new String[1];
			if (b.GetText() != "")
				code[0] = "		" + b.txtView.Text + ";";

			return code;
		}
		#endregion
		#region CONDITION
		public static String[] CONDITION(LMD_BLOCKS.BlockCondition b)
		{
			String[] code = new String[2];
			code[0] = "		if (" + b.txtView.Text + ")";
			code[1] = "		{";

			return code;
		}
		#endregion
	}
}
