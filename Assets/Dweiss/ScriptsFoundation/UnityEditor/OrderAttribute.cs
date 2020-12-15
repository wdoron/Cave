using UnityEngine;

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dweiss {


	[AttributeUsage(AttributeTargets.Class)]
	public class OrderAttribute : System.Attribute  {
		public short sequence;
		public OrderAttribute(short awakeSequance)   {
			sequence = awakeSequance;
		}

	}



}