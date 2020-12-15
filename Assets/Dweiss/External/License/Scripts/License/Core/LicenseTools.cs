using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Text;
using System.IO;
using System.Net.NetworkInformation;
using System.Linq;
using System.Globalization;

namespace Dweiss.Store.License
{
    [System.Serializable]
    public class LicenseTools
    {
        public const string DateTimeFormatToString = "MM/dd/yyyy_HH:mm";
        public const string DateTimeFormat = "MMddyyyyHHmm";

        private string keyPhrase;

        public string KeyPhrase { get { return keyPhrase; } }
        public LicenseTools(string keyPhrase)
        {
            this.keyPhrase = keyPhrase;
            if (string.IsNullOrEmpty(keyPhrase)) Debug.LogError("keyphrase empty1");
        }

        public string EncryptString(string str)
        {
            return BasicEncryptDecrypt.EncryptString(str, keyPhrase);
        }
        public string DecryptString(string str)
        {
            return BasicEncryptDecrypt.DecryptString(str, keyPhrase);
        }


        #region TimeEncryption

        public static DateTime NowTimeNormazlied()
        {
            return NowTimeNormazlied(TimeSpan.FromSeconds(0));
        }

        public static DateTime NowTimeNormazlied(TimeSpan shift)
        {
            var res = NetworkUtils.StartTimeUtc + shift;
            return res;
        }
       

        public DateTime DecryptDate(string dateStrHashed)
        {
            var dateStr = DecryptString(dateStrHashed);
            return DateFromString(dateStr);
            //DateFromString
            //return EncryptString(d.ToString(DateTimeFormat) + (string.IsNullOrEmpty(suffix) ? "" : suffix));
        }
        public string EncryptDate(DateTime d, string suffix)
        {
            
            var str = d.ToString(DateTimeFormat) + (suffix == null ? "" : suffix);
            var ret = EncryptString(str);
            //Debug.LogFormat("Set date <{0}> suffix <{1}> str <{2}> result <{3}> \nkey <{4}>", d.ToString(DateTimeFormat), suffix, str , ret, keyPhrase);
            return ret;
        }

        public string EncryptsDatesWithSuffix(DateTime utc1, DateTime utc2, string suffix)
        {
            var str = utc1.ToString(DateTimeFormat) + utc2.ToString(DateTimeFormat) + suffix;
            var ret = EncryptString(str);
            //Debug.LogFormat("Set date <{0}> suffix <{1}> str <{2}> result <{3}> \nkey <{4}>", d.ToString(DateTimeFormat), suffix, str , ret, keyPhrase);
            return ret;

        }
        public string EncryptsDatesWithSuffix(TimeSpan sp1, TimeSpan sp2, string suffix)
        {
            return EncryptsDatesWithSuffix(NowTimeNormazlied(sp1), NowTimeNormazlied(sp2), suffix);

        }
        public string EncryptsDatesWithSuffix(TimeSpan sp1, DateTime utc2, string suffix)
        {
            return EncryptsDatesWithSuffix(NowTimeNormazlied(sp1), utc2, suffix);

        }
        public string EncryptDateWithShift(TimeSpan shift, string suffix = null)
        {
            return EncryptDate(NowTimeNormazlied(shift), suffix);
        }
        public string GetEncryptToday(string suffix = null)
        {
            return EncryptDate(NowTimeNormazlied(), suffix);
        }

        public string GetEncryptThisMinute(string suffix = null)
        {
            return EncryptDate(NowTimeNormazlied(), suffix);
        }

        private static DateTime DateFromString(string str, string format)
        {
            return DateTime.ParseExact(str, format, CultureInfo.InvariantCulture);
        }
        public static DateTime DateFromString(string dateStr)
        {
            return DateFromString(dateStr, DateTimeFormat);
        }
        public static string DateToString(DateTime time)
        {
            return time.ToString(DateTimeFormat);
        }


        public bool IsTimedLicense(string hash, string suffix)
        {
            return GetTimedLicense(hash, suffix).HasValue;
        }
        public System.DateTime? GetTimedLicense(string hash, string suffix)
        {
            var decryptedStr = DecryptString(hash);
            string dateStr = "";

            if (decryptedStr.EndsWith(suffix))
            {
                dateStr = decryptedStr.Substring(0, decryptedStr.LastIndexOf(suffix));
                try
                {
                    var date = DateFromString(dateStr);
                    Debug.LogFormat("Timed license {0} -> {1} -> {2} ({3})", hash, decryptedStr, dateStr, suffix);
                    return date;
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Invalid date format of " + dateStr + " " + e);
                    Debug.LogFormat("Timed license {0} -> {1} -> {2} ({3})", hash, decryptedStr, dateStr, suffix);
                    return null;
                }
            }
            Debug.LogFormat("Timed license {0} -> {1} -> {2} ({3})", hash, decryptedStr, dateStr, suffix);
            return null;
        }


        public bool IsTimeStillValidFromEncodedString(string hash, string suffixHashed, int indexOfSuffix)
        {
            var dAndSt = GetTimeAndStringFromEncodedString(hash, suffixHashed, indexOfSuffix);
            if(dAndSt != null)
            {
                var timeDiff = (dAndSt.Item1 - NowTimeNormazlied());
                return  timeDiff.TotalMinutes >= 0;
            }
            return false;
        }
        public String GetValidStringAfterTimeValidateionFromEncodedString(string hash, string suffixHashed, int indexOfSuffix)
        {
            var dAndSt = GetTimeAndStringFromEncodedString(hash, suffixHashed, indexOfSuffix);
            if (dAndSt != null)
            {
                var timeDiff = (dAndSt.Item1 - NowTimeNormazlied());
                return timeDiff.TotalMinutes >= 0 ? dAndSt.Item2 : null;
            }
            return null;
        }
        public System.Tuple<DateTime, string> GetTimeAndStringFromEncodedString(string hash, string suffixHashed, int indexOfSuffix)
        {
            string dateStr;
            DateTime toCmpr;
            string suffixStr = null;
            try
            {
                dateStr = DecryptString(hash);
                if (string.IsNullOrEmpty(suffixHashed) == false)
                {
                    var suffix = DecryptString(suffixHashed);
                    dateStr = dateStr.Replace(suffix, "");
                    if (indexOfSuffix> 0 )
                    {
                        suffixStr = dateStr.Substring(indexOfSuffix);
                        dateStr = dateStr.Substring(0, indexOfSuffix);
                    }
                    else
                    {
                        suffixStr = "";
                    }
                }
                toCmpr = DateFromString(dateStr);
                return new System.Tuple<DateTime, string>(toCmpr, suffixStr);
            }
            catch (System.Exception e)
            {
                //Debug.Log("Invalid date format of " + hash + " " + e);
                return null;
            }
        }
        public System.Tuple<DateTime, string> GetTimeAndStringFromEncodedString(string hash, string suffixHashed, char? encodedStrSeperator)
        {
            string dateStr;
            DateTime toCmpr;
            string suffixStr = null;
            try
            {
                dateStr = DecryptString(hash);
                if (string.IsNullOrEmpty(suffixHashed) == false)
                {
                    var suffix = DecryptString(suffixHashed);
                    dateStr = dateStr.Replace(suffix, "");
                    if (encodedStrSeperator.HasValue)
                    {
                        var splited = dateStr.Split(encodedStrSeperator.Value);
                        dateStr = splited[0];
                        suffixStr = splited[1];
                    }
                    else
                    {
                        suffixStr = "";
                    }
                }
                toCmpr = DateFromString(dateStr); 
                return new System.Tuple<DateTime, string>(toCmpr, suffixStr);
            }
            catch (System.Exception e)
            {
                //Debug.Log("Invalid date format of " + hash + " " + e);
                return null;
            }
        }


        public bool IsPhrazeFromPastDays(int days, int hours, int minutes , string hash, string suffixHashed, char? encodedStrSeperator = null)
        {
            var res = GetPhrazeFromPastDays(days, hours, minutes, hash, suffixHashed, encodedStrSeperator);
            return res != null;
        }
        public bool IsPhrazeFromPastMinutes(int minutes, string hash, string suffixHashed, char? encodedStrSeperator = null)
        {
            var res = GetPhrazeFromPastMinutes(minutes, hash, suffixHashed, encodedStrSeperator);
            return res != null;
        }
        public string GetPhrazeFromPastDays(int days, int hours, int minutes, string hash, string suffixHashed, char? encodedStrSeperator = null)
        {
            
            var toCmprTuple = GetTimeAndStringFromEncodedString(hash, suffixHashed, encodedStrSeperator);
            if (toCmprTuple == null) return null;
            var now = NowTimeNormazlied();
            var timeSpan = now - toCmprTuple.Item1;
            return System.Math.Abs(timeSpan.TotalMinutes) < ((days*24+hours)*60+minutes) ? toCmprTuple.Item2 : null;
        }
        public string GetPhrazeFromPastMinutes(int minutes, string hash, string suffixHashed, char? encodedStrSeperator = null)
        {
            var toCmprTuple = GetTimeAndStringFromEncodedString(hash, suffixHashed, encodedStrSeperator);
            if (toCmprTuple == null) return null;
            var now = NowTimeNormazlied();
            var timeSpan = now - toCmprTuple.Item1;
            return System.Math.Abs(timeSpan.TotalMinutes) < minutes ? toCmprTuple.Item2 : null;
        }
        #endregion
      
        #region Mac Encryption
        public bool IsMacValid(string[] macHashes)
        {
            var hashes = GetEncryptedMacAddress();
            return hashes.Any(a => macHashes.Contains(a));
        }
        //If hashing failed will return empty string
        public string[] GetMacAddresses(string[] macHashes)
        {
            return macHashes.Select(a =>
            {
                try
                {
                    return DecryptString(a);
                }
                catch (System.Exception) { return ""; }
                    }).ToArray();// GetEncryptedMacAddress().ToArray();
        }
        public string[] GetEncryptedMacAddress()
        {
            return GetMacAddress().Select(a => EncryptString(a)).ToArray();
            
        }

        public string[] GetMacAddress()
        {
            var networks = NetworkInterface.GetAllNetworkInterfaces();
            var ret = new string[networks.Length];
            for (int i = 0; i < networks.Length; i++)
            {
                var mac = networks[i].GetPhysicalAddress();
                ret[i] = mac.ToString();
            }
            return ret;
        }

        #endregion
    }

    public static class ExtendNetworkInterface
    {
        public static string NicToStr(this NetworkInterface that)
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", that.Id, that.Name, that.Description, that.NetworkInterfaceType, that.OperationalStatus, that.IsReceiveOnly, that.Speed,
                that.GetPhysicalAddress());
        }
    }
}