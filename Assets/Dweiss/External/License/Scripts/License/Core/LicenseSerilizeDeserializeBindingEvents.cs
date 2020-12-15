using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Dweiss.Store.License
{
    public class LicenseSerilizeDeserializeBindingEvents : MonoBehaviour
    {
        public AppSerializer appSerializer;


        public float waitBetweenUpdateKeys = 10;
        

        public StringEvent  onTextSerailize, onTextDeserialize, onTimedLicenseDaySerialize, onTimedLicenseDaySerialize2, onTimedLicenseDaySerialize3, 
            onMasterResetSerialize,  onErr, onKeyPhraseChanged;

        public float delayDeseriazlie = 1;
        private float startDeseriazlieTime;
        private string deserializeStr;
        private Coroutine deserializeCoroutine;


        private void Awake()
        {
            
            var keyPhrase = PlayerPrefs.GetString("keyPhrase", appSerializer.keyPhrase);
            SetKeyPhraze(keyPhrase);
        }
        private Coroutine showPass;
        private void OnDisable()
        {
            StopAllCoroutines();
            showPass = null;
        }
        private void OnEnable()
        {
            StopAllCoroutines();
            showPass = StartCoroutine(ShowPass());
        }

        IEnumerator ShowPass()
        {
            yield return 0;
            while (true)
            {
                try
                {
                    UpdateEncryptionKeys();
                }
                catch (System.Exception)
                {
                    onErr.Invoke("Key generator error");
                };
                yield return new WaitForSecondsRealtime(waitBetweenUpdateKeys);
            }
        }


        public void SetKeyPhraze(string keyPhrase)
        {
            appSerializer.ResetKeyPhrase(keyPhrase);

            onTimedLicenseDaySerialize.Invoke("");
            onTimedLicenseDaySerialize2.Invoke("");
            onTimedLicenseDaySerialize3.Invoke("");
            onMasterResetSerialize.Invoke("");

            onTextSerailize.Invoke("");
            onTextDeserialize.Invoke("");
            onErr.Invoke("");


#if UNITY_EDITOR
            PlayerPrefs.DeleteKey("keyPhrase");
#else
            PlayerPrefs.SetString("keyPhrase", keyPhrase);
#endif
            onKeyPhraseChanged.Invoke(keyPhrase);

            if(showPass != null) StopCoroutine(showPass);
            showPass = StartCoroutine(ShowPass());
        }

        public void Serializer(string txt)
        {
            onTextSerailize.Invoke(appSerializer.Encrypt(txt));
        }

        public void Deserailize(string txt)
        {
            onTextDeserialize.Invoke("");
            startDeseriazlieTime = Time.time;
            
            deserializeStr = txt;
            if (deserializeCoroutine == null)
            {
                deserializeCoroutine = StartCoroutine(ShowDeserialize());
            }
        }

        IEnumerator ShowDeserialize() {
            
            while (Time.time < startDeseriazlieTime + delayDeseriazlie) yield return 0;
            //yield return new WaitForSecondsRealtime(delayDeseriazlie);
            try
            {
                if (string.IsNullOrEmpty(deserializeStr) == false)
                {
                    var decrypted = appSerializer.Decrypt(deserializeStr);
                    Debug.LogFormat("ShowDeserialize {0} -> {1}", deserializeStr , decrypted);
                    onTextDeserialize.Invoke(decrypted);
                }
            }
            catch(System.Exception e )
            {
                onErr.Invoke("Error deserilize");
                Debug.LogError("Error deserilize " + e);
            }
            deserializeCoroutine = null;
        }
        public void UpdateEncryptionKeys()
        {
            var timeSpan20Min = TimeSpan.FromMinutes(20);
            var timeSpan3Days = TimeSpan.FromDays(3);
            var timeSpan1Month = TimeSpan.FromDays(31);
            var timeSpan1Year = TimeSpan.FromDays(366);

            var key20MinLicense1Month = appSerializer.Tools.EncryptsDatesWithSuffix(timeSpan20Min, timeSpan1Month, SingleTimeLicense.AppLicensePasswordByDateKey);
            onTimedLicenseDaySerialize.Invoke(key20MinLicense1Month);
            var key20MinLicense1Year = appSerializer.Tools.EncryptsDatesWithSuffix(timeSpan20Min, timeSpan1Year, SingleTimeLicense.AppLicensePasswordByDateKey);
            onTimedLicenseDaySerialize2.Invoke(key20MinLicense1Year);
            var key20MinLicenseLifeTime = appSerializer.Tools.EncryptsDatesWithSuffix(timeSpan20Min, DateTime.MaxValue, SingleTimeLicense.AppLicensePasswordByDateKey);
            onTimedLicenseDaySerialize3.Invoke(key20MinLicense1Year);


            var resetMasterPas = appSerializer.Tools.EncryptDateWithShift(timeSpan20Min, SingleTimeLicense.AppLicensePasswordResetKey);
            onMasterResetSerialize.Invoke(resetMasterPas); 
        }
       
        [System.Serializable]
        public class StringEvent : UnityEngine.Events.UnityEvent<string> { }
    }
}