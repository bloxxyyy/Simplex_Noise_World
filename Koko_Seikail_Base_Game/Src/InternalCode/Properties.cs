using System;
using System.Collections.Specialized;
using System.IO;

namespace Koko_Seikail_Base_Game.Src.InternalCode;
public class PropertiesManager {

    private readonly NameValueCollection _properties = new();

    public PropertiesManager(string fileNameWithoutExtension) {
        _properties = GetProperties(fileNameWithoutExtension);
    }

    public T GetProperty<T>(string propertyName) {
        object value = _properties[propertyName];
        if (value == null) {
            return default;
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }

    private NameValueCollection GetProperties(string fileNameWithoutExtension) {
        var properties = new NameValueCollection();
        string[] lines = File.ReadAllLines("../../../Src/Properties/" + fileNameWithoutExtension + ".properties");
        foreach (string line in lines) {
            if (!line.StartsWith("#") && line.Contains("=")) {
                string[] split = line.Split("=");
                properties.Add(split[0].Trim(), split[1].Trim());
            }
        }
        
        return properties;
    }
}
