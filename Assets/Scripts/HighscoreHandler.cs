﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class HighscoreHandler : MonoBehaviour
{
    public static HighscoreHandler instance = null;

    [SerializeField]
    private Button menuButton;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button deleteHighscoresButton;

    [SerializeField]
    private TextMeshProUGUI deleteHighscoresText;

    [SerializeField]
    private Transform highscoresArea;

    [SerializeField]
    private ScrollRect highscoresScrollController;

    [SerializeField]
    private RectTransform highscoresScrollViewport;

    [SerializeField]
    private GameObject highscoreEntryPrefab;

    private bool isSureAboutClear = false;
    private bool isClearing = false;

    [SerializeField]
    private string menuName = "Main Menu";

    [SerializeField]
    private bool isHighscoreScene = false;

    private MyInputSystem input = null;
    private Vector2 move = new Vector2();

    [SerializeField]
    private float scrollSpeed = 10f;

    public HighscoreEntry newest;

    private void Awake()
    {
        instance = this;

        input = new MyInputSystem();
        if (isHighscoreScene)
        {
            restartButton.gameObject.SetActive(false);
        }
        else
        {
            restartButton.onClick.AddListener(RestartLevel);
            restartButton.onClick.AddListener(AudioHandler.Click);
            input.PlayerActionControlls.Restart.performed += delegate { Press(restartButton); };
        }
        menuButton.onClick.AddListener(LoadMenu);
        menuButton.onClick.AddListener(AudioHandler.Click);

        input.PlayerActionControlls.Move.performed += Move_performed;
        input.PlayerActionControlls.Menu.performed += delegate { Press(menuButton); };

        if (SettingsHandler.cache.highscoreDeletable)
        {
            deleteHighscoresButton.onClick.AddListener(CheckReset);
            deleteHighscoresButton.onClick.AddListener(AudioHandler.Click);
            input.PlayerActionControlls.DeleteHighscores.performed += delegate { Press(deleteHighscoresButton); };
        }
        else
        {
            deleteHighscoresButton.gameObject.SetActive(false);
        }
    }

    private void Press(Button button)
    {
        ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }

    private void Start()
    {
        if (isHighscoreScene)
        {
            StartCoroutine(HighscoreSceneWarmup());
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

    private void LoadMenu()
    {
        SceneHandler.instance.LoadScene(menuName);
    }

    private bool isRestarting = false;

    private void RestartLevel()
    {
        isRestarting = true;
        SceneHandler.instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        move = obj.ReadValue<Vector2>();
    }

    private IEnumerator HighscoreSceneWarmup()
    {
        SceneHandler.instance.haltTransitionIn++;
        while (GameController.instance.GetPartialScores())
        {
            yield return null;
        }
        GameController.instance.highscores.Sort();
        Enable();
        Display(-1);
    }

    private void Update()
    {
        if (move.y != 0f)
        {
            highscoresScrollController.verticalNormalizedPosition = Mathf.Clamp01(highscoresScrollController.verticalNormalizedPosition + move.y * Time.unscaledDeltaTime * scrollSpeed / highscoresArea.GetComponent<RectTransform>().rect.height);
        }
    }

    public void Enable()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Display(int newIndex)
    {
        StartCoroutine(DisplayAsync(newIndex));
    }

    private IEnumerator DisplayAsync(int newIndex)
    {
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < GameController.instance.highscores.Count; i++)
        {
            GameObject go = InstantiateEntry(GameController.instance.highscores[i], i);
            SceneHandler.instance.externalProgress = i / (float)GameController.instance.highscores.Count;
            if (newIndex == i)
            {
                newest = go.GetComponent<HighscoreEntry>();
                Debug.Log($"Newest is {go.name}");
            }
            yield return null;
        }
        stopwatch.Stop();
        Debug.Log($"Instantiate Score Entries took {stopwatch.ElapsedMilliseconds}ms");

        SetScrollSize();
        SceneHandler.instance.haltTransitionIn--;
    }

    private void CheckReset()
    {
        if (!isClearing)
        {
            if (!isSureAboutClear)
            {
                isSureAboutClear = true;
                deleteHighscoresText.text = "Are you sure?";
                StartCoroutine(ResetSure());
            }
            else
            {
                isSureAboutClear = false;
                deleteHighscoresText.text = "Click to cancel";
                ResetHighscores();
            }
        }
        else
        {
            isClearing = false;
            isSureAboutClear = false;
            deleteHighscoresText.text = "Canceled";
        }
    }

    private IEnumerator ResetSure()
    {
        yield return new WaitForSecondsRealtime(3.5f);
        if (isSureAboutClear)
        {
            isSureAboutClear = false;
            deleteHighscoresText.text = "Clear Highscores";
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
                time -= Time.unscaledDeltaTime;
            }
            Destroy(highscoresArea.GetChild(0).gameObject);
            SetScrollSize();
            time += 0.1f;
            yield return null;
        }
        if (isClearing)
        {
            isClearing = false;
            DeleteHighscores();
            deleteHighscoresText.text = "Cleared";
            yield return new WaitForSecondsRealtime(3.5f);
            deleteHighscoresText.text = "Clear Highscores";
        }
        else
        {
            for (int i = 0; i < highscoresArea.childCount; i++)
            {
                Destroy(highscoresArea.GetChild(i).gameObject);
            }
            Display(-1);
            yield return new WaitForSecondsRealtime(3.5f);
            deleteHighscoresText.text = "Clear Highscores";
        }
    }

    /*public void AddNew(ScoreEntry newScore)
    {
        int index = GameController.instance.highscores.FindIndex(new Predicate<ScoreEntry>((x) => { return x.Equals(newScore); }));
        GameObject go = InstantiateEntry(newScore, index);
        go.transform.SetSiblingIndex(index);

        SetScrollSize();
        SnapTo(go.GetComponent<RectTransform>());
    }*/

    private static void DeleteHighscores()
    {
        SettingsHandler.ReadToCache();
        PlayerPrefs.DeleteAll();
        SettingsHandler.WriteFromCache();
    }

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform rectTransform = highscoresArea.GetComponent<RectTransform>();
        rectTransform.anchoredPosition =
            (Vector2)highscoresScrollController.transform.InverseTransformPoint(rectTransform.position)
            - (Vector2)highscoresScrollController.transform.InverseTransformPoint(target.position)
            - Vector2.up * (highscoresScrollViewport.rect.height -
            target.rect.height / 2f);

        Debug.Log($"Snapped to GameObject: {target.name}");
    }

    private void SetScrollSize()
    {
        //highscoresArea.GetComponent<RectTransform>().sizeDelta = new Vector2(highscoresArea.GetComponent<RectTransform>().sizeDelta.x, highscoreEntryPrefab.GetComponent<RectTransform>().rect.height * highscoresArea.childCount);
    }

    private GameObject InstantiateEntry(ScoreEntry entry, int index)
    {
        GameObject go = Instantiate(highscoreEntryPrefab, highscoresArea);
        go.GetComponent<HighscoreEntry>().Initialize(entry, index + 1);
        return go;
    }
}