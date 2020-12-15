using UnityEngine;

namespace Dweiss
{
    public class MultiMessagesAction : MonoBehaviour
    {

        public string[] msgRegistrationStr;
        [SerializeField] private float delay = -1;

        public bool activeWhenDisable = false;
        public SimpleEvent onAction;

        private void Action()
        {
            if (delay < 0)
            {
                onAction.Invoke();
            }
            else
            {
                this.WaitForSeconds(delay, onAction.Invoke);
            }
        }

        private void OnEnable()
        {
            if(activeWhenDisable == false)
            {
                ManageRegistration(true);
            }
        }
        private void OnDisable()
        {
            if (activeWhenDisable == false && MsgSystemStr.S)
            {
                ManageRegistration(false);
            }
        }

        private void Awake()
        {
            if (activeWhenDisable)
            {
                ManageRegistration(true);
            }
        }

        private void OnDestroy()
        {
            if (activeWhenDisable && MsgSystemStr.S)
            {
                ManageRegistration(false);
            }
        }

        private void ManageRegistration(bool register)
        {
            if (register)
            {
                foreach (var m in msgRegistrationStr)
                {
                    //if(m.ToLower() == "levelfinish")
                    //    Debug.LogFormat("--Register {0} by {1}", m, this.FullName() );
                    MsgSystemStr.S.Register(m, Action);
                }
            }
            else
            {
                foreach (var m in msgRegistrationStr)
                {
                    //if (m.ToLower() == "levelfinish")
                    //    Debug.LogFormat("--Unregister {0} by {1}", m, this.FullName());
                    MsgSystemStr.S.Unregister(m, Action);
                }
            }
            
        }
        
    }
}