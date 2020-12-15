using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Store.License
{
    [DefaultExecutionOrder(-1)]
    public class Configuration : MonoBehaviour
    {
        [Header("_Editor_")]
        public bool clearAppSettingsInEditor = true;

        [Header("_References_")]
        public SingleTimeLicense license;
        public AppSerializer appSerializer;

        [Header("_Key configuration_")]
        [Tooltip("Use the longest keyphrase as possible")]
        public string keyphrase;


        [Header("_Email_")]
        public string defualtGmailSenser = "<gmail sender>";
        public string defualtSenderPass = "<email password>", defualtEmailOfReciever = "<Email of Receiver>";

      


        private void OnValidate()
        {
            if (license == null) license = FindObjectOfType<SingleTimeLicense>();
            if(appSerializer == null) appSerializer = FindObjectOfType<AppSerializer>();

            if (license) license.keyPhrase = keyphrase;
            else Debug.LogError("Missing license to configure");

            if (appSerializer) appSerializer.keyPhrase = keyphrase;
            else Debug.LogError("Missing AppSerializer to configure");
        }

        private void Awake()
        {
            if (license == null) license = FindObjectOfType<SingleTimeLicense>();
            if (appSerializer == null) appSerializer = FindObjectOfType<AppSerializer>();

            

            if (license)
            {
                license.keyPhrase = keyphrase;
                license.onEnterLicenseInfo.AddListener(OnEnterPassword);

#if UNITY_EDITOR
                if (clearAppSettingsInEditor)
                {
                    license.ClearData();
                }
#endif
                Debug.Log("Finished override license configuration and network email configuration");
            } else
            {
                Debug.LogError("Missing license configuration");
            }

            if (appSerializer) appSerializer.keyPhrase = keyphrase;
            else Debug.LogError("Missing AppSerializer to configure");
        }

        private void OnDestroy()
        {

#if UNITY_EDITOR
            if (clearAppSettingsInEditor)
            {
                license.ClearData();
            }
#endif
        }

        [ContextMenu("Try demo email")]
        public void DemoSendEmail()
        {
            NetworkUtils.SendEmail(defualtGmailSenser, defualtSenderPass, defualtEmailOfReciever, "Send email from unity test", "body ");
        }
      
        public void OnEnterPassword(System.Tuple<string,string,string> keyNmacsNdate)
        {
            OnEnterPassword(keyNmacsNdate.Item1, keyNmacsNdate.Item2, keyNmacsNdate.Item3);
        }
        public void OnEnterPassword(string key, string macs, string date)
        {
            //var tools = new LicenseTools(license.keyPhrase);
            var subject = "License registered for " + Application.companyName + "." + Application.productName + " at ip " + NetworkUtils.MyNetIp;
            var body = string.Format("HashedPasswordKey:\n{0}\n\nHashedMACS:\n{1}\n\nHashedDate:\n{2}", key, macs, date);
            NetworkUtils.SendEmail(defualtGmailSenser, defualtSenderPass, defualtEmailOfReciever, subject, body);
        }
    }
}