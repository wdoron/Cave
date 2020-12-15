using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Msg
{
    public class RaiseMsgEnum : RaiseMsgGeneric<int>{



        public void RaiseOtherMessageByMsgEnumName(string id ) { RaiseMsg(MsgEnum.GetId(id)); }
        public void RaiseOtherMessageByMsgEnumName(string id, float v) { RaiseMsg(MsgEnum.GetId(id), v); }
        public void RaiseOtherMessageByMsgEnumName(string id, int v) { RaiseMsg(MsgEnum.GetId(id), v); }
        public void RaiseOtherMessageByMsgEnumName(string id, string v) { RaiseMsg(MsgEnum.GetId(id), v); }
        public void RaiseOtherMessageByMsgEnumName(string id, bool v) { RaiseMsg(MsgEnum.GetId(id), v); }
        public void RaiseOtherMessageByMsgEnumName(string id, System.Object obj) { RaiseMsg(MsgEnum.GetId(id), obj); }
        public void RaiseOtherMessageByMsgEnumName(string id, UnityEngine.Object v) { RaiseMsg(MsgEnum.GetId(id), v); }
        public void RaiseOtherMessageByMsgEnumName(string id, Component v) { RaiseMsg(MsgEnum.GetId(id), v); }
        public void RaiseOtherMessageByMsgEnumName(string id, GameObject v) { RaiseMsg(MsgEnum.GetId(id), v); }

    }
}
