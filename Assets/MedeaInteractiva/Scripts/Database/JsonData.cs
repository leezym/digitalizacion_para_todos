using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using System.IO;

public class JsonData
{
    /// <summary>
    /// The JSON serializer.
    /// </summary>
    private fsSerializer serializer = new fsSerializer();

    /// <summary>
    /// Loads the JSON file from the specified path.
    /// </summary>
    /// <typeparam name="T">The type of data to load.</typeparam>
    /// <param name="path">The path to the file.</param>
    /// <returns>The data contained in the file.</returns>
    private T LoadJSONFile<T>(string path) where T : class
    {
        if (File.Exists(path))
        {
            var file = new StreamReader(path);
            var fileContents = file.ReadToEnd();
            var data = fsJsonParser.Parse(fileContents);

            object deserialized = null;
            serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
            file.Close();
            return deserialized as T;
        }
        return null;
    }

    /// <summary>
    /// Loads the JSON data from the specified string.
    /// </summary>
    /// <typeparam name="T">The type of data to load.</typeparam>
    /// <param name="json">The JSON string.</param>
    /// <returns>The data contained in the string.</returns>
    private T LoadJSONString<T>(string json) where T : class
    {
        var data = fsJsonParser.Parse(json);
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
        return deserialized as T;
    }

    /// <summary>
    /// Saves the specified data to the specified path.
    /// </summary>
    /// <typeparam name="T">The type of data to save.</typeparam>
    /// <param name="path">The path where to save the data.</param>
    /// <param name="data">The data to save.</param>
    private void SaveJSONFile<T>(string path, T data) where T : class
    {
        fsData serializedData;
        serializer.TrySerialize(data, out serializedData).AssertSuccessWithoutWarnings();
        var json = fsJsonPrinter.PrettyJson(serializedData);
        var file = new StreamWriter(path);
        file.WriteLine(json);
        file.Close();
    }

    public CustomizedString[] LoadCustomizedString(string json)
    {
        var result = LoadJSONString<CustomizedString[]>(json);

        return result;
    }

    public string GetCustomizedString(CustomizedString[] json)
    {
        fsData serializedData;
        CustomizedString[] result = json;
        serializer.TrySerialize(result, out serializedData).AssertSuccessWithoutWarnings();
        return fsJsonPrinter.PrettyJson(serializedData);
    }

    public EncuestaM3 LoadSurveyM3(string json)
    {
        var result = LoadJSONString<EncuestaM3>(json);

        return result;
    }

    public string GetSurveyM3(EncuestaM3 json)
    {
        fsData serializedData;
        EncuestaM3 result = json;
        serializer.TrySerialize(result, out serializedData).AssertSuccessWithoutWarnings();
        return fsJsonPrinter.PrettyJson(serializedData);
    }
}
