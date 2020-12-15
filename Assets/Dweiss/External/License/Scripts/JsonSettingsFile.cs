using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
//using Newtonsoft.Json;

namespace Dweiss.Store
{
    [System.Serializable]
    public class JsonSettingsFile
    {


        private string appDir;
        private string AppDirectory
        {
            get
            {

                if (string.IsNullOrEmpty(appDir))
                {
#if UNITY_EDITOR
                    appDir = Application.dataPath;
#else
                    appDir = System.IO.Path.Combine(Application.dataPath, "../");
#endif
                }
                return appDir;
            }
        }

        
        private string GetParentDirectory(string folder)
        {
            return
                 folder.IndexOf(':') >= 0 ? folder :
                 (AppDirectory + "/" + folder);
        }

        public string GetFullFileName(string fileName, string folder)
        {
            return GetParentDirectory(folder) + "/" + fileName;
        }

        private StreamWriter GetWriteStream(string fileName, string folderName)
        {

            if (Directory.Exists(GetParentDirectory(folderName)) == false)
            {
                var dirInfo = Directory.CreateDirectory(GetParentDirectory(folderName));
                Debug.Log("Create directory " + dirInfo.Name + " (" + dirInfo.FullName + ")");
            }

            var fullFileName = GetFullFileName(fileName, folderName);
            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }


            var ret = new System.IO.StreamWriter(fullFileName); ;
            return ret;

        }
        public bool Exists(string fileName, string folder)
        {
            var fullFileName = GetFullFileName(fileName, folder);
            return (File.Exists(fullFileName));
        }

        public bool Read<T>(string fileName, string folder, out T obj)
        {


            var fullFileName = GetFullFileName(fileName, folder);
            if (File.Exists(fullFileName))
            {
                using (var fileStream = new System.IO.StreamReader(fullFileName))
                {

                    var line = fileStream.ReadLine();
                    //Debug.Log(fullFileName + " reading " + line);

                    obj = JsonUtility.FromJson<T>(line);
                    return true;
                }

            }
            obj = default(T);


            return false;

        }
        public bool Write(string fileName, string folder, object obj)
        {
            var fullFileName = GetFullFileName(fileName, folder);
            try
            {
                //Debug.Log("Full file name " + Path.GetFullPath(fullFileName));
                using (var fileStream = GetWriteStream(fileName, folder))
                {
                    var objToWrite = obj;
                    var json = JsonUtility.ToJson(objToWrite);
                    fileStream.WriteLine(json);
                    //Debug.Log("<color=blue>Json settings file " + fullFileName + " </color>" );
                    return true;
                }
                
            }
            catch (System.Exception e)
            {
                Debug.LogError(fullFileName + " " + e.Message + "\n" + e.StackTrace + "\n" + e);
            }
            return false;
        }

        public void WriteAsync(string fileName, string folder, object objToWrite)
        {

            //Debug.Log("Full file name " + System.IO.Path.GetFullPath(fullFileName));

            var json = JsonUtility.ToJson(objToWrite);
            var fullFileName = GetFullFileName(fileName, folder);

            System.Threading.ThreadPool.QueueUserWorkItem((a) =>
            {
                try
                {

                    using (var fileStream = GetWriteStream(fileName, folder))
                    {
                        fileStream.WriteLine(json);
                        //MainThread.Instance.RunInMainThread(() => {
                            Debug.Log("Json settings file " + fullFileName);
                       // });
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(fullFileName + " " + e.Message + "\n" + e.StackTrace + "\n" + e);
                }
            });
        }

    }
}