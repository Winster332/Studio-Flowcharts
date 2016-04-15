using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_Compiling.Language
{
	public class PascalABC
	{
		#region BEGIN
		public static String[] BEGIN()
		{
			List<String> prev_code = new List<String>();

			#region Base code
			String[] bb = new String[] {
			"Program SF_APP1;"
			};
			List<LMD_Compiling.INFO_VARIABLE> info = LMD_Compiling.ManagerVariables.GetAllVariables();
			List<String> str_val = new List<String>();

			for (int j = 0; j < info.Count; j++)
			{
				String v = "";
				if (info[j].data != "")
					v = "	" + info[j].name + " := " + info[j].data + ";";

				str_val.Add(v);
			}

			for (int i = 0; i < bb.Length; i++)
			{
				prev_code.Add(bb[i]);
			}
			#endregion
			#region create var
			for (int j = 0; j < info.Count; j++)
			{
				String v = "var ";
				v += info[j].name + " : " + ConvertType(info[j].type) + ";";

				prev_code.Add(v);
			}
			prev_code.Add("begin");

			for (int j = 0; j < str_val.Count; j++)
			{
				prev_code.Add(str_val[j]);
			}
			#endregion

			String[] result = new String[prev_code.Count];
			for (int j = 0; j < prev_code.Count; j++)
			{
				result[j] = prev_code[j];
			}

			return result;
		}
		#endregion
		#region END
		public static String[] END = new String[] {
			"end."
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
				code[0] = "	write(\'" + b.GetPreviewText() + "\');";
				if (variable != "")
				{
					INFO_VARIABLE info_variable = ManagerVariables.GetVar(variable);
					code[1] = "	readln(" + b.GetVariable() + ");";
				}
				else code[1] = "	// Ввод данных пропущен";
			}
			else
			{
				if (variable != "")
				{
					INFO_VARIABLE info_variable = ManagerVariables.GetVar(variable);
					code[1] = "	readln(" + b.GetVariable() + ");";
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
				code[0] = "	write(\'" + b.GetText() + "\' + " + b.GetVariable() + ");";
			else code[0] = "	write(\'" + b.GetText() + "\');";

			return code;
		}
		#endregion
		#region CALCULATE
		public static String[] CALCULATE(LMD_BLOCKS.BlockCalculate b)
		{
			String[] code = new String[1];

			if (b.txtView.Text != "")
			{
				String str = b.txtView.Text;
				str = str.Replace("=", ":=");
				code[0] = "	" + str + ";";
			}
			else code[0] = "	// Арифметический блок не реализован";

			return code;
		}
		#endregion
		#region CONDITION
		public static String[] CONDITION(LMD_BLOCKS.BlockCondition b)
		{
			String[] code = new String[2];
			code[0] = "		If " + b.txtView.Text + " then";
			code[1] = "		begin";

			return code;
		}
		#endregion
		#region ConvertType
		public static String ConvertType(String type)
		{
			String result = type;

			switch (type)
			{
				case "int": result = "integer"; break;
				case "double": result = "real"; break;
				case "float": result = "real"; break;
				case "String": result = "string"; break;
				case "bool": result = "boolean"; break;
			}

			return result;
		}
		#endregion
	}
}
