using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using System;

public class Data 
{
    public string Name;
    public float Height;
    private string secret;

    public Data(string name, float height, string secret)
    {
        Name = name;
        Height = height;
        this.secret = secret;
    }

    public override string ToString()
    {
        return Name + " " + Height + " " + secret;
    }
}


public class JsonTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Data charles = new Data("철수", 198, "발바닥에 점이 두개");

        string json1 = JsonConvert.SerializeObject(charles);
        Debug.Log(json1);

        Data aMan = JsonConvert.DeserializeObject<Data>(json1);
        Debug.Log(aMan);

        Save(aMan, "save.txt");
    }

    void Save(Data data, string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(path);

        try
        {
            string json  = JsonConvert.SerializeObject(data);
            json = SimpleEncryptionUtility.Encrypt(json);
            File.WriteAllText(path, json);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
