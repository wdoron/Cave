using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dweiss;

public class MedianList<E> {
	public List<MedianGroup<E>> numList = new List<MedianGroup<E>>() ;
	public MedianList(int numOfMedianGroups, System.Comparison<E> cmp, int medianListSize = 7) {
		for(int i=0; i < numOfMedianGroups; ++i) {
			numList.Add(new MedianGroup<E>(cmp, medianListSize));
		}
	}
	public void Add(params E[] num) {
		for (int i = 0; i < num.Length; ++i) {
			numList [i].Add (num [i]);
		}
//		Debug.Log (Time.time + " Added " + numList.Select(a => a.states.Count).ToCommaString());
	}

	public E[] GetMedian() {
		var ret = new E[numList.Count];
		for (int i = 0; i < numList.Count; ++i) {
			ret [i] = numList[i].GetMedian ();
		}
//		Debug.Log (Time.time + " Median " + numList.Select(a => a.states.Count).ToCommaString() + " => " + ret);
		return ret;
	}
}

public static class MedianExtended {
	public static void Add(this MedianList<float> ml, Quaternion rot) {
		ml.Add (rot.x, rot.y, rot.z, rot.w);
	}

	public static Quaternion GetQuaterionMedian(this MedianList<float> ml) {
		var ret = ml.GetMedian ();
		return new Quaternion (ret [0], ret [1], ret [2], ret [3]);
	}

	public static Quaternion GetQuaterionAverage(this MedianList<float> ml) {
		var ret = ml.GetAverage ();
		return new Quaternion (ret [0], ret [1], ret [2], ret [3]);
	}

	public static void Add(this MedianList<float> ml, Vector3 rot) {
		ml.Add (rot.x, rot.y, rot.z);
	}

	public static Vector3 GetVector3Median(this MedianList<float> ml) {
		var ret = ml.GetMedian ();
		return new Vector3 (ret [0], ret [1], ret [2]);
	}

	public static Vector3 GetVector3Average(this MedianList<float> ml) {
		var ret = ml.GetAverage ();
		return new Vector3 (ret [0], ret [1], ret [2]);
	}

	public static float[] GetAverage(this MedianList<float> that) {
		var ret = new float[that.numList.Count];
		for (int i = 0; i < that.numList.Count; ++i) {
			ret [i] = that.numList[i].GetAverage ();
		}
		return ret;
	}

	public static float GetAverage(this MedianGroup<float> that) {
		float avg = 0;
		for (int i = 0; i < that.states.Count; ++i) {
			avg += that.states [i].item;
		}
		return avg/that.states.Count;
	}
}




public class MedianGroup<T>
{
	public class Item<E> {
		public E item;
		public System.DateTime time;
	}

	public int maxSize;
	public List<Item<T>> states;

	private System.Comparison<T> _compare;

	public MedianGroup(System.Comparison<T> cmp, int maxSize = 9) {
		_compare = cmp;
		this.maxSize = maxSize;
		states = new List<Item<T>>(maxSize);
	}
	public void Add(T v) {
		while(states.Count >= maxSize) {
//			Debug.Log ("Reached max " + states.Count + "/" + maxSize + " remove " + states.ToCommaString() );
			states.Remove(states.MinItem( (s1, s2) => (int)(s1.time - s2.time).Ticks));
		}

		var newItem = new Item<T> (){ item = v,  time = System.DateTime.Now };
//		Debug.Log ("Trying to add " + newItem.item + ", " + newItem.time.ToString("mm:ss.fff") + " " + states.ToCommaString());
			
			
//		} else {
		if (states.Count != 0) {
			for (int i = 0; i < states.Count; ++i) {
				if (_compare (newItem.item, states [i].item) > 0) {
//					Debug.Log ("Add to middle list " + newItem.item + " at " + i);
					states.Push (newItem, i);
					return;
				}
			}
		}
//		Debug.Log ("Add to end of list " + newItem.item);
		states.Add (newItem);
	}
	//(v1, v2) => (int)((v1 - v2).magnitude)
	public T GetMedian() {
//		Debug.Log(states.Select(a => string.Format("[V{0},T{1}]", a.item, a.time)).ToCommaString());
		return states[states.Count / 2].item;
	}



}