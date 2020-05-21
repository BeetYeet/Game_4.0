using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;

    public Transition transition;

    private AsyncOperation loadCallback;

    private bool transitioningOut = false;
    private bool transitioningIn = false;

    [SerializeField]
    private Slider progressSlider;

    [SerializeField]
    private TextMeshProUGUI progressText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneAsync(name));
    }

    private IEnumerator LoadSceneAsync(string name)
    {
        // start loading
        loadCallback = SceneManager.LoadSceneAsync(name);
        // dont finish tho
        loadCallback.allowSceneActivation = false;
        // start the transition
        transition.OnTransitionOutEnd += TransitionOutDone;
        transitioningOut = true;
        transition.StartTransitionOut();
        Show();
        while (loadCallback.progress < .9f)
        {
            // update progress
            UpdateProgress(loadCallback.progress / .9f);
            yield return null;
        }
        // we done
        progressSlider.value = 1f;
        progressText.text = "Loading: Done!";

        while (transitioningOut)
        {
            // wait for the transition to finish
            Debug.Log("Waiting for transition");
            yield return null;
        }
        // go into new scene
        loadCallback.allowSceneActivation = true;
        yield return null;

        // ok fade into new scene
        transition.OnTransitionInEnd += TransitionInDone;
        transitioningIn = true;
        transition.StartTransitionIn();
    }

    public void Hide()
    {
        progressSlider.gameObject.SetActive(false);
        progressText.text = "";
    }

    public void Show()
    {
        progressSlider.gameObject.SetActive(true);
        progressText.text = "";
    }

    private void TransitionOutDone()
    {
        transition.OnTransitionOutEnd -= TransitionOutDone;
        transitioningOut = false;
    }

    private void TransitionInDone()
    {
        // TODO: start scene
        transition.OnTransitionInEnd -= TransitionInDone;
        transitioningIn = false;
    }

    private void UpdateProgress(float val)
    {
        progressSlider.value = val;
        progressText.text = $"Loading: {Mathf.CeilToInt(val * 100f)}%";
    }
}