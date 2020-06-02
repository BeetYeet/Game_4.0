﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Tools;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField]
    private TMP_InputField nameInput;

    [SerializeField]
    private Button nameConfirmButton;

    [SerializeField]
    private Button nameDiscardButton;

    [SerializeField]
    private GameObject playingUI;

    [SerializeField]
    private GameObject deathUI;

    [SerializeField]
    private GameObject nameSubmitUI;

    [SerializeField]
    private CanvasGroup nameSubmitCanvasGroup;

    [SerializeField]
    private GameObject deathVideo;

    [SerializeField]
    private Button keyFirstSelected = null;

    [SerializeField]
    private RectTransform controllerKeyboard = null;

    [SerializeField]
    private GameObject pauseUI = null;

    public AudioHandler.AudioContext zombieLiveContext;
    public List<string> zombieLiveSounds;

    public AudioHandler.AudioContext zombieDieContext;
    public List<string> zombieDieSounds;

    public AudioHandler.AudioContext zombieNibbleContext;
    public List<string> zombieNibbleSounds;

    public AudioHandler.AudioContext zombieAttackContext;
    public List<string> zombieAttackSounds;

    public int Score
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return Mathf.FloorToInt(score * .1f);

                default:
                    return Mathf.FloorToInt(score);

                case Difficulty.Hard:
                    return Mathf.FloorToInt(score * 1.3f);

                case Difficulty.Extreme:
                    return Mathf.FloorToInt(score * 2f);
            }
        }
    }

    [SerializeField]
    private float score = 0f;

    public bool playing = true;
    private string playerName;
    private float time;
    public int kills = 0;

    public List<ScoreEntry> highscores;
    public int numHighscores { get; private set; }

    [SerializeField]
    private bool debugMode = false;

    [SerializeField]
    private bool isHighscoreScene = false;

    private MyInputSystem input = null;

    private Difficulty difficulty = Difficulty.Normal;

    [SerializeField]
    private GameObject ammoBox = null;

    [Range(0f, 1f), SerializeField]
    private float ammoRate = .5f;

    [SerializeField]
    private GameObject healthBox = null;

    [Range(0f, 1f), SerializeField]
    private float healthRate = .5f;

    [SerializeField]
    private HighscoreEntry afterGameEntry = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        numHighscores = PlayerPrefs.GetInt("numHighscores", 0);

        if (!isHighscoreScene)
        {
            nameConfirmButton.onClick.AddListener(delegate { OnSubmit(nameInput.text); });
            nameConfirmButton.onClick.AddListener(AudioHandler.Click);
            nameInput.onEndEdit.AddListener(OnSubmit);

            nameInput.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            nameInput.onValidateInput += ValidateName;

            nameDiscardButton.onClick.AddListener(OnDiscard);
            nameDiscardButton.onClick.AddListener(AudioHandler.Click);

            Time.timeScale = 1f;
        }

        SettingsHandler.ReadToCache();
        difficulty = SettingsHandler.cache.dificulty;

        input = new MyInputSystem();
        input.PlayerActionControlls.Menu.performed += Menu_performed;
        input.PlayerActionControlls.Restart.performed += Restart_performed;
    }

    public void DieSound(Vector3 position)
    {
        string sound = zombieDieSounds[Random.Range(0, zombieDieSounds.Count)];
        Sound(sound, position, zombieDieContext);
    }

    public void LiveSound(Vector3 position)
    {
        string sound = zombieLiveSounds[Random.Range(0, zombieLiveSounds.Count)];
        Sound(sound, position, zombieLiveContext);
    }

    public void NibbleSound(Vector3 position)
    {
        string sound = zombieNibbleSounds[Random.Range(0, zombieNibbleSounds.Count)];
        Sound(sound, position, zombieNibbleContext);
    }

    public void AttackSound(Vector3 position)
    {
        string sound = zombieAttackSounds[Random.Range(0, zombieAttackSounds.Count)];
        Sound(sound, position, zombieAttackContext);
    }

    private void Sound(string name, Vector3 position, AudioHandler.AudioContext context)
    {
        if (HighscoreHandler.instance.transform.GetChild(0).gameObject.activeSelf)
            return;
        AudioHandler.AudioContext modifiedContext = context;
        modifiedContext.volume *= SettingsHandler.cache.volume_master * SettingsHandler.cache.volume_zombie;
        if (!playing)
        {
            modifiedContext.volume *= .3f;
            modifiedContext.minPitch *= .85f;
            modifiedContext.maxPitch *= .85f;
        }
        AudioHandler.PlaySound(name, modifiedContext, position);
    }

    // A button
    private void Restart_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!isHighscoreScene && playing && Time.timeScale == 0f)
        {
            // paused so we restart
            SceneHandler.instance.LoadScene("Main Menu");
        }
        else if (!isHighscoreScene && !playing && deathUI.activeSelf)
        {
            StartCoroutine(LateNameSelect());
        }
    }

    private IEnumerator LateNameSelect()
    {
        yield return null;
        ShowNameSelect();
    }

    // B or MENU button
    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!isHighscoreScene && playing)
        {
            if (Time.timeScale == 0f)
            {
                // unpause
                playingUI.SetActive(true);
                pauseUI.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                // pause
                playingUI.SetActive(false);
                pauseUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void AddCharacter(string character)
    {
        if (string.IsNullOrEmpty(character))
            return;
        if (nameInput.text.Length < 6)
            nameInput.text += character[0];
    }

    public void RemoveCharacter()
    {
        string s = nameInput.text;
        string ns = "";
        for (int i = 0; i < s.Length - 1; i++)
        {
            ns += s[i];
        }
        nameInput.text = ns;
    }

    public bool GetPartialScores()
    {
        if (highscores == null)
            highscores = new List<ScoreEntry>();
        int toGo = Mathf.Clamp(numHighscores - highscores.Count, 0, 3);

        for (int i = 0; i < toGo; i++)
        {
            highscores.Add(ScoreEntry.Read(highscores.Count));
        }
        if (toGo != 0)
            return true;
        return false;
    }

    private char ValidateName(string text, int charIndex, char added)
    {
        if (char.IsLetterOrDigit(added) && charIndex < 6)
        {
            return char.ToUpper(added);
        }
        else
        {
            return (char)0x00;
        }
    }

    private void Update()
    {
        if (playing)
            score += Time.deltaTime * 10f;
    }

    public void OnEnemyDie(Vector3 pos)
    {
        kills++;
        score += 100f;
        DieSound(pos);

        if (UnityEngine.Random.value < healthRate)
        {
            Instantiate(healthBox, pos, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
        }
        else if (UnityEngine.Random.value < ammoRate)
        {
            Instantiate(ammoBox, pos, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
        }
    }

    public void OnRunEnd()
    {
        // do death things here
        playing = false;
        Time.timeScale = .2f;
        if (!debugMode && SettingsHandler.cache.youDiedEnabled)
        {
            deathVideo = Instantiate(deathVideo, transform);
            deathVideo.transform.GetChild(0).GetComponent<UnityEngine.Video.VideoPlayer>().SetDirectAudioVolume(0, SettingsHandler.cache.volume_YOUDIED * SettingsHandler.cache.volume_master / 10f);
            StartCoroutine(FadeInScoreUI());
        }
        ShowDeathUI();
        time = Time.timeSinceLevelLoad;
    }

    private void ShowDeathUI()
    {
        playingUI.SetActive(false);
        deathUI.SetActive(true);
    }

    private void ShowNameSelect()
    {
        GetScores();
        deathUI.SetActive(false);
        nameSubmitUI.SetActive(true);
        afterGameEntry.Initialize(ScoreEntry.New(Score, "", SettingsHandler.cache.dificulty, time, kills), 0);

        if (SettingsHandler.cache.showControllerKeyboard)
        {
            controllerKeyboard.gameObject.SetActive(true);
            keyFirstSelected.Select();
        }
        else
            nameInput.Select();

        if (SettingsHandler.cache.preloadHighscores)
        {
            highscores.Sort();
            ReadInHighscores();
        }
    }

    private void OnSubmit(string name)
    {
        if (name == "" || name == null)
        {
            // TODO: tell user name cant be blank
            return;
        }
        playerName = name;

        if (SettingsHandler.cache.preloadHighscores)
        {
            HighscoreHandler.instance.newest.SetNewName(playerName);
        }

        DisplayHighscores();
        AddScore(ScoreEntry.New(Score, name, SettingsHandler.cache.dificulty, time, kills));
    }

    private void OnDiscard()
    {
        DisplayHighscores();
    }

    private void DisplayHighscores()
    {
        nameSubmitUI.SetActive(false);
        HighscoreHandler.instance.Enable();

        if (SettingsHandler.cache.preloadHighscores)
        {
            StartCoroutine(SnapToAfterPreload());
        }
        else
        {
            highscores.Sort();
            ReadInHighscores();
        }
    }

    private IEnumerator SnapToAfterPreload()
    {
        while (SceneHandler.instance.haltTransitionIn > 0)
            yield return null;
        if (HighscoreHandler.instance.newest != null)
        {
            HighscoreHandler.instance.SnapTo((RectTransform)HighscoreHandler.instance.newest.transform);
            HighscoreHandler.instance.newest.SetNewName(playerName);
        }
    }

    private void ReadInHighscores()
    {
        ScoreEntry newScore = ScoreEntry.New(Score, "", SettingsHandler.cache.dificulty, time, kills);
        highscores.Add(newScore);
        highscores.Sort();
        int index = highscores.FindIndex((x) => x.Equals(newScore));
        SceneHandler.instance.haltTransitionIn++;
        HighscoreHandler.instance.Display(index);
        if (!SettingsHandler.cache.preloadHighscores)
            StartCoroutine(UpdateHighscoreProgress());
    }

    private IEnumerator UpdateHighscoreProgress()
    {
        SceneHandler.instance.transition.StartTransitionOut();
        while (SceneHandler.instance.haltTransitionIn > 0)
        {
            SceneHandler.instance.UpdateProgress(SceneHandler.instance.externalProgress);
            yield return null;
        }
        SceneHandler.instance.UpdateProgressDone();
        SceneHandler.instance.transition.StartTransitionIn();
        if (!SettingsHandler.cache.preloadHighscores)
            if (HighscoreHandler.instance.newest != null)
            {
                HighscoreHandler.instance.SnapTo((RectTransform)HighscoreHandler.instance.newest.transform);
                HighscoreHandler.instance.newest.SetNewName(playerName);
            }
    }

    public void GetScores()
    {
        ScoreEntry[] scores = new ScoreEntry[numHighscores];
        for (int i = 0; i < numHighscores; i++)
        {
            scores[i] = ScoreEntry.Read(i);
        }

        highscores = scores.ToList();
    }

    private void AddScore(ScoreEntry score)
    {
        ScoreEntry.Write(score, numHighscores);
        numHighscores++;
        PlayerPrefs.SetInt("numHighscores", numHighscores);
        PlayerPrefs.Save();
        return;
    }

    private IEnumerator FadeInScoreUI()
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - start < 8)
        {
            nameSubmitCanvasGroup.alpha = Mathf.Lerp(0f, 1f, Time.realtimeSinceStartup - start - 6);
            yield return null;
        }
        nameSubmitCanvasGroup.alpha = 1f;
    }
}

public struct ScoreEntry : System.IComparable<ScoreEntry>, System.IEquatable<ScoreEntry>
{
    #region constants

    private const string PrefEntryName_Score = "_score";
    private const string PrefEntryName_Name = "_name";
    private const string PrefEntryName_Difficulty = "_difficulty";
    private const string PrefEntryName_TimeTaken = "_timeTaken";
    private const string PrefEntryName_Kills = "_kills";
    private const string PrefEntryName_IRLTime_left = "_IRLTime_left";
    private const string PrefEntryName_IRLTime_right = "_IRLTime_right";
    private static readonly ScoreEntry error = new ScoreEntry(0, "LD_ERR", Difficulty.Normal, 0f, 0, backup, false);

    #endregion constants

    public int score;
    public string name;
    public Difficulty difficulty;
    public float timeTaken;
    public int kills;
    public System.DateTime entryIRL_Time;
    public bool isLast;

    private static System.DateTime backup = new System.DateTime(1945, 4, 30, 15, 30, 0, 7, System.DateTimeKind.Local);

    public static ScoreEntry New(int score, string name, Difficulty difficulty, float timeTaken, int kills)
    {
        ScoreEntry se = new ScoreEntry();
        se.score = score;
        se.name = name;
        se.difficulty = difficulty;
        se.timeTaken = timeTaken;
        se.kills = kills;
        se.entryIRL_Time = System.DateTime.Now;
        se.isLast = true;
        return se;
    }

    public ScoreEntry(int score, string name, Difficulty difficulty, float timeTaken, int kills, System.DateTime entryIRL_Time, bool isLast)
    {
        this.score = score;
        this.name = name;
        this.difficulty = difficulty;
        this.timeTaken = timeTaken;
        this.kills = kills;
        this.entryIRL_Time = entryIRL_Time;
        this.isLast = isLast;
    }

    public static bool Exists(int index)
    {
        return Exists(index, out _);
    }

    public static bool Exists(int index, out ScoreEntry check)
    {
        check = new ScoreEntry();
        bool success = true;
        if (PlayerPrefs.HasKey(index + PrefEntryName_Score))
            check.score = PlayerPrefs.GetInt(index + PrefEntryName_Score);
        else
            success = false;

        if (PlayerPrefs.HasKey(index + PrefEntryName_Name))
            check.name = PlayerPrefs.GetString(index + PrefEntryName_Name);
        else
            success = false;

        if (PlayerPrefs.HasKey(index + PrefEntryName_Difficulty))
            check.difficulty = (Difficulty)PlayerPrefs.GetInt(index + PrefEntryName_Difficulty);
        else
            success = false;

        if (PlayerPrefs.HasKey(index + PrefEntryName_TimeTaken))
            check.timeTaken = PlayerPrefs.GetFloat(index + PrefEntryName_TimeTaken);
        else
            success = false;

        if (PlayerPrefs.HasKey(index + PrefEntryName_Kills))
            check.kills = PlayerPrefs.GetInt(index + PrefEntryName_Kills);
        else
            success = false;

        if (CheckDateTime(index))
            check.entryIRL_Time = GetDateTime(index);
        else
            success = false;

        check.isLast = false;
        return success;
    }

    public static void Write(ScoreEntry entry, int index)
    {
        PlayerPrefs.SetInt(index + PrefEntryName_Score, entry.score);
        PlayerPrefs.SetString(index + PrefEntryName_Name, entry.name);
        PlayerPrefs.SetInt(index + PrefEntryName_Difficulty, (int)entry.difficulty);
        PlayerPrefs.SetFloat(index + PrefEntryName_TimeTaken, entry.timeTaken);
        PlayerPrefs.SetInt(index + PrefEntryName_Kills, entry.kills);

        int left, right;
        GetTimeInts(entry.entryIRL_Time, out left, out right);
        PlayerPrefs.SetInt(index + PrefEntryName_IRLTime_left, left);
        PlayerPrefs.SetInt(index + PrefEntryName_IRLTime_right, right);
    }

    public static ScoreEntry Read(int index)
    {
        ScoreEntry se = new ScoreEntry();
        if (Exists(index, out se))
            return se;
        else
        {
            return error;
        }
    }

    public static void GetTimeInts(System.DateTime dateTime, out int left, out int right)
    {
        dateTime.Ticks.Split(out left, out right);
    }

    public static bool CheckDateTime(int index)
    {
        return PlayerPrefs.HasKey(index + PrefEntryName_IRLTime_left) && PlayerPrefs.HasKey(index + PrefEntryName_IRLTime_right);
    }

    public static System.DateTime GetDateTime(int index)
    {
        int left = PlayerPrefs.GetInt(index + PrefEntryName_IRLTime_left);
        int right = PlayerPrefs.GetInt(index + PrefEntryName_IRLTime_right);
        long dt = Tools.Tools.MakeLong(left, right);
        System.DateTime dateTime = System.DateTime.FromBinary(dt);
        return dateTime;
    }

    public int CompareTo(ScoreEntry other)
    {
        int comp = score.CompareTo(other.score);
        switch (comp)
        {
            case 0:
                comp = name.CompareTo(other.name);
                switch (comp)
                {
                    case 0:
                        comp = difficulty.CompareTo(other.difficulty);
                        switch (comp)
                        {
                            case 0:
                                comp = kills.CompareTo(other.kills);
                                switch (comp)
                                {
                                    case 0:
                                        comp = -timeTaken.CompareTo(other.timeTaken);
                                        switch (comp)
                                        {
                                            case 0: // basically impossible
                                                if (Tools.Tools.Coinflip())
                                                    return 1;
                                                return -1;

                                            default:
                                                return -timeTaken.CompareTo(other.timeTaken); // more time -> higher up
                                        }

                                    default:
                                        return -comp; // more kills -> higher up
                                }
                            default:
                                return comp; // higher difficulty -> higher up
                        }
                    default:
                        return comp; // earlyer name -> higher up
                }
            default:
                return -comp; // higher score -> higher up
        }
    }

    public bool Equals(ScoreEntry other)
    {
        return score == other.score && name == other.name && difficulty == other.difficulty && timeTaken == other.timeTaken && kills == other.kills && entryIRL_Time == other.entryIRL_Time;
    }
}