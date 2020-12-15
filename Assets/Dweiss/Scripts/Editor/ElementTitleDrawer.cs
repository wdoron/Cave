
using UnityEngine;
using UnityEditor;
using Dweiss.ReflectionWrapper;
using System.Reflection;
using System.Linq;

namespace Dweiss
{
    [CustomPropertyDrawer(typeof(ElementTitleAttribute))]
    public class ElementTitleDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                        GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        protected virtual ElementTitleAttribute Atribute
        {
            get { return (ElementTitleAttribute)attribute; }
        }
        SerializedProperty TitleNameProp;
        public override void OnGUI(Rect position,
                                  SerializedProperty property,
                                  GUIContent label)
        {
            string newlabel = null;
            try
            {
                newlabel = GetPropertyDescription(property, Atribute.Varname);
                //if (string.IsNullOrEmpty(newlabel))
                //{
                //    string FullPathName = property.propertyPath + "." + Atribute.Varname;
                //    TitleNameProp = property.serializedObject.FindProperty(FullPathName);

                //    newlabel = GetTitle();
                //}
                if (string.IsNullOrEmpty(newlabel))
                    newlabel = label.text;
                EditorGUI.PropertyField(position, property, new GUIContent(newlabel, label.tooltip), true);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exeption (Use function not property?)" + e);
                throw;
            }
        }
        private string GetPropertyDescription(SerializedProperty property, string fieldName)
        {
            var obj = GetTargetObjectOfProperty(property);
            if (obj == null) return "";
            var primitives = ReflectionUtils.GetPrimitiveMembers(obj.GetType());
            foreach (var m in primitives)
            {
                if (fieldName == m.Name)
                {
                    switch (m.MemberT)
                    {
                        case System.Reflection.MemberTypes.Field:
                            return ReflectionUtils.GetField(obj, fieldName).ToString();
                        case System.Reflection.MemberTypes.Property:
                            return ReflectionUtils.GetProperty(obj, fieldName).ToString();

                        case System.Reflection.MemberTypes.Method:
                            return ReflectionUtils.SetFunc(obj, fieldName, null).ToString();
                        default: throw new System.NotSupportedException("Only fields and properties are supported ");
                    }

                }
            }
            throw new System.ArgumentException("no field or property named " + fieldName + " <<in>> " + string.Join(",", primitives.Select(a => a.ToString()).ToArray()));
        }

        /// <summary>
        /// Gets the object the property represents.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            //Debug.Log("Info " + prop.propertyPath);
            var path = prop.propertyPath;// prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            //var element = path;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }


        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }


        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }

        private string GetTitle()
        {
            if (TitleNameProp == null) return "";

            switch (TitleNameProp.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    return TitleNameProp.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return TitleNameProp.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return TitleNameProp.floatValue.ToString();
                case SerializedPropertyType.String:
                    return TitleNameProp.stringValue;
                case SerializedPropertyType.Color:
                    return TitleNameProp.colorValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    return TitleNameProp.objectReferenceValue.ToString();
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    return TitleNameProp.enumNames[TitleNameProp.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return TitleNameProp.vector2Value.ToString();
                case SerializedPropertyType.Vector3:
                    return TitleNameProp.vector3Value.ToString();
                case SerializedPropertyType.Vector4:
                    return TitleNameProp.vector4Value.ToString();
                case SerializedPropertyType.Rect:
                    break;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    break;
                case SerializedPropertyType.Bounds:
                    break;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Quaternion:
                    break;
                default:
                    break;
            }
            return "";
        }
    }
}