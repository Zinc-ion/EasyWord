using EasyWord.Library.Services;

namespace EasyWord.Web.Services;

public class PreferenceStorage : IPreferenceStorage
{
    private static readonly Dictionary<string, int> _intPreferences = new();

    private static readonly Dictionary<string, string> _stringPreferences =
        new();

    private static readonly Dictionary<string, DateTime> _dateTimePreferences =
        new();

    public void Set(string key, int value) => _intPreferences[key] = value;

    public int Get(string key, int defaultValue) =>
        _intPreferences.TryGetValue(key, out var value) ? value : defaultValue;

    public void Set(string key, string value) =>
        _stringPreferences[key] = value;

    public string Get(string key, string defaultValue) =>
        _stringPreferences.TryGetValue(key, out var value)
            ? value
            : defaultValue;

    public void Set(string key, DateTime value) =>
        _dateTimePreferences[key] = value;

    public DateTime Get(string key, DateTime defaultValue) =>
        _dateTimePreferences.TryGetValue(key, out var value)
            ? value
            : defaultValue;
}