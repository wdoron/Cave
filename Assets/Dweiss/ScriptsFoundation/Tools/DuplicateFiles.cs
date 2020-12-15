using System.Collections;

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class DuplicateFiles : MonoBehaviour {
    public string prefix;
    public string orginalName;
    public string suffix = ".mp4";
    public string newfolder;
    public string[] targetNamesPrefix;


    public enum OriginalNameType
    {
        Prefix,
        FullName,
        FolderToCopyFromWithPrefix
    }

    public OriginalNameType sourceType;
    public bool active;
    public bool overrideFiles;
    private void Reset()
    {
        prefix = Application.dataPath + "/StreamingAssets/";
    }

    public bool workOnData = false;
    private void WorkOnData()
    {
        for (int i = 0; i < targetNamesPrefix.Length; ++i) targetNamesPrefix[i] = targetNamesPrefix[i].Substring(1);
    }

    private FileInfo[] ReadFolder()
    {
        //var info = new DirectoryInfo(path);
        //var fileInfo = info.GetFiles();
        //foreach (var file in fileInfo) Debug.Log(file);

        DirectoryInfo dir = new DirectoryInfo(prefix + orginalName);
        FileInfo[] info = dir.GetFiles("*.*");
        //foreach (FileInfo f in info) Debug.Log(f);
        return info;
    }



    public void Update()
    {
        if (workOnData)
        {
            //WorkOnData();
            workOnData = false;
        }
        if (active)
        {
            Debug.Log("Copy started");
            if (sourceType == OriginalNameType.Prefix)
            {
                var targetNames = new List<string>();
                foreach (var prf in targetNamesPrefix)
                {
                    targetNames.Add(prf + orginalName);
                }

                CopyFile(orginalName, false, targetNames.ToArray());
            }else if(sourceType == OriginalNameType.FullName)
            {
                CopyFile(orginalName, false, targetNamesPrefix);
            }
            else if (sourceType == OriginalNameType.FolderToCopyFromWithPrefix)
            {
                var files = ReadFolder();
                

                foreach (var f in files)
                {
                    var targetNames = new List<string>();
                    foreach (var prf in targetNamesPrefix)
                    {
                        targetNames.Add(prf + f.Name);
                    }
                    CopyFile(f.FullName, true, targetNames.ToArray());
                }
            }
            active = false;
        }
    }

    public bool copyActive;

    private void CopyFile(string fileName, bool sourceFullName, params string[] targetNames)
    {
        if(newfolder != null)
        {
            var dirPath = prefix + newfolder;
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
        }
        var fullName = sourceFullName ? fileName :( prefix + fileName + suffix);

        var res = "Copy " + fullName + " to:\n ";
        foreach (var targetName in targetNames)
        {
            var fullTarget = prefix + newfolder + targetName + (targetName.IndexOf(".") < 0 ? suffix : "");
            var fi = new FileInfo(fullTarget);
            if (fi.Exists == false || overrideFiles)
            {
                res +=  fullTarget + "\n";
               if(copyActive) System.IO.File.Copy(fullName, fullTarget, true);
            } else
            {
                res += "File exists and override disable " + fullTarget + "\n";
            }
        }
        Debug.Log(res);
    }
}
