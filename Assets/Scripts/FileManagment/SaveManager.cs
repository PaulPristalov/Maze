using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static readonly string Path = Application.dataPath + "/Saves";

    public const string LEVELS_PATH = "Levels";

    public static void Save(string saveName, object data)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }

        string path = $"{Path}/{saveName}.save";
        FileStream file = File.Create(path);
        formatter.Serialize(file, data);

        file.Close();
    }

    public static object Load(string name)
    {
        string path = $"{Path}/{name}.save";
        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            object save = formatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch
        {
            Debug.LogError("Failed to load save at " + path);
            file.Close();
            return null;
        }
    }

    private static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();

        var vector3Surrogate = new Vector3SerializationSurrogate();
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        var vector2IntSurrogate = new Vector2IntSerializationSurrogate();
        selector.AddSurrogate(typeof(Vector2Int), new StreamingContext(StreamingContextStates.All), vector2IntSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }

    public static bool Exists(string name)
    {
        return File.Exists($"{Path}/{name}.save");
    }

    public static void Delete(string name)
    {
        File.Delete($"{Path}/{name}.save");
    }
}

public class Vector3SerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Vector3 vector3 = (Vector3)obj;
        info.AddValue("x", vector3.x);
        info.AddValue("y", vector3.y);
        info.AddValue("z", vector3.z);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector3 vector3 = (Vector3)obj;
        vector3.x = (float)info.GetValue("x", typeof(float));
        vector3.y = (float)info.GetValue("y", typeof(float));
        vector3.z = (float)info.GetValue("z", typeof(float));
        obj = vector3;
        return obj;
    }
}

public class Vector2IntSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Vector2Int vector2int = (Vector2Int)obj;
        info.AddValue("x", vector2int.x);
        info.AddValue("y", vector2int.y);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector2Int vector2int = (Vector2Int)obj;
        vector2int.x = (int)info.GetInt32("x");
        vector2int.y = (int)info.GetInt32("y");
        obj = vector2int;
        return obj;
    }
}
