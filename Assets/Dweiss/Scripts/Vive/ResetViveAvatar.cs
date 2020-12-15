using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class ResetViveAvatar : MonoBehaviour
    {
        public float heightFactorFromEyes = 1.07f;
        private Vector3 startScale;
        private void Awake()
        {
            startScale = transform.localScale;
        }

        void ResetHeight()
        {
            var userPos = Camera.main.transform.localPosition;
            var newScale = startScale * userPos.y * heightFactorFromEyes;
            transform.localScale = newScale;
        }

        private void OnEnable()
        {
            MsgSystemStr.S.Register("ViveResetEnd", ResetHeight);
        }

        private void OnDisable()
        {
            if(MsgSystemStr.S)
                MsgSystemStr.S.Unregister("ViveResetEnd", ResetHeight);
        }
    }
}