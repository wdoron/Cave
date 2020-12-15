using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Store.License
{
    [ExecuteInEditMode]
    public class SingleTimeLicense : MonoBehaviour
    {
        [System.Serializable]
        public class EventEmpty : UnityEngine.Events.UnityEvent { }
        [System.Serializable]
        public class EventFloat : UnityEngine.Events.UnityEvent<float> { }
        [System.Serializable]
        public class OnRegisterInfoEvent : UnityEngine.Events.UnityEvent<Tuple<string, string, string>> { };

        public bool debug = true;

        private const float DaysLeftThreshhold = 365;
        private const char Seperator = '\n';


        [Header("PasswordKeyConfig")]
        [Tooltip("The phrase to create a new password system")]

        public string keyPhrase = "TheLongestPasswordYouCanCreate";

        public const string AppLicensePasswordKey = "Ps";
        public const string AppLicensePasswordByDateKey = "PT";
        public const string AppLicensePasswordResetKey = "Rs";

        public const string AppInternalLockId = "AppPss";//Internal key save
                                                         //public const string AppInternalLockIdTime = "PssT";//Internal key save

        // public const char DateSeperator = '';

        private static string FolderPath { get { return Application.persistentDataPath /*+ "/License/"*/; } }
        private const string JsonFileName = "info.sys";

        [Header("_Password Time_")]
        //public int passwordActiveDays = 0;
        //public int passwordActiveHours = 0;
        //public int passwordActiveMinutes = 20;
        //public int masterResetPasswordActiveMinutes = 10;

        [Header("_Events_")]
        public EventEmpty onPasswordInvalid;
        public EventEmpty onPasswordCorrect, onAppLock, onResetPassword;
        public EventFloat onDaysLeft;
        public OnRegisterInfoEvent onEnterLicenseInfo;


        private LicenseTools _pcTools;
        private LicenseTools PcTools
        {
            get
            {
                if (_pcTools == null || string.IsNullOrEmpty(_pcTools.KeyPhrase))
                {
                    _pcTools = new LicenseTools(keyPhrase);
                }
                return _pcTools;
            }
        }


        [System.Serializable]
        public class LicenseInfo
        {
            public string[] hashes;


            public DateTime GetDate(LicenseTools decryptor)
            {
                return string.IsNullOrEmpty(dateStrHashed) ? DateTime.MaxValue : decryptor.DecryptDate(dateStrHashed);
            }
            public void SetDate(DateTime date, LicenseTools encryptor)
            {
                dateStrHashed = encryptor.EncryptDate(date, "");
            }
            public string dateStrHashed;
            public LicenseInfo()
            {
                hashes = new string[0];
            }
            public override string ToString()
            {
                return string.Format("Hashes-{0} Time:({1})", hashes == null ? "NULL" : string.Join(",", hashes), dateStrHashed);
            }
            public string ToString(LicenseTools decryptor)
            {
                string macs = hashes == null ? "NONE" : string.Join(",", decryptor.GetMacAddresses(hashes));
                DateTime date = string.IsNullOrEmpty(dateStrHashed) ? DateTime.MinValue : decryptor.DecryptDate(dateStrHashed);
                return string.Format("Hashes-{0} Time:({1})", macs, date);
            }
        }

        private void Awake()
        {
            onPasswordInvalid.AddListener(() => { Debug.Log("Password invalid"); });
            onPasswordCorrect.AddListener(() => { Debug.Log("Password correct"); });

            var setupPcTools = PcTools;
        }
        private void Start()
        {
            Init();
        }
        private bool IsPasswordAndDateValid()
        {
            var licence = GetAppLicense();
            if (debug) Debug.Log("IsPasswordAndDateValid-License " + licence);
            return licence != null && NetworkUtils.StartTimeUtc <= licence.GetDate(PcTools) && IsValidMacAddress(licence);
        }
        private void PasswordCorrectCheckDate()
        {
            var isValid = IsPasswordAndDateValid();
            if (isValid == false)
            {
                ClearData();
            }
            else
            {
                var licence = GetAppLicense();

                var timeLeft = (licence.GetDate(PcTools) - NetworkUtils.StartTimeUtc);
                if (timeLeft.TotalDays < DaysLeftThreshhold)
                    onDaysLeft.Invoke((float)timeLeft.TotalDays);

                onPasswordCorrect.Invoke();
            }
        }


        private void Init()
        {

            var isMacAppCorrect = IsMacAppDataCorrect();
            var isMacFileCorrect = IsMacFileCorrect();
            var isAppLock = IsAppLockedToADifferentPc();

            if (isAppLock && isMacAppCorrect == false)
            {
                if (debug) Debug.Log("App locked to a different computer");
                onAppLock.Invoke();
            }
            else if (isMacAppCorrect && isMacFileCorrect)
            {
                if (debug) Debug.Log("App V file V");
                PasswordCorrectCheckDate();//onPasswordCorrect.Invoke();
            }
            else if (isMacAppCorrect)
            {
                if (debug) Debug.Log("App V file X");

                SaveMacAddressToAppAndToFile(GetAppLicense().GetDate(PcTools));
                PasswordCorrectCheckDate();//onPasswordCorrect.Invoke();
            }
            else if (isMacFileCorrect)
            {
                if (debug) Debug.Log("App X file V");
                SaveMacAddressToAppAndToFile(GetValidFileLicense().GetDate(PcTools));
                PasswordCorrectCheckDate();//onPasswordCorrect.Invoke();
            }
            else
            {
                if (debug) Debug.Log("App X file X");
            }
            //LicenseInfo license;

        }
        private bool IsMacAppDataCorrect()
        {
            var license = GetAppLicense();
            return IsValidMacAddress(license);
        }
        public LicenseInfo GetFileLicense()
        {
            LicenseInfo license;
            var myFile = new JsonSettingsFile();
            if (myFile.Exists(JsonFileName, FolderPath))
            {
                myFile.Read(JsonFileName, FolderPath, out license);
                return license;
            }
            return null;
        }

        public LicenseInfo GetValidFileLicense()
        {
            LicenseInfo license = GetFileLicense();
            if (IsValidMacAddress(license))
            {
                return license;
            }
            return null;
        }
        private bool IsMacFileCorrect()
        {
            return GetValidFileLicense() != null;
        }


        public LicenseInfo GetAppLicense()
        {
            try
            {
                var json = PlayerPrefs.GetString(AppInternalLockId, "");
                if (string.IsNullOrEmpty(json)) return null;
                var ret = JsonUtility.FromJson<LicenseInfo>(json);
                return ret;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Read license from internal app failed " + e);
                ClearAppData();
                return null;
            }
        }
        private bool IsAppLockedToADifferentPc()
        {
            var license = GetAppLicense();
            if (license == null) return false;
            return IsValidMacAddress(license.hashes) == false;
        }

        public LicenseInfo GetValidAppLicense()
        {
            var license = GetAppLicense();
            if (IsValidMacAddress(license))
            {
                return license;
            }
            return null;
        }


        private void RaiseLoginInfo(string hashKey)
        {
            var license = GetValidAppLicense();

            onEnterLicenseInfo.Invoke(Tuple.Create(hashKey, string.Join("\t", license.hashes), license.dateStrHashed));
        }

        public void EnterPassword(string hash)
        {
            try
            {

                var isTruePassWord = PcTools.IsTimeStillValidFromEncodedString(hash, PcTools.EncryptString(AppLicensePasswordKey), -1);//Save only encrypted suffix?
                var isResetPasword = PcTools.IsTimeStillValidFromEncodedString(hash, PcTools.EncryptString(AppLicensePasswordResetKey), -1);
                var timeStr = PcTools.GetValidStringAfterTimeValidateionFromEncodedString(hash, PcTools.EncryptString(AppLicensePasswordByDateKey), LicenseTools.DateTimeFormat.Length);
                // var isTruePassWord = PcTools.IsPhrazeFromPastDays(passwordActiveDays, passwordActiveHours, passwordActiveMinutes, hash, PcTools.EncryptString(AppLicensePasswordKey));//Save only encrypted suffix?
                //var isResetPasword = PcTools.IsPhrazeFromPastMinutes(masterResetPasswordActiveMinutes, hash, PcTools.EncryptString(AppLicensePasswordResetKey));
                //var timeStr = PcTools.GetPhrazeFromPastDays(passwordActiveDays, passwordActiveHours, passwordActiveMinutes, hash, PcTools.EncryptString(AppLicensePasswordByDateKey), DateSeperator);
                var isTimedLicensePassword = timeStr != null;

                var needReset = IsAppLockedToADifferentPc();

                if (needReset == false)
                {
                    if (isTruePassWord)
                    {
                        SaveMacAddressToAppAndToFile(DateTime.MaxValue);
                        RaiseLoginInfo(hash);
                        onPasswordCorrect.Invoke();

                        if (debug) Debug.Log("Correct password");
                    }
                    else if (isTimedLicensePassword)
                    {
                        var t = LicenseTools.DateFromString(timeStr);

                        SaveMacAddressToAppAndToFile(t);
                        RaiseLoginInfo(hash);
                        if (IsPasswordAndDateValid())
                        {
                            onPasswordCorrect.Invoke();
                            if (debug) Debug.Log("Correct Time password until " + LicenseTools.DateToString(t));
                        }
                        else
                        {
                            onPasswordInvalid.Invoke();
                            if (debug) Debug.Log("Incorrect password");
                        }

                    }
                    else
                    {
                        onPasswordInvalid.Invoke();
                        if (debug) Debug.Log("Incorrect password");
                    }
                }
                else
                {
                    if (isResetPasword)
                    {
                        ClearData();
                        //LockAppWithInfo(DateTime.MaxValue, null);
                        onResetPassword.Invoke();
                        if (debug) Debug.Log("ResetPassword successful");

                    }
                }

            }
            catch (System.Exception e) { }
        }

        private void ClearFileData()
        {
            try
            {
                var myFile = new JsonSettingsFile();
                if (myFile.Exists(JsonFileName, FolderPath))
                {
                    System.IO.File.Delete(myFile.GetFullFileName(JsonFileName, FolderPath));
                }
                if (debug) Debug.Log("ClearFileData finished");
            }
            catch (System.Exception e) { Debug.LogError("Problem deleting file license data" + e); };
        }
        private void ClearAppData()
        {

            try
            {
                LockAppWithInfo(DateTime.MaxValue, null);
                if (debug) Debug.Log("ClearAppData finished");
            }
            catch (System.Exception e) { Debug.LogError("Problem deleting internal app license data " + e); };

        }
        [ContextMenu("Clear Data")]
        public void ClearData()
        {
            ClearFileData();
            ClearAppData();
            if (debug) Debug.Log("Clear data finished");
        }

        private void SaveMacAddressToAppAndToFile(DateTime lastDate)
        {
            CreateJsonFileWithMyLicenseInfo(lastDate);
            LockAppWithMyLicenseInfo(lastDate);
        }
        public void CreateJsonFileWithMyLicenseInfo(DateTime lastDate)
        {
            CreateJsonFileWithLicenseInfo(lastDate, PcTools.GetEncryptedMacAddress());
        }

        public void CreateJsonFileWithLicenseInfo(DateTime lastDate, params string[] macAddresses)
        {
            try
            {
                var newLicense = new LicenseInfo() { hashes = macAddresses };
                newLicense.SetDate(lastDate, PcTools);
                var myFile = new JsonSettingsFile();
                myFile.Write(JsonFileName, FolderPath, newLicense);

                //#if UNITY_ANDROID == false
                //            var f = new System.IO.FileInfo(FolderPath + JsonFileName);
                //            f.Attributes = f.Attributes | System.IO.FileAttributes.Hidden;

                //            var d = new System.IO.DirectoryInfo(FolderPath);
                //            d.Attributes = d.Attributes | System.IO.FileAttributes.Hidden;
                //#endif
                if (debug) Debug.Log("Create json file with info " + JsonUtility.ToJson(newLicense) + "\nvs\n" + newLicense);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Problem with saving license file " + e);
            }
        }

        public void LockAppWithMyLicenseInfo(DateTime lastDate)
        {
            LockAppWithInfo(lastDate, PcTools.GetEncryptedMacAddress());
        }
        public void LockAppWithInfo(DateTime lastDate, params string[] macAddresses)
        {
            var newLicense = new LicenseInfo() { hashes = macAddresses };
            newLicense.SetDate(lastDate, PcTools);
            var str = macAddresses == null ? "" : JsonUtility.ToJson(newLicense);
            PlayerPrefs.SetString(AppInternalLockId, str);
            if (debug) Debug.Log("Lock app with info " + str + "\nvs\n" + newLicense);
        }
        private bool IsValidMacAddress(LicenseInfo license)
        {
            return license != null && IsValidMacAddress(license.hashes);
        }
        private bool IsValidMacAddress(string[] licenseArr)
        {
            return licenseArr != null && PcTools.IsMacValid(licenseArr);
        }

        public string[] GetLicenses(LicenseInfo license)
        {
            if (license == null || license.hashes == null) return null;

            return PcTools.GetMacAddresses(license.hashes);
        }


    }
}