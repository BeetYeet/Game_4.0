using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button scoresButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private string playSceneName = "MainScene";

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayPressed);
        scoresButton.onClick.AddListener(OnScoresPressed);
        exitButton.onClick.AddListener(OnExitPressed);
    }

    private void OnPlayPressed()
    {
        SceneHandler.instance.LoadScene(playSceneName);
    }

    private void OnScoresPressed()
    {
        // TODO: highscores scene
    }

    private void OnExitPressed()
    {
        SceneHandler.instance.transition.StartTransitionOut();
        SceneHandler.instance.transition.OnTransitionOutEnd += delegate { Application.Quit(); };
        SceneHandler.instance.Hide();
    }
}