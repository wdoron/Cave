using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class MonoExtension : MonoBehaviour
    {
        protected Transform _transform;
        public Transform t { get { if (_transform == null) _transform = transform; return _transform; } }
    }
}