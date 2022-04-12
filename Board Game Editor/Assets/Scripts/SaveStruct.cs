using UnityEngine;
using System.IO;

public static class SaveStruct
{
    public static string directory = "/SaveData/";
    public static string filename = "SaveFile.txt";

    public static void Save(SaveObject so){
        string dir = Application.persistentDataPath + directory;
        string fullpath = dir + filename;
        string json = JsonUtility.ToJson(so);
        File.WriteAllText(fullpath, json);
        Debug.Log(JsonUtility.ToJson(so));
    }

    public static void Load(SaveObject so){
        //SaveObject so = new SaveObject();
        string dir = Application.persistentDataPath + directory;

        if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string fullpath = dir + filename;

        if(File.Exists(fullpath)){
            string json = File.ReadAllText(fullpath);
            Debug.Log(json);
            JsonUtility.FromJsonOverwrite(json, so);
            //return so;
        }else{
            Debug.Log("Save File not found");
            File.WriteAllText(fullpath, "");
            //return so;
        }
    }
}
