using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Tools;

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
    private Button deleteHighscoresButton;

    [SerializeField]
    private Button menuButton;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private TextMeshProUGUI deleteHighscoresText;

    [SerializeField]
    private GameObject playingUI;

    [SerializeField]
    private GameObject nameSubmitUI;

    [SerializeField]
    private CanvasGroup nameSubmitCanvasGroup;

    [SerializeField]
    private GameObject highscoresUI;

    [SerializeField]
    private Transform highscoresArea;

    [SerializeField]
    private GameObject highscoreEntryPrefab;

    [SerializeField]
    private GameObject deathVideo;

    public int Score { get { return Mathf.FloorToInt(score); } }

    [SerializeField]
    private float score = 0f;

    public bool playing = true;
    private string playerName;
    private float time;
    public int kills = 0;

    public List<ScoreEntry> highscores;
    private int numHighscores;

    private bool isSureAboutClear = false;
    private bool isClearing = false;

    [SerializeField]
    private string menuName = "Main Menu";

    [SerializeField]
    private bool debugMode = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        nameConfirmButton.onClick.AddListener(delegate { OnSubmit(nameInput.text); });
        nameInput.onEndEdit.AddListener(OnSubmit);

        nameInput.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
        nameInput.onValidateInput += ValidateName;

        nameDiscardButton.onClick.AddListener(OnDiscard);

        deleteHighscoresButton.onClick.AddListener(delegate { CheckReset(); });

        restartButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
        menuButton.onClick.AddListener(delegate { SceneHandler.instance.LoadScene(menuName); });
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

    private void CheckReset()
    {
        if (!isClearing)
        {
            if (!isSureAboutClear)
            {
                isSureAboutClear = true;
                deleteHighscoresText.text = "Are you sure?";
            }
            else
            {
                isSureAboutClear = false;
                deleteHighscoresText.text = "Click to cancel";
                ResetHighscores();
                deleteHighscoresButton.onClick.RemoveListener(delegate { CheckReset(); });
            }
        }
        else
        {
            isClearing = false;
            isSureAboutClear = false;
            deleteHighscoresText.text = "Canceled";
        }
    }

    private void ResetHighscores()
    {
        StartCoroutine(RemoveHighscores());
    }

    private IEnumerator RemoveHighscores()
    {
        isClearing = true;
        float time = 0f;
        while (highscoresArea.childCount > 0 && isClearing)
        {
            while (time > 0)
            {
                yield return null;
                time -= Time.deltaTime;
            }
            Destroy(highscoresArea.GetChild(0).gameObject);
            SetScrollSize();
            time += 0.1f;
        }
        if (isClearing)
        {
            isClearing = false;
            PlayerPrefs.DeleteAll();
            deleteHighscoresText.text = "Cleared";
            yield return new WaitForSeconds(3.5f);
            deleteHighscoresText.text = "Clear Highscores";
        }
        else
        {
            for (int i = 0; i < highscoresArea.childCount; i++)
            {
                Destroy(highscoresArea.GetChild(i).gameObject);
            }
            DisplayHighscores();
            yield return new WaitForSeconds(3.5f);
            deleteHighscoresText.text = "Clear Highscores";
        }
    }

    public void OnEnemyDie()
    {
        kills++;
        score += 100f;
    }

    public void OnRunEnd()
    {
        // do death things here
        playing = false;
        if (!debugMode)
        {
            deathVideo = Instantiate(deathVideo);
            StartCoroutine(FadeInScoreUI());
        }
        ShowNameSelect();
    }

    private void ShowNameSelect()
    {
        GetScores();
        time = Time.timeSinceLevelLoad;
        playingUI.SetActive(false);
        nameSubmitUI.SetActive(true);
    }

    private void OnSubmit(string name)
    {
        if (name == "" || name == null)
            return;
        playerName = name;
        AddScore(ScoreEntry.New(Score, playerName, time, kills));

        DisplayHighscores();
        ResetLastTracker();
    }

    private void OnDiscard()
    {
        DisplayHighscores();
    }

    private void ResetLastTracker()
    {
        highscores.ForEach(x => x.isLast = false);
    }

    private void DisplayHighscores()
    {
        nameSubmitUI.SetActive(false);
        highscoresUI.SetActive(true);

        highscores.Sort();

        for (int i = 0; i < highscores.Count; i++)
        {
            InstantiateEntry(i);
        }
        SetScrollSize();
    }

    private void SetScrollSize()
    {
        highscoresArea.GetComponent<RectTransform>().sizeDelta = new Vector2(highscoresArea.GetComponent<RectTransform>().sizeDelta.x, highscoreEntryPrefab.GetComponent<RectTransform>().rect.height * highscoresArea.childCount - highscoresArea.GetComponent<RectTransform>().rect.height);
    }

    private void InstantiateEntry(int index)
    {
        ScoreEntry hs = highscores[index];
        GameObject go = Instantiate(highscoreEntryPrefab, highscoresArea);
        go.GetComponent<HighscoreEntry>().Initialize(hs, index + 1);
    }

    private void GetScores()
    {
        numHighscores = PlayerPrefs.GetInt("numHighscores", 0);
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
        highscores.Add(score);
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

public struct ScoreEntry : IComparable<ScoreEntry>
{
    #region constants

    private const string PrefEntryName_Score = "_score";
    private const string PrefEntryName_Name = "_name";
    private const string PrefEntryName_TimeTaken = "_timeTaken";
    private const string PrefEntryName_Kills = "_kills";
    private const string PrefEntryName_IRLTime_left = "_IRLTime_left";
    private const string PrefEntryName_IRLTime_right = "_IRLTime_right";
    private static readonly ScoreEntry error = new ScoreEntry(0, "LD_ERR", 0f, 0, backup, false);

    #endregion constants

    public int score;
    public string name;
    public float timeTaken;
    public int kills;
    public DateTime entryIRL_Time;
    public bool isLast;

    private static DateTime backup = new DateTime(1945, 4, 30, 15, 30, 0, 7, DateTimeKind.Local);

    public static ScoreEntry New(int score, string name, float timeTaken, int kills)
    {
        ScoreEntry se = new ScoreEntry();
        se.score = score;
        se.name = name;
        se.timeTaken = timeTaken;
        se.kills = kills;
        se.entryIRL_Time = DateTime.Now;
        se.isLast = true;
        return se;
    }

    public ScoreEntry(int score, string name, float timeTaken, int kills, DateTime entryIRL_Time, bool isLast)
    {
        this.score = score;
        this.name = name;
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

    public static void GetTimeInts(DateTime dateTime, out int left, out int right)
    {
        dateTime.Ticks.Split(out left, out right);
    }

    public static bool CheckDateTime(int index)
    {
        return PlayerPrefs.HasKey(index + PrefEntryName_IRLTime_left) && PlayerPrefs.HasKey(index + PrefEntryName_IRLTime_right);
    }

    public static DateTime GetDateTime(int index)
    {
        int left = PlayerPrefs.GetInt(index + PrefEntryName_IRLTime_left);
        int right = PlayerPrefs.GetInt(index + PrefEntryName_IRLTime_right);
        long dt = Tools.Tools.MakeLong(left, right);
        DateTime dateTime = DateTime.FromBinary(dt);
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
                        return comp; // earlyer name -> higher up
                }
            default:
                return -comp; // higher score -> higher up
        }
    }
}