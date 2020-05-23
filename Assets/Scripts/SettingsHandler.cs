using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsHandler : MonoBehaviour
{

    [SerializeField]
    private Toggle youDiedEnabled = null;

    [SerializeField]
    private Toggle highscoresDeletable = null;

    [SerializeField]
    private TMP_Dropdown difficulty = null;

    [SerializeField]
    private Toggle showControllerKeyboard = null;

    [SerializeField]
    private Toggle preloadHighscores = null;

    private void Start()
    {
        ReadToCache();
        youDiedEnabled.isOn = cache.youDiedEnabled;
        highscoresDeletable.isOn = cache.highscoreDeletable;
        difficulty.value = (int)cache.dificulty;
        showControllerKeyboard.isOn = cache.showControllerKeyboard;
        preloadHighscores.isOn = cache.preloadHighscores;

        youDiedEnabled.onValueChanged.AddListener(YouDiedEnabledChanged);
        highscoresDeletable.onValueChanged.AddListener(HighscoresDeletableChanged);
        difficulty.onValueChanged.AddListener(DifficultyChanged);
        showControllerKeyboard.onValueChanged.AddListener(ControllerKeyboardChanged);
        preloadHighscores.onValueChanged.AddListener(PreloadChanged);
    }

    private void YouDiedEnabledChanged(bool value)
    {
        Settings s = cache;
        s.youDiedEnabled = value;
        cache = s;
        WriteFromCache();
    }

    private void HighscoresDeletableChanged(bool value)
    {
        Settings s = cache;
        s.highscoreDeletable = value;
        cache = s;
        WriteFromCache();
    }

    private void DifficultyChanged(int value)
    {
        Settings s = cache;
        s.dificulty = (Difficulty)value;
        cache = s;
        WriteFromCache();
    }

    private void ControllerKeyboardChanged(bool value)
    {
        Settings s = cache;
        s.showControllerKeyboard = value;
        cache = s;
        WriteFromCache();
    }

    private void PreloadChanged(bool value)
    {
        Settings s = cache;
        s.preloadHighscores = value;
        cache = s;
        WriteFromCache();
    }

    #region static

    public static Settings cache { get; private set; }

    public static void WriteFromCache()
    {
        Settings.Write(cache);
    }

    public static void ReadToCache()
    {
        cache = Settings.Read();
    }

    #endregion static
}

public struct Settings
{
    public bool youDiedEnabled;
    public bool highscoreDeletable;
    public Difficulty dificulty;
    public bool showControllerKeyboard;
    public bool preloadHighscores;

    #region constants

    private const string PrefEnrtyName_youDiedEnabled = "youDiedEnabled";
    private const string PrefEnrtyName_highscoreDeletable = "highscoresDeletable";
    private const string PrefEnrtyName_difficulty = "difficulty";
    private const string PrefEnrtyName_showControllerKeyboard = "showKeyboard";
    private const string PrefEnrtyName_preloadHighscores = "preload";

    #endregion constants

    public Settings(bool youDiedEnabled, bool highscoreDeletable, Difficulty dificulty, bool showControllerKeyboard, bool preloadHighscores)
    {
        this.youDiedEnabled = youDiedEnabled;
        this.highscoreDeletable = highscoreDeletable;
        this.dificulty = dificulty;
        this.showControllerKeyboard = showControllerKeyboard;
        this.preloadHighscores = preloadHighscores;
    }

    public static Settings Read()
    {
        bool youDiedEnabled = PlayerPrefs.GetInt(PrefEnrtyName_youDiedEnabled, 1) == 0 ? false : true;
        bool highscoreDeletable = PlayerPrefs.GetInt(PrefEnrtyName_highscoreDeletable, 1) == 0 ? false : true;
        Difficulty difficulty = (Difficulty)PlayerPrefs.GetInt(PrefEnrtyName_difficulty, 1);
        bool showControllerKeyboard = PlayerPrefs.GetInt(PrefEnrtyName_showControllerKeyboard, 1) == 0 ? false : true;
        bool preloadHighscores = PlayerPrefs.GetInt(PrefEnrtyName_preloadHighscores, 1) == 0 ? false : true;
        return new Settings(youDiedEnabled, highscoreDeletable, difficulty, showControllerKeyboard, preloadHighscores);
    }

    public static void Write(Settings settings)
    {
        PlayerPrefs.SetInt(PrefEnrtyName_youDiedEnabled, settings.youDiedEnabled ? 1 : 0);
        PlayerPrefs.SetInt(PrefEnrtyName_highscoreDeletable, settings.highscoreDeletable ? 1 : 0);
        PlayerPrefs.SetInt(PrefEnrtyName_difficulty, (int)settings.dificulty);
        PlayerPrefs.SetInt(PrefEnrtyName_showControllerKeyboard, settings.showControllerKeyboard ? 1 : 0);
        PlayerPrefs.SetInt(PrefEnrtyName_preloadHighscores, settings.preloadHighscores ? 1 : 0);
    }
}

public enum Difficulty
{
    Easy, Normal, Hard, Extreme
}