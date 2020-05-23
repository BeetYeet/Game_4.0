using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private string playSceneName = "MainScene";

    [SerializeField]
    private string HighscoreSceneName = "Highscores Scene";

    [SerializeField]
    private RectTransform settings = null;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayPressed);
        scoresButton.onClick.AddListener(OnScoresPressed);
        settingsButton.onClick.AddListener(OnSettingsPressed);
        exitButton.onClick.AddListener(OnExitPressed);
    }

    private void OnPlayPressed()
    {
        SceneHandler.instance.LoadScene(playSceneName);
    }

    private void OnScoresPressed()
    {
        SceneHandler.instance.LoadScene(HighscoreSceneName);
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