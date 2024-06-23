using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlayerPrefsHelper
{
    public static string MoneyKey = "Money";
    public const string IntListKey = "IntListKey";

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public static string GetString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public const string EnvironmentDataKey = "EnvironmentData";

    // Save the list of EnvironmentData to PlayerPrefs
    public static void SaveEnvironmentDataList(List<EnvironmentData> dataList)
    {
        string json = JsonUtility.ToJson(new SerializationWrapper<EnvironmentData>(dataList));
        PlayerPrefs.SetString(EnvironmentDataKey, json);
        PlayerPrefs.Save();
    }

    // Load the list of EnvironmentData from PlayerPrefs
    public static List<EnvironmentData> LoadEnvironmentDataList()
    {
        if (PlayerPrefs.HasKey(EnvironmentDataKey))
        {
            string json = PlayerPrefs.GetString(EnvironmentDataKey);
            SerializationWrapper<EnvironmentData> wrapper = JsonUtility.FromJson<SerializationWrapper<EnvironmentData>>(json);
            return wrapper.data;
        }
        return new List<EnvironmentData>();
    }

    // Wrapper class for serialization
    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> data;

        public SerializationWrapper(List<T> data)
        {
            this.data = data;
        }
    }

}
