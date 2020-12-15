using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dweiss {


	public class MainThread : Dweiss.AutoSingleton<MainThread> {
				
		//MainThreadScript.RunInMainThread (f);
		//private static Dweiss.MainThread _threadGO = null;
		//public static Dweiss.MainThread Instance {
		//	get { 
		//		if (_threadGO == null) {
		//			throw new System.InvalidOperationException ("Main Thread script isn't located on a gameobject");
		//		}
		//		return _threadGO;
		//	}
		//	private set {
		//		if (_threadGO == null) {
		//			_threadGO = value;
		//		}
		//	}
		//}

		public delegate void MainThreadTodo();

		private List<MainThreadTodo> _q2 = new List<MainThreadTodo>();
		private List<MainThreadTodo> _queue = new List<MainThreadTodo>();

		public void RunInMainThread(MainThreadTodo f) {
			lock (_queue) {
				_queue.Add (f);
			}
		}
		protected new void Awake() {
            //Instance = this;
            base.Awake();
			DontDestroyOnLoad (gameObject);
		}


		// Update is called once per frame
		void Update () {
			lock (_queue) {
				if (_queue.Count > 0) {
					var tmp = _q2;
					_q2 = _queue;
					_queue = tmp;
				}
			}
			if (_q2.Count > 0) {
				for (int i = 0; i < _q2.Count; ++i) {
					_q2 [i] ();
				}
				_q2 = new List<MainThreadTodo> ();
			}
		}
	}
}