using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_Compiling
{
	public static class ManagerVariables
	{
		#region variables
		public static int Count
		{
			get
			{
				return variables.Count;
			}
		}
		private static List<INFO_VARIABLE> variables = new List<INFO_VARIABLE>();
		#endregion
		#region Add
		/// <summary>
		/// Добавляет новую переменную по параметрам
		/// </summary>
		/// <param name="name">Имя переменной</param>
		/// <param name="type">Тип переменной</param>
		/// <param name="date">Данные переменной</param>
		/// <param name="element">UI из которого создавалась эта переменная</param>
		/// <returns></returns>
		public static INFO_VARIABLE Add(String name, String type, String date, System.Windows.UIElement element)
		{
			INFO_VARIABLE info = new INFO_VARIABLE();
			info.name = name;
			info.type = type;
			info.data = date;
			info.element = element;

			variables.Add(info);

			return info;
		}
		/// <summary>
		/// Добавляет переменную по уже заданным данным
		/// </summary>
		/// <param name="info">Данные о переменной</param>
		/// <returns></returns>
		public static INFO_VARIABLE Add(INFO_VARIABLE info)
		{
			variables.Add(info);

			return info;
		}
		#endregion
		#region Remove
		/// <summary>
		/// Удаляет переменную с заданным именем
		/// </summary>
		/// <param name="name">Имя переменной</param>
		public static void Remove(String name)
		{
			for (int i = 0; i < variables.Count; i++)
				if (variables[i].name == name)
				{
					variables.RemoveAt(i);
					break;
				}
		}
		/// <summary>
		/// Удаляет переменную данного объекта
		/// </summary>
		/// <param name="info">Удаляемый объект</param>
		public static void Remove(INFO_VARIABLE info)
		{
			variables.Remove(info);
		}
		/// <summary>
		/// Удаляет переменную по привязанному к ней UI элементу
		/// </summary>
		/// <param name="el">UI элемент</param>
		public static void Remove(System.Windows.UIElement el)
		{
			for (int i = 0; i < variables.Count; i++)
				if (variables[i].element == el)
				{
					variables.RemoveAt(i);
					break;
				}
		}
		#endregion
		#region RemoveAt
		/// <summary>
		/// Удаляет переменную заданного индекса
		/// </summary>
		/// <param name="index"></param>
		public static void RemoveAt(int index)
		{
			variables.RemoveAt(index);
		}
		#endregion
		#region Clear
		/// <summary>
		/// Очищает все переменные
		/// </summary>
		public static void Clear()
		{
			variables.Clear();
		}
		#endregion
		#region Exist
		/// <summary>
		/// Проверяет существование переменной по объекту
		/// </summary>
		/// <param name="info">Проверяемый объект</param>
		/// <returns></returns>
		public static Boolean Exist(INFO_VARIABLE info)
		{
			for (int i = 0; i < variables.Count; i++)
				if (variables[i] == info)
					return true;

			return false;
		}
		/// <summary>
		/// Проверяет существование объекта по имени
		/// </summary>
		/// <param name="name">Имя проверяемого объекта</param>
		/// <returns></returns>
		public static Boolean Exist(String name)
		{
			for (int i = 0; i < variables.Count; i++)
				if (variables[i].name == name)
					return true;

			return false;
		}
		/// <summary>
		/// Проверяет существование переменной по заданному UI элементу
		/// </summary>
		/// <param name="el">UI элемент</param>
		/// <returns></returns>
		public static Boolean Exist(System.Windows.UIElement el)
		{
			for (int i = 0; i < variables.Count; i++)
				if (variables[i].element == el)
					return true;

			return false;
		}
		#endregion
		#region GetAllVariables
		public static List<INFO_VARIABLE> GetAllVariables()
		{
			return variables;
		}
		#endregion
		#region GetVar
		/// <summary>
		/// Возвращает переменную связанную с этим именем
		/// </summary>
		/// <param name="name">Имя переменной</param>
		/// <returns></returns>
		public static INFO_VARIABLE GetVar(String name)
		{
			for (int i = 0; i < Count; i++)
				if (variables[i].name == name)
					return variables[i];

			return null;
		}
		/// <summary>
		/// Возвращает переменную связанную с данным UI элементом
		/// </summary>
		/// <param name="el">UI элемент</param>
		/// <returns></returns>
		public static INFO_VARIABLE GetVar(System.Windows.UIElement el)
		{
			for (int i = 0; i < Count; i++)
				if (variables[i].element == el)
					return variables[i];

			return null;
		}
		#endregion
	}
}
