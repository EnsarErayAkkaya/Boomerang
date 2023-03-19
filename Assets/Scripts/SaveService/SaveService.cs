using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveService
{
    public static SaveData saveData = new SaveData();

    public const string saveFile = "/gamesave.save";

    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveFile);
        bf.Serialize(file, saveData);
        file.Close();

        Debug.Log("Game Saved");
    }
    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + saveFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveFile, FileMode.Open);
            saveData = (SaveData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            saveData = new SaveData();
            Debug.Log("No game saved!");
        }
    }
}
