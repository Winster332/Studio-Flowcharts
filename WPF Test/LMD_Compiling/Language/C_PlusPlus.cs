using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_Compiling.Language
{
	public class C_PlusPlus
	{
		#region BEGIN
		public static String[] BEGIN()
		{
			String[] bb = {
			"#include \"stdafx.h\"",
			"#include <iostream>",
			"#inlcude <math.h>",

			"using namespace std;",

			"int main()",
			"{",
			"	SetLocale(LC_ALL, \"Rus\");"
							};
			String[] res = new String[bb.Length + ManagerVariables.Count];
			int i = 0;
			for (i = 0; i < bb.Length; i++)
			{
				res[i] = bb[i];
			}

			List<LMD_Compiling.INFO_VARIABLE> info = LMD_Compiling.ManagerVariables.GetAllVariables();

			for (int j = 0; j < info.Count; j++)
			{
				String v = "	";
				v += info[j].type + " " + info[j].name;

				if (info[j].data != "")
					v += " = " + info[j].data + ";";
				else v += ";";

				res[i + j] = v;
			}

			return res;
		}
		#endregion
		#region END
		public static String[] END = new String[] {
			"	system(\"PAUSE\");",
			"	return 0;",
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
				code[0] = "	cout << \"" + b.GetPreviewText() + "\";";
				if (variable != "")
				{
					INFO_VARIABLE info_variable = ManagerVariables.GetVar(variable);
					code[1] = "	cin >> " + b.GetVariable() + ";";
				}
				else code[1] = "	// Ввод данных пропущен";
			}
			else
			{
				if (variable != "")
				{
					INFO_VARIABLE info_variable = ManagerVariables.GetVar(variable);
					code[1] = "	cin >> " + b.GetVariable() + ";";
				}
				else code[1] = "	// Блок ввода не реализован";
			}

			return code;
		}
		#endregion
		#region OUTPUT
		public static String[] OUTPUT(LMD_BLOCKS.BlockOutput b)
		{
			String[] code = new String[1];

			if (b.GetVariable() != "")
				code[0] = "	cout << \"" + b.GetText() + "\" << " + b.GetVariable() + ";";
			else code[0] = "	cout << \"" + b.GetText() + "\";";

			return code;
		}
		#endregion
		#region CALCULATE
		public static String[] CALCULATE(LMD_BLOCKS.BlockCalculate b)
		{
			String[] code = new String[1];

			if (b.txtView.Text != "")
				code[0] = "	" + b.txtView.Text + ";";
			else code[0] = "	// Арифметический блок не реализован";

			return code;
		}
		#endregion
		#region CONDITION
		public static String[] CONDITION(LMD_BLOCKS.BlockCondition b)
		{
			String[] code = new String[2];
			if (b.txtView.Text != "")
			{
				code[0] = "		if (" + b.txtView.Text + ")";
				code[1] = "		{";
			}
			else
			{
				code[0] = "	// Блок условия не реализован";
				code[1] = " // Следует обязательно указать условие";
			}

			return code;
		}
		#endregion
	}
}
