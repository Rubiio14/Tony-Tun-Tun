using System.IO;
using System;
using UnityEngine;

public static class FileManager
{
    public static bool DoesFileExists(string filename)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, filename);
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

    public static bool WriteToFile(string savegameFileName, string fileContents)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, savegameFileName);

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

    public static bool LoadFromFile(string savegameFileName, out string result)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, savegameFileName);

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

    public static bool Delete(string savegameFileName)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, savegameFileName);
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
