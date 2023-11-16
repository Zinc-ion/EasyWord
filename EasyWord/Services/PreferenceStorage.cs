using DailyPoetryH.Library.Services;

namespace DailyPoetryH.Services;

public class PreferenceStorage : IPreferenceStorage {
    public void Set(string key, int value) => Preferences.Set(key, value);

    public int Get(string key, int defaultValue) =>
        Preferences.Get(key, defaultValue);
}