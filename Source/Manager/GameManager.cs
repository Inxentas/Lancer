using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : Entity
{
    // Fields
    public AudioMixer audioMixer;

    // SaveData
    public string saveDataPath = "";
    public SettingsData settingsData = new SettingsData();
    public SaveData saveData = new SaveData();

    protected override void OnAwake()
    {
        base.OnAwake();
        this.PrepareIO();
        Application.targetFrameRate = 60;
    }
    private void PrepareIO()
    {
        this.saveDataPath = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(this.saveDataPath)) { Directory.CreateDirectory(this.saveDataPath); }
        if (!File.Exists(saveDataPath + "/settings.json")) { SaveSettings(); } else { LoadSettings(); }
        //GameEvents.settingsDataPropagate.Invoke(this.settingsData);
    }
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(this.settingsData);
        File.WriteAllText(this.saveDataPath + "/settings.json", json);
        //Debug.Log(this + " Settings saved.");
        //Debug.Log(json);
    }
    public void LoadSettings()
    {
        string json = File.ReadAllText(this.saveDataPath + "/settings.json");
        this.settingsData = JsonUtility.FromJson<SettingsData>(json);
        //Debug.Log(this + " Settings loaded.");
        //Debug.Log(json);
    }
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(this.saveData);
        File.WriteAllText(this.saveDataPath + "/save.json", json);
        //Debug.Log(this + " Game saved.");
        //Debug.Log(json);
    }
    public void LoadGame()
    {
        if (File.Exists(saveDataPath + "/save.json"))
        {
            string json = File.ReadAllText(this.saveDataPath + "/save.json");
            this.saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            this.saveData = new SaveData();
        }
        //Debug.Log(this + " Game loaded.");
        //Debug.Log(json);
    }

    public static float SliderToDecibel(float value)
    {
        return Remap(value, 0, 1, -80, 0);
    }
    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
[Serializable]
public class SaveData
{
    public float timePlayed = 0;
}
[Serializable]
public class SettingsData
{
    // volumes are stored as a value between 0 (min) and 1 (max).
    [Range(0, 1)] public float volumeMaster = 1.0f;
    [Range(0, 1)] public float volumeBGM = 1.0f;
    [Range(0, 1)] public float volumeSFX = 1.0f;
}
