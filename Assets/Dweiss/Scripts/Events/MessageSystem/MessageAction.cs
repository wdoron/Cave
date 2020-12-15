using UnityEngine;

namespace Dweiss
{
    public class MessageAction : MonoBehaviour
    {

        public string msgRegistrationStr = "MessageToMe";
        public SimpleEvent onAction;

        public bool activeWhenDisable2 = false;

        private void Action()
        {
            if(activeWhenDisable2 || enabled)
                onAction.Invoke();
        }

        private void Awake()
        {
            //if (msgRegistrationStr.ToLower() == "levelfinish")
            //    Debug.LogFormat("--Register {1} by {0}", this.FullName(), msgRegistrationStr);

            MsgSystemStr.S.Register(msgRegistrationStr, Action);
        }

        private void OnDestroy()
        {
            //if (msgRegistrationStr.ToLower() == "levelfinish")
            //    Debug.LogFormat("--Unregister {1} by {0}", this.FullName(), msgRegistrationStr);

            if (MsgSystemStr.S)
                MsgSystemStr.S.Unregister(msgRegistrationStr, Action);
        }
    }
}