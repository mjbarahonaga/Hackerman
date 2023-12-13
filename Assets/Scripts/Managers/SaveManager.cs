using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager 
{
    public static void SaveGameState(SaveData saveData)
    {
        var json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.dataPath + "/SaveData.json", json);
        //binaryFormatter.Serialize(file, json);
    }

    public static SaveData LoadGameState()
    {

        try
        {
            if(File.Exists(Application.dataPath + "/SaveData.json"))
            {
                string saveString = File.ReadAllText(Application.dataPath + "/SaveData.json");
                SaveData data = JsonUtility.FromJson<SaveData>(saveString);
                return data;
            }
        }
        catch
        {

        }
        
        return null;
    }
}
