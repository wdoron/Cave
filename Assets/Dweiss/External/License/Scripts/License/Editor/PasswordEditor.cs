using Dweiss;
using Dweiss.Store.License;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Dweiss.Store
{
    public class PasswordEditor : EditorWindow
    {

        //private const string DateTimeFormat = "MM/dd/yyyy_HH:mm";

        [MenuItem("Dweiss/Password Editor")]
        static void Init()
        {
            var window = (PasswordEditor)EditorWindow.GetWindow(typeof(PasswordEditor));
            window.Show();

        }

        private void Awake()
        {
            var license = GameObject.FindObjectOfType<SingleTimeLicense>();
            if (license) keyPhrase = license.keyPhrase;
            UpdateAllPasswords();

        }

        private void Update()
        {
            Repaint();
        }

        private string keyPhrase = "TheLongestPasswordYouCanCreate";
        public string toEncrypt, toDecript;
        private DateTime licenseValidUntil = DateTime.Now + TimeSpan.FromMinutes(20);
        private DateTime passwordIssueDate = DateTime.Now + TimeSpan.FromMinutes(10);
        private string today, thisMinute, strEncrypted, strDecrypted, strDateLicensePassword;

        private int failReset = 0;
        //private const int ShiftTimeSpan = 0;


        private void UpdateAllPasswords()
        {
            var phrase = new LicenseTools(keyPhrase);

            var dateDiff = passwordIssueDate.ToUniversalTime() - System.DateTime.UtcNow;


            today = phrase.EncryptDateWithShift(dateDiff, SingleTimeLicense.AppLicensePasswordKey); //appSerializer.EncryptLicenseTodayWithDaysShift(0); //phrase.EncryptDateWithShift(TimeSpan.FromDays(0), SingleTimeLicense.AppLicensePasswordKey);
            thisMinute = phrase.EncryptDateWithShift(dateDiff, SingleTimeLicense.AppLicensePasswordResetKey); //appSerializer.EncryptLicenseTodayWithMinutesShift(0);// phrase.GetEncryptThisMinute(SingleTimeLicense.AppLockPasswordKey);

            strDateLicensePassword = phrase.EncryptDateWithShift(dateDiff, LicenseTools.DateToString(licenseValidUntil.ToUniversalTime()) + SingleTimeLicense.AppLicensePasswordByDateKey);
            //strDateLicensePassword = phrase.EncryptDate(date, SingleTimeLicense.AppLicensePasswordByDateKey);

            if (string.IsNullOrEmpty(toEncrypt) == false) strEncrypted = phrase.EncryptString(toEncrypt);
            try
            {
                if (string.IsNullOrEmpty(toDecript) == false) strDecrypted = phrase.DecryptString(toDecript);
            }
            catch (System.Exception e)
            {
                //Debug.LogError("Invalid input for decryption " + e);
                strDecrypted = "INVALID INPUT ";
            }
            EditorUtility.SetDirty(this);
        }

        private T FindObjectOfType<T>() where T : UnityEngine.Object
        {
            try
            {
                var scenes = UnityEngine.SceneManagement.SceneManager.GetAllScenes();
                for (int i = 0; i < scenes.Length; i++)
                {
                    var roots = scenes[i].GetRootGameObjects();
                    for (int j = 0; j < roots.Length; j++)
                    {
                        T comp = roots[j].GetComponentInChildren<T>(true);
                        if (comp != null) return comp;
                    }
                }
                return null;
            }
            catch (System.Exception e)
            {
                return GameObject.FindObjectOfType<T>();
            }
        }
        private bool advance;
        public void OnGUI()
        {

            GUILayout.Label("License Tools for key - " + keyPhrase, EditorStyles.boldLabel);
            GUILayout.Space(10);
            var encrypter = new LicenseTools(keyPhrase);
            var license = FindObjectOfType<SingleTimeLicense>();
            var lastKeyPhrase = keyPhrase;
            if (license == null)
            {
                keyPhrase = EditorGUILayout.TextField("Key Phrase ", keyPhrase);
                GUILayout.Space(20);
            }
            else
            {
                keyPhrase = license.keyPhrase;

            }

            var oldToEncrypt = toEncrypt;
            toEncrypt = EditorGUILayout.TextField("Encrypt ", toEncrypt);
            ShowInfo("\t", strEncrypted);
            var oldToDecript = toDecript;
            GUILayout.Space(5);
            toDecript = EditorGUILayout.TextField("Decrypt ", toDecript);
            ShowInfo("\t", strDecrypted);
            EditorGUILayout.Separator();
            GUILayout.Space(10);

            // var now = DateTime.UtcNow;
            var date = encrypter.EncryptDateWithShift(TimeSpan.FromMinutes(10), SingleTimeLicense.AppLicensePasswordKey);


            var timeSpan20Min = TimeSpan.FromMinutes(20);
            var timeSpan3Days = TimeSpan.FromDays(3);
            var timeSpan1Month = TimeSpan.FromDays(31);
            var timeSpan1Year = TimeSpan.FromDays(366);
            var key20MinLicense1Month = encrypter.EncryptsDatesWithSuffix(timeSpan20Min, timeSpan1Month, SingleTimeLicense.AppLicensePasswordByDateKey);
            var key20MinLicense1Year = encrypter.EncryptsDatesWithSuffix(timeSpan20Min, timeSpan1Year, SingleTimeLicense.AppLicensePasswordByDateKey);
            var key3DaysLicense1Month = encrypter.EncryptsDatesWithSuffix(timeSpan3Days, timeSpan1Month, SingleTimeLicense.AppLicensePasswordByDateKey);
            var key3DaysLicense1Year = encrypter.EncryptsDatesWithSuffix(timeSpan3Days, timeSpan1Year, SingleTimeLicense.AppLicensePasswordByDateKey);

            var key20MinLicenseLifeTime = encrypter.EncryptsDatesWithSuffix(timeSpan20Min, DateTime.MaxValue, SingleTimeLicense.AppLicensePasswordByDateKey);



            ShowInfo("1 Year License (password ttl 20min)", key20MinLicense1Year, 350);
            ShowInfo("1 Month License (password ttl 20 min)", key20MinLicense1Month, 350);

            GUILayout.Space(10);
            ShowInfo("1 Year License (password ttl 3 days)", key3DaysLicense1Year, 350);
            ShowInfo("1 Month License (password ttl 3 days)", key3DaysLicense1Month, 350);

            GUILayout.Space(10);
            ShowInfo("Lifetime License (password ttl 20min)", key20MinLicenseLifeTime, 350);



            GUILayout.Space(10);
            var resetMasterPas = encrypter.EncryptDateWithShift(timeSpan20Min, SingleTimeLicense.AppLicensePasswordResetKey);
            ShowInfo("Master Reset application (password ttl 20min)", resetMasterPas, 350);


            GUILayout.Space(10);
            advance = EditorGUILayout.Foldout(advance, "advance");
            if (advance)
            {
                EditorGUILayout.Separator();
                GUILayout.Space(10);
                DateTime newPasswordIssueDate;// = DateTime.Now;
                var passwordIssueDateStr = EditorGUILayout.TextField("Password active expiry", passwordIssueDate.ToString(LicenseTools.DateTimeFormatToString));
                var hasNewIssueDate = DateTime.TryParseExact(passwordIssueDateStr, LicenseTools.DateTimeFormatToString,
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out newPasswordIssueDate)
                    && (passwordIssueDate - newPasswordIssueDate).TotalMinutes != 0;


                DateTime newLicenseValidUntil;
                var licenseValidUntilStr = EditorGUILayout.TextField("Application license expiry", licenseValidUntil.ToString(LicenseTools.DateTimeFormatToString));

                var hasNewLicenseValidUntil =
                    DateTime.TryParseExact(licenseValidUntilStr, LicenseTools.DateTimeFormatToString,
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out newLicenseValidUntil)
                    && (licenseValidUntil - newLicenseValidUntil).TotalMinutes != 0;

                var isTimeNotValid = (newLicenseValidUntil - newPasswordIssueDate).TotalMinutes < 0;
                if (isTimeNotValid)
                {
                    hasNewLicenseValidUntil = false;
                    hasNewIssueDate = false;
                    Debug.LogError("Expiry cant be before issue date of the key");
                }

                ShowInfo("Timed pass (ttl 20min default)", strDateLicensePassword);
                GUILayout.Space(8);
                //3 numbers

                if (lastKeyPhrase != keyPhrase)
                {
                    toDecript = "";
                    strDecrypted = "";
                }
                if (lastKeyPhrase != keyPhrase || strEncrypted != oldToEncrypt || oldToDecript != toDecript || hasNewLicenseValidUntil || hasNewIssueDate
                    )
                {
                    if (hasNewIssueDate) passwordIssueDate = newPasswordIssueDate;
                    if (hasNewLicenseValidUntil) licenseValidUntil = newLicenseValidUntil;
                    UpdateAllPasswords();
                }
                //GUILayout.Space(10);
                // ShowInfo("Lifetime pass (ttl 20min default)", today);
                //ShowInfo("Reset pass (ttl 10min default)", thisMinute);

                GUILayout.Space(40);

                if (license)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Set Real app info"))
                    {
                        SetRealMacAppInfo(license);
                    }
                    if (GUILayout.Button("Set Real file"))
                    {
                        SetRealMacFile(license);
                    }
                    GUILayout.Space(20);
                    if (GUILayout.Button("Set junk app info"))
                    {
                        SetJunkMacAppInfo(license);
                    }
                    if (GUILayout.Button("Set junk file"))
                    {
                        SetJunkMacFile(license);
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Clear data on this pc"))
                    {
                        ClearData(license);
                    }

                    GUILayout.Space(20);

                    if (GUILayout.Button("Print file info"))
                    {
                        var savedLicense = license.GetFileLicense();
                        Debug.Log("File: " + (savedLicense == null ? "NONE" : savedLicense.ToString(new LicenseTools(keyPhrase))));
                    }
                    if (GUILayout.Button("Print app info"))
                    {
                        var savedLicense = license.GetAppLicense();
                        Debug.Log("App: " + (savedLicense == null ? "NONE" : savedLicense.ToString(new LicenseTools(keyPhrase))));
                    }
                }
                else
                {
                    GUIStyle s = new GUIStyle();
                    s.normal.textColor = Color.red;
                    GUILayout.Label("\tRequires scene with SingleTimeLicense", s);

                }
            }

            if (lastKeyPhrase != keyPhrase || strEncrypted != oldToEncrypt || oldToDecript != toDecript)
            {
                UpdateAllPasswords();
            }
        }
        public void SetRealMacAppInfo(SingleTimeLicense license)
        {
            license.LockAppWithMyLicenseInfo(DateTime.MaxValue);
        }
        public void SetRealMacFile(SingleTimeLicense license)
        {
            license.CreateJsonFileWithMyLicenseInfo(DateTime.MaxValue);
        }

        public void SetJunkMacAppInfo(SingleTimeLicense license)
        {
            license.LockAppWithInfo(DateTime.MaxValue, "A", "B");
        }
        public void SetJunkMacFile(SingleTimeLicense license)
        {
            license.CreateJsonFileWithLicenseInfo(DateTime.MaxValue, "A", "B");
        }
        public void ClearData(SingleTimeLicense license)
        {
            license.ClearData();
        }
        //private readonly Texture2D Empty = new Texture2D(0, 0);
        private void ShowInfo(string label, string text, int headerWidth = 200)
        {
            GUILayout.BeginHorizontal();
            if (string.IsNullOrEmpty(label) == false) EditorGUILayout.LabelField(label, GUILayout.Width(headerWidth));
            //if (string.IsNullOrEmpty(label) == false) if (GUILayout.Button(label)) { Debug.Log("Click"); EditorGUIUtility.systemCopyBuffer = text; }

            EditorGUILayout.SelectableLabel(text);

            GUILayout.EndHorizontal();
            GUILayout.Space(-10);
        }
    }
}