using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dweiss.Msg
{

    public class MsgEnum : Singleton<MsgEnum>
    {
        [SerializeField]private List<string> msgOptions;
        public List<string> MsgOptions { get { return msgOptions; } set { msgOptions = value; } }

        private Dictionary<string, int> msgToId = new Dictionary<string, int>();

        protected new void Awake()
        {
            base.Awake();
            for (int i = 0; i < msgOptions.Count; ++i)
            {
                msgToId[msgOptions[i]] = i;
            }
        }

        public static string GetNameById(int id)
        {
            if (S.msgOptions.Count == 0 || S.msgOptions.Count <= id) return "null";
            return S.msgOptions[id];
        }
        

        public static int GetId(string str)
        {
            return S.msgToId[str];
        }
    }
}