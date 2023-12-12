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
        if (File.Exists(Application.persistentDataPath + "/SaveData.save"))
        {
            try
            {
                if(File.Exists(Application.dataPath + "/SaveData.json"))
                {
                    string saveString = File.ReadAllText(Application.dataPath + "/SaveData.json");
                    SaveData data = JsonUtility.FromJson<SaveData>(saveString);
                    return data;
                }
                //BinaryFormatter binaryFormatter = new BinaryFormatter();
                //FileStream file = File.Open(Application.persistentDataPath + "/SaveData.save", FileMode.Open);
                //SaveData saveData =  JsonUtility.from < SaveData>(file)//= (SaveData)binaryFormatter.Deserialize(file);
                //file.Close();
                //return saveData;
            }
            catch
            {

            }
        }
        return null;
    }
}
