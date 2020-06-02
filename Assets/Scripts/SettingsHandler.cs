using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField]
    private Slider volume_master = null;

    [SerializeField]
    private Slider volume_gun = null;

    [SerializeField]
    private Slider volume_music = null;

    [SerializeField]
    private Slider volume_YOUDIED = null;

    [SerializeField]
    private Slider volume_zombie = null;

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

    [SerializeField]
    private int increments = 20;

    private void Start()
    {
        ReadToCache();
        youDiedEnabled.isOn = cache.youDiedEnabled;
        highscoresDeletable.isOn = cache.highscoreDeletable;
        difficulty.value = (int)cache.dificulty;
        showControllerKeyboard.isOn = cache.showControllerKeyboard;
        preloadHighscores.isOn = cache.preloadHighscores;

        volume_master.maxValue = increments;
        volume_gun.maxValue = increments;
        volume_music.maxValue = increments;
        volume_YOUDIED.maxValue = increments;
        volume_zombie.maxValue = increments;

        volume_master.value = cache.volume_master * increments;
        volume_gun.value = cache.volume_gun * increments;
        volume_music.value = cache.volume_music * increments;
        volume_YOUDIED.value = cache.volume_YOUDIED * increments;
        volume_zombie.value = cache.volume_zombie * increments;

        youDiedEnabled.onValueChanged.AddListener(YouDiedEnabledChanged);
        highscoresDeletable.onValueChanged.AddListener(HighscoresDeletableChanged);
        difficulty.onValueChanged.AddListener(DifficultyChanged);
        showControllerKeyboard.onValueChanged.AddListener(ControllerKeyboardChanged);
        preloadHighscores.onValueChanged.AddListener(PreloadChanged);

        volume_master.onValueChanged.AddListener(Volume_Master);
        volume_gun.onValueChanged.AddListener(Volume_Gun);
        volume_music.onValueChanged.AddListener(Volume_Music);
        volume_YOUDIED.onValueChanged.AddListener(Volume_YOUDIED);
        volume_zombie.onValueChanged.AddListener(Volume_Zombie);
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

    // volume sliders
    private void Volume_Master(float value)
    {
        Debug.Log("New Value");
        Settings s = cache;
        s.volume_master = value / increments;
        cache = s;
        WriteFromCache();
    }

    private void Volume_Gun(float value)
    {
        Settings s = cache;
        s.volume_gun = value / increments;
        cache = s;
        WriteFromCache();
    }

    private void Volume_Music(float value)
    {
        Settings s = cache;
        s.volume_music = value / increments;
        cache = s;
        WriteFromCache();
    }

    private void Volume_YOUDIED(float value)
    {
        Settings s = cache;
        s.volume_YOUDIED = value / increments;
        cache = s;
        WriteFromCache();
    }

    private void Volume_Zombie(float value)
    {
        Settings s = cache;
        s.volume_zombie = value / increments;
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

    public float volume_master, volume_gun, volume_music, volume_YOUDIED, volume_zombie;

    #region constants

    private const string PrefEnrtyName_youDiedEnabled = "youDiedEnabled";
    private const string PrefEnrtyName_highscoreDeletable = "highscoresDeletable";
    private const string PrefEnrtyName_difficulty = "difficulty";
    private const string PrefEnrtyName_showControllerKeyboard = "showKeyboard";
    private const string PrefEnrtyName_preloadHighscores = "preload";

    private const string PrefEnrtyName_volMaster = "volumeMaster";
    private const string PrefEnrtyName_volGun = "volumeGun";
    private const string PrefEnrtyName_volMusic = "volumeMusic";
    private const string PrefEnrtyName_volYOUDIED = "volumeYOUDIED";
    private const string PrefEnrtyName_volZombie = "volumeZombie";

    public Settings(bool youDiedEnabled, bool highscoreDeletable, Difficulty dificulty, bool showControllerKeyboard, bool preloadHighscores, float volume_master, float volume_gun, float volume_music, float volume_YOUDIED, float volume_zombie)
    {
        this.youDiedEnabled = youDiedEnabled;
        this.highscoreDeletable = highscoreDeletable;
        this.dificulty = dificulty;
        this.showControllerKeyboard = showControllerKeyboard;
        this.preloadHighscores = preloadHighscores;
        this.volume_master = volume_master;
        this.volume_gun = volume_gun;
        this.volume_music = volume_music;
        this.volume_YOUDIED = volume_YOUDIED;
        this.volume_zombie = volume_zombie;
    }

    #endregion constants

    public static Settings Read()
    {
        bool youDiedEnabled = PlayerPrefs.GetInt(PrefEnrtyName_youDiedEnabled, 1) == 0 ? false : true;
        bool highscoreDeletable = PlayerPrefs.GetInt(PrefEnrtyName_highscoreDeletable, 1) == 0 ? false : true;
        Difficulty difficulty = (Difficulty)PlayerPrefs.GetInt(PrefEnrtyName_difficulty, 1);
        bool showControllerKeyboard = PlayerPrefs.GetInt(PrefEnrtyName_showControllerKeyboard, 1) == 0 ? false : true;
        bool preloadHighscores = PlayerPrefs.GetInt(PrefEnrtyName_preloadHighscores, 1) == 0 ? false : true;

        float volumeMaster = PlayerPrefs.GetFloat(PrefEnrtyName_volMaster, .5f);
        float volumeGun = PlayerPrefs.GetFloat(PrefEnrtyName_volGun, .5f);
        float volumeMusic = PlayerPrefs.GetFloat(PrefEnrtyName_volMusic, .5f);
        float volumeYOUDIED = PlayerPrefs.GetFloat(PrefEnrtyName_volYOUDIED, .5f);
        float volumeZombie = PlayerPrefs.GetFloat(PrefEnrtyName_volZombie, .5f);
        return new Settings(youDiedEnabled, highscoreDeletable, difficulty, showControllerKeyboard, preloadHighscores, volumeMaster, volumeGun, volumeMusic, volumeYOUDIED, volumeZombie);
    }

    public static void Write(Settings settings)
    {
        PlayerPrefs.SetInt(PrefEnrtyName_youDiedEnabled, settings.youDiedEnabled ? 1 : 0);
        PlayerPrefs.SetInt(PrefEnrtyName_highscoreDeletable, settings.highscoreDeletable ? 1 : 0);
        PlayerPrefs.SetInt(PrefEnrtyName_difficulty, (int)settings.dificulty);
        PlayerPrefs.SetInt(PrefEnrtyName_showControllerKeyboard, settings.showControllerKeyboard ? 1 : 0);
        PlayerPrefs.SetInt(PrefEnrtyName_preloadHighscores, settings.preloadHighscores ? 1 : 0);

        PlayerPrefs.SetFloat(PrefEnrtyName_volMaster, settings.volume_master);
        PlayerPrefs.SetFloat(PrefEnrtyName_volGun, settings.volume_gun);
        PlayerPrefs.SetFloat(PrefEnrtyName_volMusic, settings.volume_music);
        PlayerPrefs.SetFloat(PrefEnrtyName_volYOUDIED, settings.volume_YOUDIED);
        PlayerPrefs.SetFloat(PrefEnrtyName_volZombie, settings.volume_zombie);
    }
}

public enum Difficulty
{
    Easy, Normal, Hard, Extreme
}