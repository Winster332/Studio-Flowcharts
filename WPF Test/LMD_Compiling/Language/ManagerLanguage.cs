using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_Compiling.Language
{
	public class ManagerLanguage
	{
		#region variables
		public enum LANGUAGE { C_PLUS_PLUS, C_SHARP, JAVA, PASCALABC, JS, PHP, Python, Basic, TextCode }
		public LANGUAGE PresentLanguage;
		#endregion
		#region SetLanguage
		public void SetLanguage(LANGUAGE new_lan)
		{
			this.PresentLanguage = new_lan;
		}
		#endregion
		#region GetBegin
		public String[] GetBegin()
		{
			String[] result = null;
			switch (PresentLanguage)
			{
				case LANGUAGE.C_SHARP:
					result = C_Sharp.BEGIN(); 
					break;
				case LANGUAGE.C_PLUS_PLUS: 
					result = C_PlusPlus.BEGIN();
					break;
				case LANGUAGE.PASCALABC:
					result = PascalABC.BEGIN();
					break;
				case LANGUAGE.JAVA:
					result = Java.BEGIN();
					break;
			}

			return result;
		}
		#endregion
		#region GetEnd
		public String[] GetEnd()
		{
			String[] result = null;
			switch (PresentLanguage)
			{
				case LANGUAGE.C_SHARP: result = C_Sharp.END; break;
				case LANGUAGE.C_PLUS_PLUS: result = C_PlusPlus.END; break;
				case LANGUAGE.JAVA: result = Java.END; break;
				case LANGUAGE.PASCALABC: result = PascalABC.END; break;
			}

			return result;
		}
		#endregion
		#region GetInput
		public String[] GetInput(LMD_BLOCKS.BlockInput b)
		{
			String[] result = null;
			switch (PresentLanguage)
			{
				case LANGUAGE.C_SHARP: result = C_Sharp.INPUT(b); break;
				case LANGUAGE.C_PLUS_PLUS: result = C_PlusPlus.INPUT(b); break;
				case LANGUAGE.JAVA: result = Java.INPUT(b); break;
				case LANGUAGE.PASCALABC: result = PascalABC.INPUT(b); break;
			}

			return result;
		}
		#endregion
		#region GetOutput
		public String[] GetOutput(LMD_BLOCKS.BlockOutput b)
		{
			String[] result = null;
			switch (PresentLanguage)
			{
				case LANGUAGE.C_SHARP: result = C_Sharp.OUTPUT(b); break;
				case LANGUAGE.C_PLUS_PLUS: result = C_PlusPlus.OUTPUT(b); break;
				case LANGUAGE.JAVA: result = Java.OUTPUT(b); break;
				case LANGUAGE.PASCALABC: result = PascalABC.OUTPUT(b); break;
			}

			return result;
		}
		#endregion
		#region GetCalculate
		public String[] GetCalculate(LMD_BLOCKS.BlockCalculate b)
		{
			String[] result = null;
			switch (PresentLanguage)
			{
				case LANGUAGE.C_SHARP: result = C_Sharp.CALCULATE(b); break;
				case LANGUAGE.C_PLUS_PLUS: result = C_PlusPlus.CALCULATE(b); break;
				case LANGUAGE.JAVA: result = Java.CALCULATE(b); break;
				case LANGUAGE.PASCALABC: result = PascalABC.CALCULATE(b); break;
			}

			return result;
		}
		#endregion
		#region GetCondition
		public String[] GetCondition(LMD_BLOCKS.BlockCondition b)
		{
			String[] result = null;
			switch (PresentLanguage)
			{
				case LANGUAGE.C_SHARP: result = C_Sharp.CONDITION(b); break;
				case LANGUAGE.C_PLUS_PLUS: result = C_PlusPlus.CONDITION(b); break;
				case LANGUAGE.JAVA: result = Java.CONDITION(b); break;
				case LANGUAGE.PASCALABC: result = PascalABC.CONDITION(b); break;
			}

			return result;
		}
		#endregion
		#region GetCloseCondition
		public String GetCloseCondition() 
		{
			String result = "";
			switch (PresentLanguage)
			{
				case LANGUAGE.C_SHARP: result = "}"; break;
				case LANGUAGE.C_PLUS_PLUS: result = "}"; break;
				case LANGUAGE.JAVA: result = "}"; break;
				case LANGUAGE.PASCALABC: result = result = "end"; break;
			}

			return result;
		}
		#endregion
		#region GetElseCondition
		public String GetElseCondition()
		{
			String result = "";
			switch (PresentLanguage)
			{
				case LANGUAGE.C_SHARP: result = "} else {"; break;
				case LANGUAGE.C_PLUS_PLUS: result = "} else {"; break;
				case LANGUAGE.JAVA: result = "} else {"; break;
				case LANGUAGE.PASCALABC: result = result = "else"; break;
			}

			return result;
		}
		#endregion
	}
}
