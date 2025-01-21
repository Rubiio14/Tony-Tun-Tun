using System.IO;
using System;
using UnityEngine;

public static class FileManager
{

    private static readonly string _savegameFileName = "TonyTunTunSavedata.dat";

    public static bool DoesSaveFileExists()
    {
        var fullPath = Path.Combine(Application.persistentDataPath, _savegameFileName);
        try
        {
            return File.Exists(fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
            return false;
        }
    }

    public static bool WriteToSaveFile(string fileContents)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, _savegameFileName);

        try
        {
            File.WriteAllText(fullPath, fileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
            return false;
        }
    }

    public static bool LoadFromSaveFile(out string result)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, _savegameFileName);

        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
            result = "";
            return false;
        }
    }

    public static bool DeleteSavefile()
    {
        var fullPath = Path.Combine(Application.persistentDataPath, _savegameFileName);
        try
        {
            File.Delete(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete {fullPath} with exception {e}");
            return false;
        }
    }
}
