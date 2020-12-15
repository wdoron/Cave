using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Store.License
{
    public class AppSerializer : MonoBehaviour
    {

        [Header("PasswordKeyConfig")]

        [Tooltip("The phrase to create a new password system")]
        public string keyPhrase = "TheLongestPasswordYouCanCreate";

        private void Reset()
        {
            var stl = GameObject.FindObjectOfType<SingleTimeLicense>();
            if (stl)
            {
                keyPhrase = stl.keyPhrase;
            }
        }

        private LicenseTools tools;
        private void Awake()
        {
            Reset();
            ResetKeyPhrase(keyPhrase);
        }
        public void ResetKeyPhrase(string keyPhrase)
        {
            this.keyPhrase = keyPhrase;
            tools = new LicenseTools(keyPhrase);
        }

        public string Encrypt(string txt)
        {
            return tools.EncryptString(txt);
        }

        public string Decrypt(string txt)
        {
            return tools.DecryptString(txt);
        }

        public LicenseTools Tools { get { return tools; } }
        
    }
}