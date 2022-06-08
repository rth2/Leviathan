using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
            Directory.CreateDirectory(SAVE_FOLDER);
    }

    public static void Save(string saveString, string saveFileName)
    {
        File.WriteAllText(SAVE_FOLDER + saveFileName + ".txt", saveString);
    }

    public static string Load(string saveFileName)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");

        if (File.Exists(SAVE_FOLDER + saveFileName + ".txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + saveFileName + ".txt");
            return saveString;
        }
        else
            return null;
    }
}
