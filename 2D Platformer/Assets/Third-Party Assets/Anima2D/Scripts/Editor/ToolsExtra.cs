﻿using UnityEditor;
using System.Reflection;

namespace Anima2D
{
    [InitializeOnLoad]
	public class ToolsExtra
	{
		static PropertyInfo s_ViewToolActivePropertyInfo;

		static ToolsExtra()
		{
			s_ViewToolActivePropertyInfo = typeof(Tools).GetProperty("viewToolActive",BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		}

		public static bool viewToolActive {
			get {
				return (bool) s_ViewToolActivePropertyInfo.GetValue(null,null);
			}
		}
	}
}
