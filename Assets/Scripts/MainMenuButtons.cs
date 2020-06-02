using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField]
    private Button playButton = null;

    [SerializeField]
    private Button scoresButton = null;

    [SerializeField]
    private Button settingsButton = null;

    [SerializeField]
    private Button exitButton = null;

    [SerializeField]
    private Button instructionsButton = null;

    [SerializeField]
    private string playSceneName = "MainScene";

    [SerializeField]
    private string highscoreSceneName = "Highscores Scene";

    [SerializeField]
    private string instructionsSceneName = "Instrucions Scene";

    [SerializeField]
    private RectTransform settings = null;

    private void Awake()
    {
        StartCoroutine(SetOnHover());
    }

    private IEnumerator SetOnHover()
    {
        yield return null;
        yield return null;
        EventTrigger et = playButton.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener(delegate { MenuHover(); });
        et.triggers.Add(entry);
    }

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayPressed);
        scoresButton.onClick.AddListener(OnScoresPressed);
        settingsButton.onClick.AddListener(OnSettingsPressed);
        exitButton.onClick.AddListener(OnExitPressed);
        instructionsButton.onClick.AddListener(OnInstructionsPressed);
    }

    public void OnInstructionsPressed()
    {
        SceneHandler.instance.LoadScene(instructionsSceneName);
    }

    public void MenuHover()
    {
        AudioHandler.Click2();
    }

    public void MenuClick()
    {
        AudioHandler.Click();
    }

    private void OnPlayPressed()
    {
        SceneHandler.instance.LoadScene(playSceneName);
    }

    private void OnScoresPressed()
    {
        SceneHandler.instance.LoadScene(highscoreSceneName);
    }

    private void OnSettingsPressed()
    {
        settings.gameObject.SetActive(!settings.gameObject.activeSelf);
    }

    private void OnExitPressed()
    {
        SceneHandler.instance.transition.StartTransitionOut();
        SceneHandler.instance.transition.OnTransitionOutEnd += delegate { Application.Quit(); };
        SceneHandler.instance.Hide();
    }
}