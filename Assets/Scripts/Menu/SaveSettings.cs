using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSettings
{
    public static void _SaveSettings(bool fullscreen, int resolutionWidth, int resolutionHeight, int indexResolution, int indexFrequency, int frequency, int quality)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Settings.Data";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveSettingsData data = new SaveSettingsData(fullscreen, resolutionWidth, resolutionHeight, indexResolution, indexFrequency, frequency, quality);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveSettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "/Settings.Data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveSettingsData data = formatter.Deserialize(stream) as SaveSettingsData;
            stream.Close();

            return data;

        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }
}

[System.Serializable]
public class SaveSettingsData
{
    public bool Fullscreen;
    public int ResolutionWidth;
    public int ResolutionHeight;
    public int IndexResolution;
    public int IndexFrequency;
    public int Frequency;
    public int Quality;

    public SaveSettingsData(bool fullscreen, int resolutionWidth, int resolutionHeight, int indexResolution, int indexFrequency, int frequency, int quality)
    {
        Fullscreen = fullscreen;
        ResolutionWidth = resolutionWidth;
        ResolutionHeight = resolutionHeight;
        IndexResolution = indexResolution;
        IndexFrequency = indexFrequency;
        Frequency = frequency;
        Quality = quality;
    }
}
