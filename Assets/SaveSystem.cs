using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/saves/";
    public static readonly string FILE_EXT = ".json";

    public static void Save(string fileName, string dataToSave)
    {
        Debug.Log("Save path" + SAVE_FOLDER);
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        File.WriteAllText(SAVE_FOLDER + fileName + FILE_EXT, dataToSave);
    }

    public static string Load(string fileName)
    {
        string fileLocation = SAVE_FOLDER + fileName + FILE_EXT;

        if (File.Exists(fileLocation))
        {
            string loadedData = File.ReadAllText(fileLocation);

            return loadedData;
        }
        else
        {
            return null;
        }
    }
}
