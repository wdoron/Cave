using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

using System.Collections;

using System.Collections.Generic;
using System.Reflection;
using System;

#endif

namespace Dweiss {

	#if UNITY_EDITOR
	[InitializeOnLoad]
	public class SetExecutionOrder : EditorWindow{

		static SetExecutionOrder() {
//			Debug.Log(DateTime.Now.ToString("HH:mm:ss.fff")  + " INIT SetExecutionOrder");
			Change ();
			EditorApplication.playmodeStateChanged += Change;

			EditorApplication.update += CompileOnPlay;
		}
		private static bool compiledExecuted = false;
		static void CompileOnPlay() {
			
			if (EditorApplication.isCompiling) {
				compiledExecuted = true;
			} else {
				if (compiledExecuted) {
//					Debug.Log (DateTime.Now.ToString("HH:mm:ss.fff") + " CALLIING compiledExecuted: " + compiledExecuted);
					Change ();
				}
				compiledExecuted = false;
			}
		}

		static void Change()
		{
			MonoBehaviour[] sceneActive = GameObject.FindObjectsOfType<MonoBehaviour>();
			var scriptToOrder = new Dictionary<string, int> ();

			for(var j=0; j < sceneActive.Length; ++j)
			{
				MonoBehaviour mono = sceneActive [j];

				Type monoType = mono.GetType();
				var attrs = monoType.GetCustomAttributes (typeof(OrderAttribute), true).ToList();//Run
				var attr = attrs.FirstOrDefault ();
				if (attr != null) {
					var order = (attr as OrderAttribute).sequence;
					scriptToOrder [monoType.Name] = order;
//					Debug.Log ("Check " + monoType + ": " + attr);
					SetExecutionOrder.SetScriptAwakeOrder(monoType.Name, order);
				}
			}
//			Debug.Log ("Set sequence state " + string.Join(" " ,scriptToOrder.Select(a => a.Key + ":" + a.Value).ToArray()));
		}
		public static void SetScriptAwakeOrder<T> (short num) {
			string scriptName = typeof(T).Name;
			SetScriptAwakeOrder(scriptName, num);
		}
		public static void SetScriptAwakeOrder (string scriptName, short num) {
			foreach (var monoScript in UnityEditor.MonoImporter.GetAllRuntimeMonoScripts())
			{
				if (monoScript.name == scriptName)
				{
					var exeOrder = UnityEditor.MonoImporter.GetExecutionOrder (monoScript);
					if ( exeOrder != num)
					{
						Debug.Log ("Change script " + monoScript.name + " old " + exeOrder + " new " + num);
						UnityEditor.MonoImporter.SetExecutionOrder(monoScript, num);
					}
					break;
				}
			}
		}
	}
	#else
	public class SetExecutionOrder {

		static SetExecutionOrder() {}
		static void Change(){}
		public static void SetScriptAwakeOrder<T> (int num) {}
		public static void SetScriptAwakeOrder (string scriptName, int num) {}
	}
	#endif
		

}