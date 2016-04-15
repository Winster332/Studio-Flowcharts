using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace WPF_Test.LMD_Compiling
{
	public class CompilingManager
	{
		#region variables
		public Language.ManagerLanguage ML;
		public Language.ManagerError ME;
		public List<String> PrevTextCode;
		public VersionFramework vf;
		public enum VersionFramework { v_3, v_3_5, v_4, v_4_5 }
		public String PATH_CODE = @"Projects\";
		public String name_project = "first_project.exe";
		public List<FileManager.ManagerProjects.INFO_BLOCK> ListInfoBlocks;
		#endregion
		#region CompillingManager
		public CompilingManager()
		{
			ME = new Language.ManagerError();
			ML = new Language.ManagerLanguage();
			ML.SetLanguage(Language.ManagerLanguage.LANGUAGE.PASCALABC);
			PrevTextCode = new List<String>();
			vf = VersionFramework.v_3_5;
			ListInfoBlocks = new List<FileManager.ManagerProjects.INFO_BLOCK>();
		}
		#endregion
		#region Run
		public int Run(List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK> blocks, LMD_GUI.LMD_BottomPanel info,
			LMD_GUI.LMD_FastInformationPanel fast_info)
		{
			info.ClearDate();
			fast_info.SetCurrentText("Начато построение");
			PrevTextCode.Clear();
			level = 0;
			ME.Clear();
			int count_block = blocks.Count;

			// Set setting from projects to compile
			FileManager.INFO_PROJECT info_project = FileManager.ManagerProjects.GetPresentProjectInfo();
			ML.SetLanguage(info_project.Language);

			info.AddItemText("Информация о проекте: ");
			info.AddItemText("Название: " + info_project.name);
			info.AddItemText("Путь: " + Environment.CurrentDirectory + "\\" + info_project.path);
			info.AddItemText("Тип проекта: " + info_project.Type);

			info.AddItemText("Колличество блоков: " + count_block);
			fast_info.ProgressBar.Value = 20;
			String lang = "";
			switch (ML.PresentLanguage)
			{
				case Language.ManagerLanguage.LANGUAGE.C_PLUS_PLUS: lang = "C++"; break;
				case Language.ManagerLanguage.LANGUAGE.C_SHARP: lang = "C#"; break;
				case Language.ManagerLanguage.LANGUAGE.JAVA: lang = "Java"; break;
				case Language.ManagerLanguage.LANGUAGE.PASCALABC: lang = "Pascal"; break;
			}
			PrevTextCode.Add("Язык построения: " + lang);
			PrevTextCode.Add("");
			info.AddItemText("Язык построения: " + lang);

			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_begin = null;
			for (int i = 0; i < count_block; i++)
				if (blocks[i].Type == LMD_GUI.TYPE_BLOCK.begin)
					block_begin = blocks[i];

			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_next = block_begin;

			info.AddItemText("Построение последовательности блоков");

			ResolutionBlocks(block_next, info);
			fast_info.ProgressBar.Value = 60;
			info.AddItemText("Собирается код");
			String res_code = "";
			List<Language.BLOCK_CODE_CONTROL_ERROR> BCCE = ME.GetItems();
			int lines = 0;
			for (int i = 0; i < BCCE.Count; i++)
			{
				for (int j = 0; j < BCCE[i].code.Length; j++)
				{
					lines++;
					PrevTextCode.Add(BCCE[i].code[j]);
					info.AddItemText(BCCE[i].code[j]);
					res_code += BCCE[i].code[j];
					BCCE[i].lines.Add(lines);
				}
			}
			fast_info.ProgressBar.Value = 80;
			switch (info_project.Type)
			{
				case FileManager.TYPE_PROJECT.DLL: this.name_project = info_project.name + ".dll"; CompileCode(res_code, info, false); break;
				case FileManager.TYPE_PROJECT.EXE: this.name_project = info_project.name + ".exe"; RunCoreCompile(info_project, info, blocks, fast_info); break;
				case FileManager.TYPE_PROJECT.NONE: break;
				case FileManager.TYPE_PROJECT.TXT: FileManager.ManagerProjects.CreateTXTCode(res_code); break;
			}

			info.AddItemText("Собирание проекта завершено");
			fast_info.SetCurrentText("Готово");
			fast_info.ProgressBar.Value = 100;
			return 0;
		}
		#endregion
		#region ResolutionBlocks
		public int level = 0;
		private int ResolutionBlocks(LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_next, LMD_GUI.LMD_BottomPanel info)
		{
			try
			{
				int result = 0;
				String str_lvl = "";

				for (int i = 0; i < level; i++)
					str_lvl += "---";

				if (block_next.element != null)
					ME.AddItem(block_next, GetCodeParseBlock(block_next.element));

				info.AddItemText("1>---" + str_lvl + "| Block[" + block_next.Type + "] Mode[" +
					block_next.Mode + "] Type element[" + block_next.element.GetType() + "] lvl[" + level + "]");

				if (block_next.Type == LMD_GUI.TYPE_BLOCK.condition)
				{
					level += 2;
					LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_next_1 = block_next.next_2;

					ResolutionBlocks(block_next_1, info);
				}

				if (block_next.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false_end ||
					block_next.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true_end)
				{
					level--;

					if (block_next.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false_end)
					{
						ME.AddItem(null, new String[] { "			" + ML.GetElseCondition() });
					}
					else if (block_next.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true_end)
					{
						ME.AddItem(null, new String[] { "			" + ML.GetCloseCondition() });
					}
				}

				block_next = block_next.next_1;
				if (block_next != null)
					ResolutionBlocks(block_next, info);

				return result;
			}
			catch
			{
				info.AddItemText("Произошла ошибка собирания блоков");
				return 0;
			}
		}
		#endregion
		#region ResolutionBlocksSave
		private int ResolutionBlocksSave(LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_next, LMD_GUI.LMD_BottomPanel info)
		{
			/*
			int result = 0;
			String str_lvl = "";

			FileManager.ManagerProjects.INFO_BLOCK b = new FileManager.ManagerProjects.INFO_BLOCK();
			b.ID = block_next.ID;
			if (block_next.next_1 != null)
			{
				b.NextID = block_next.next_1.ID;
			}
			b.Type = block_next.Type;

			if (block_next.element != null)
			{
				b.x = (float)System.Windows.Controls.Canvas.GetLeft(block_next.element);
				b.y = (float)System.Windows.Controls.Canvas.GetTop(block_next.element);
			}
			ListInfoBlocks.Add(b);

			info.AddItemText("1>--- ID[" + b.ID + "] NextID[" + b.NextID + "] Block[" + b.Type + "] ");

			for (int i = 0; i < level; i++)
				str_lvl += "---";

			if (block_next.element != null)
				ME.AddItem(block_next, GetCodeParseBlock(block_next.element));

			if (block_next.Type == LMD_GUI.TYPE_BLOCK.condition)
			{
				level += 2;
				LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_next_1 = block_next.next_2;

				ResolutionBlocksSave(block_next_1, info);
			}

			if (block_next.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false_end ||
				block_next.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true_end)
			{
				level--;

				if (block_next.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_false_end)
					ME.AddItem(null, new String[] { "			} else {" });
				else if (block_next.Mode == LMD_BLOCKS.BLOCK_JOINT.STATE_BLOCK.base_condition_true_end)
					ME.AddItem(null, new String[] { "			}" });
			}

			block_next = block_next.next_1;
			if (block_next != null)
				ResolutionBlocksSave(block_next, info);
			
			return result;
			*/
			return 0;
		}
		#endregion
		#region GetCodeParseBlock
		private String[] GetCodeParseBlock(System.Windows.UIElement element)
		{
			String[] code = null;

			#region BEGIN
			if (element.GetType() == typeof(LMD_BLOCKS.BlockBegin))
			{
				LMD_BLOCKS.BlockBegin b = (LMD_BLOCKS.BlockBegin)element;
				code = ML.GetBegin();
			}
			#endregion
			#region CALCULATE
			else if (element.GetType() == typeof(LMD_BLOCKS.BlockCalculate))
			{
				LMD_BLOCKS.BlockCalculate b = (LMD_BLOCKS.BlockCalculate)element;
				code = ML.GetCalculate(b);
			}
			#endregion
			#region CONDITION
			else if (element.GetType() == typeof(LMD_BLOCKS.BlockCondition))
			{
				LMD_BLOCKS.BlockCondition b = (LMD_BLOCKS.BlockCondition)element;
				code = ML.GetCondition(b);
			}
			#endregion
			#region END
			else if (element.GetType() == typeof(LMD_BLOCKS.BlockEnd))
			{
				LMD_BLOCKS.BlockEnd b = (LMD_BLOCKS.BlockEnd)element;
				code = ML.GetEnd();
			}
			#endregion
			#region INPUT
			else if (element.GetType() == typeof(LMD_BLOCKS.BlockInput))
			{
				LMD_BLOCKS.BlockInput b = (LMD_BLOCKS.BlockInput)element;
				code = ML.GetInput(b);
			}
			#endregion
			#region OUTPUT
			else if (element.GetType() == typeof(LMD_BLOCKS.BlockOutput))
			{
				LMD_BLOCKS.BlockOutput b = (LMD_BLOCKS.BlockOutput)element;
				code = ML.GetOutput(b);
			}
			#endregion

			return code;
		}
		#endregion
		#region CompileCode
		public void CompileCode(String code, LMD_GUI.LMD_BottomPanel info, Boolean start_file)
		{
			if (info != null)
				info.AddItemText("Начата компиляция кода");
			String v = "3.5";
			switch (vf)
			{
				case VersionFramework.v_3: v = "3"; break;
				case VersionFramework.v_3_5: v = "3.5"; break;
				case VersionFramework.v_4: v = "4"; break;
				case VersionFramework.v_4_5: v = "4.5"; break;
			}
			Dictionary<String, String> provOpt = new Dictionary<String, String> { {"CompilerVersion", "v" + v} };
			CSharpCodeProvider provider = new CSharpCodeProvider(provOpt);
			CompilerParameters compilePar = new CompilerParameters 
			{
				OutputAssembly = PATH_CODE + name_project,
				GenerateExecutable = true
			};
			CompilerResults res = provider.CompileAssemblyFromSource(compilePar, code);
			if (info != null)
				info.AddItemText(String.Format("Компиляция завершилась в [{0}] ошибок", res.Errors.Count));
			if (res.Errors.Count > 0)
			{
				if (info != null)
					info.AddItemText("Перечисление ошибок:");
				for (int i = 0; i < res.Errors.Count; i++)
				{
					CompilerError error = res.Errors[i];
					if (info != null)
						info.AddItemText(String.Format("ERROR [{0}] Line [{1}] Text: {2}", (i + 1), error.Line, error.ErrorText));
				}
			}
			else
			{
				if (start_file)
					System.Diagnostics.Process.Start(PATH_CODE + name_project);
			}

		//	try
		//	{
		//	}
		//	catch (Exception ex)
		//	{
		//		ME.GetBlock(ex);
		//	}
		}
		#endregion
		#region GetInfoBlocks
		public List<FileManager.ManagerProjects.INFO_BLOCK> GetInfoBlocks(List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK> blocks, LMD_GUI.LMD_BottomPanel info,
			LMD_GUI.LMD_FastInformationPanel fast_info)
		{
			info.ClearDate();
			List<FileManager.ManagerProjects.INFO_BLOCK> bl = new List<FileManager.ManagerProjects.INFO_BLOCK>();
			
			ListInfoBlocks.Clear();
			PrevTextCode.Clear();
			level = 0;
			ME.Clear();
			int count_block = blocks.Count;

			info.AddItemText("Сохранение проекта... " + count_block);

			String lang = "";
			switch (ML.PresentLanguage)
			{
				case Language.ManagerLanguage.LANGUAGE.C_PLUS_PLUS: lang = "C++"; break;
				case Language.ManagerLanguage.LANGUAGE.C_SHARP: lang = "C#"; break;
				case Language.ManagerLanguage.LANGUAGE.JAVA: lang = "Java"; break;
				case Language.ManagerLanguage.LANGUAGE.PASCALABC: lang = "Pascal"; break;
			}
			PrevTextCode.Add("Язык построения: " + lang);
			PrevTextCode.Add("");
			info.AddItemText("Язык построения: " + lang);

			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_begin = null;
			for (int i = 0; i < count_block; i++)
			{
				blocks[i].ID = i;
				if (blocks[i].Type == LMD_GUI.TYPE_BLOCK.begin)
					block_begin = blocks[i];
			}

			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_next = block_begin;

			info.AddItemText("Построение последовательности блоков");
			
			ResolutionBlocksSave(block_next, info);
			
			info.AddItemText("Сохранение выполнено успешно");

			return ListInfoBlocks;
		}
		#endregion
		#region RunCoreCompile
		public void RunCoreCompile(FileManager.INFO_PROJECT info_project, LMD_GUI.LMD_BottomPanel info, List<LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK> blocks, 
			LMD_GUI.LMD_FastInformationPanel fast_info)
		{
			Language.ManagerLanguage.LANGUAGE old_language = ML.PresentLanguage;
			ML.SetLanguage(Language.ManagerLanguage.LANGUAGE.C_SHARP);

			level = 0;
			ME.Clear();

			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_begin = null;
			for (int i = 0; i < blocks.Count; i++)
				if (blocks[i].Type == LMD_GUI.TYPE_BLOCK.begin)
					block_begin = blocks[i];

			LMD_BLOCKS.BLOCK_JOINT.BASE_BLOCK block_next = block_begin;

			ResolutionBlocks(block_next, info);

			String res_code = "";
			List<Language.BLOCK_CODE_CONTROL_ERROR> BCCE = ME.GetItems();
			int lines = 0;
			for (int i = 0; i < BCCE.Count; i++)
			{
				for (int j = 0; j < BCCE[i].code.Length; j++)
				{
					lines++;
					info.AddItemText(BCCE[i].code[j]);
					res_code += BCCE[i].code[j];
					BCCE[i].lines.Add(lines);
				}
			}
			
			CompileCode(res_code, null, true);
			ML.SetLanguage(old_language);
		}
		#endregion
	}
}
