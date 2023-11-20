using EasyWord.Library.Services;

namespace EasyWord.Services;

//此处实现PreferenceStorage键值对存储，只有MAUI中有，所以只能在此实现，其他项目调接口使用 xj实现
public class PreferenceStorage : IPreferenceStorage {
    public void Set(string key, int value) =>
        Preferences.Set(key, value);

    public int Get(string key, int defaultValue) =>
        Preferences.Get(key, defaultValue);

    public void Set(string key, string value)
    {
        Preferences.Set(key, value);
    }

    public string Get(string key, string defaultValue)
    {
        return Preferences.Get(key, defaultValue);
    }

    public void Set(string key, DateTime value)
    {
        Preferences.Set(key, value);
    }

    public DateTime Get(string key, DateTime defaultValue)
    {
        return Preferences.Get(key, defaultValue);
    }
}