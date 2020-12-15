using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using Dweiss;

using System.Globalization;
using System.Threading;

namespace Common{
public class SelectWindow : EditorWindow {
//		string scriptName = "Renderer";
//		string properyName = "Materials";
		Component componentToCampare;
		string attributeName;

        bool includeInactive = true;
        //		bool groupEnabled;
        //		bool myBool = true;
        //		float myFloat = 1.23f;

        // Add menu named "My Window" to the Window menu
        [MenuItem ("Dweiss/Select")]
		static void Init () {
			// Get existing open window or if none, make a new one:
			var window = (SelectWindow)EditorWindow.GetWindow (typeof (SelectWindow));
			window.Show();
		}


		private List<T> Query<T>(Type t, string property, List<T> list, Component itemToCompareWith) {
			//Get type
			//var t = StaticFunctions.GetType(compoenentName);


			//Get property real name - case incensative 
			var flags = BindingFlags.NonPublic | BindingFlags.Public |
				BindingFlags.Instance | BindingFlags.Static;
			var cmpr = StringComparison.CurrentCultureIgnoreCase;
			PropertyInfo[] properties = t.GetProperties(flags);
			var prop = properties.FirstOrDefault(p => String.Equals(p.Name, "Shared"+property, cmpr));
			if(prop == null) prop = properties.FirstOrDefault(p => String.Equals(p.Name, property, cmpr));

            if (prop == null)
            {
                return list;
            }

            //Find attribute in script
            var compVl = prop.GetValue(itemToCompareWith, null);

			Array arrComp = null;
			if (prop != null && prop.PropertyType.IsArray) {
				arrComp = (Array)prop.GetValue (itemToCompareWith, null);
			}
			Debug.Log("prop compare to <" + compVl + ">");
            //Debug.Log("Editor Override " + prop.Name + " = " + prm + " components: " + list.ToCommaString() + " (Type Found:" + (t == null ? "NULL" : t.ToString()) + ") by " + itemsToChange[i]);


            var res = list.Where(obj => {
				var vl = prop.GetValue(obj, null);

				if (prop.PropertyType.IsArray) {
					Array arr = (Array)prop.GetValue(obj, null);
					if(arrComp.Length != arr.Length) return false;

					//Debug.Log("prop is " + arr.ToListCasted<object>().ToCommaString());
					for(int i = 0; i < arr.Length; i++)
					{
						var cmpObj = arrComp.GetValue(i);
						var oObj = arr.GetValue(i);

						if(oObj == null && cmpObj == null) continue;

						if((oObj == null && cmpObj != null) || (cmpObj == null && oObj != null)) return false;
						if(oObj.Equals( cmpObj) == false) return false;
					}

					return true;
				}
				//Debug.LogFormat("{0} - Not array prop is {1}={2}? {3}", obj, vl, compVl, vl.Equals(compVl));
				return (vl == null && compVl == null) || (vl != null && vl.Equals(compVl));
			}).ToList();
			return res;
		}

		void OnGUI () {
			GUILayout.Label ("Select Settings", EditorStyles.boldLabel);
            includeInactive = EditorGUILayout.Toggle("Include inactive", includeInactive);
			GUILayout.Label ("Select component to compare with");
			componentToCampare = (Component) EditorGUILayout.ObjectField (componentToCampare, typeof(Component), true);

			GUILayout.Label ("Select attribute to compare");
			attributeName = EditorGUILayout.TextField ("Attribute ", attributeName);

			if (GUILayout.Button ("Select")) {
				var goList = Selection.gameObjects;
                var newList = new HashSet<GameObject>();
                foreach (var go in goList)
                {
                    foreach(var t in go.GetComponentsInChildren<Transform>(includeInactive))
                    {
                        newList.Add(t.gameObject);
                    }
                }
                goList = newList.ToArray();
                if (Selection.gameObjects.Length == 0) {
					goList = GameObject.FindObjectsOfType<GameObject>() ;
				}

				var res = Query (componentToCampare.GetType (), attributeName, goList.Select (a => a.GetComponent (componentToCampare.GetType ())).Where(a => a!=null).ToList (), componentToCampare);
				Debug.Log ("Found " + res.ToCommaString());

				//var rndrs = goList.Select (a => a.GetComponent<Renderer> ()).ToList();
				//rndrs.ForEach (r => Debug.LogFormat ("{0} - shared {1}, {2}",r.name, r.sharedMaterials, r.sharedMaterials[0] == null));
				var newGo = res.Select(a => a.gameObject).ToArray ();
				Selection.objects = newGo;
			}
		}
	}
}


namespace Dweiss
{
	public static class StaticFunctions
	{

		public static System.Type GetType(string TypeName)
		{

			// Try Type.GetType() first. This will work with types defined
			// by the Mono runtime, in the same assembly as the caller, etc.
			var type = Type.GetType(TypeName);

			// If it worked, then we're done here
			if (type != null)
				return type;

			// If the TypeName is a full name, then we can try loading the defining assembly directly
			if (TypeName.Contains("."))
			{

				// Get the name of the assembly (Assumption is that we are using 
				// fully-qualified type names)
				var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

				// Attempt to load the indicated Assembly
				var assembly = Assembly.Load(assemblyName);
				if (assembly == null)
					return null;

				// Ask that assembly to return the proper Type
				type = assembly.GetType(TypeName);
				if (type != null)
					return type;

			}

			// If we still haven't found the proper type, we can enumerate all of the 
			// loaded assemblies and see if any of them define the type
			var currentAssembly = Assembly.GetExecutingAssembly();
			var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
			foreach (var assemblyName in referencedAssemblies)
			{

				// Load the referenced assembly
				var assembly = Assembly.Load(assemblyName);
				if (assembly != null)
				{
					// See if that assembly defines the named type
					type = assembly.GetType(TypeName);
					if (type != null)
						return type;
				}
			}

			// The type just couldn't be found...
			return null;

		}

	}
}