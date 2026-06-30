namespace Library.Helpers;

public static class JsonHelper
{
    public static string Serialize(object obj, bool prettyPrint = false)
    {
        var formatting = prettyPrint ? Formatting.Indented : Formatting.None;
        return JsonConvert.SerializeObject(obj, formatting);
    }

    public static T? Deserialize<T>(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "JsonHelper.Deserialize failed for type {Type}", typeof(T).Name);
            return default;
        }
    }

    public static bool TryDeserialize<T>(string json, out T? result)
    {
        try
        {
            result = JsonConvert.DeserializeObject<T>(json);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static bool IsValid(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return false;
        try
        {
            JToken.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string PrettyPrint(string json)
    {
        try
        {
            return JToken.Parse(json).ToString(Formatting.Indented);
        }
        catch
        {
            return json;
        }
    }

    public static T? Clone<T>(T obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
