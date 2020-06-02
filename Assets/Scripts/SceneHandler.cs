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

    public int haltTransitionIn = 0;
    public float externalProgress = 0f;

    [SerializeField]
    private Slider progressSlider;

    [SerializeField]
    private TextMeshProUGUI progressText;

    private const bool debug = false;

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
        LoadScene(name, false);
    }

    private bool isLoading = false;

    public void LoadScene(string name, bool hideLoad)
    {
        if (isLoading)
            return;
        isLoading = true;
        StartCoroutine(LoadSceneAsync(name, hideLoad));
    }

    private IEnumerator LoadSceneAsync(string name, bool hideLoad)
    {
        // start loading
        loadCallback = SceneManager.LoadSceneAsync(name);
        // dont finish tho
        loadCallback.allowSceneActivation = false;
        // start the transition
        transition.OnTransitionOutEnd += TransitionOutDone;
        transitioningOut = true;
        transition.StartTransitionOut();
        if (!hideLoad)
        {
            Show();
            while (loadCallback.progress < .9f)
            {
                // update progress
                UpdateProgress(loadCallback.progress / 9f);
                if (debug)
                    Debug.Log("Waiting for Load");
                yield return null;
            }
        }
        else
        {
            Hide();
            while (loadCallback.progress < .9f)
            {
                // wait for completion
                if (debug)
                    Debug.Log("Waiting for Load");
                yield return null;
            }
        }

        if (debug)
            Debug.Log("Loading Done!");

        while (transitioningOut)
        {
            // wait for the transition to finish
            if (debug)
                Debug.Log("Waiting for transition");
            yield return null;
        }

        externalProgress = 0f;

        // go into new scene
        loadCallback.allowSceneActivation = true;
        if (debug)
            Debug.Log("Switching Scene");
        yield return null;

        while (!loadCallback.isDone)
        {
            // wait for scene to fully load
            yield return null;
        }
        yield return null;

        while (haltTransitionIn > 0)
        {
            // update progress
            UpdateProgress(.1f + externalProgress * .9f);
            if (debug)
                Debug.Log("Waiting for Load");
            yield return null;
        }

        // we done
        UpdateProgressDone();

        if (debug)
            Debug.Log("Transitioning in");
        // ok fade into new scene
        transition.OnTransitionInEnd += TransitionInDone;
        transitioningIn = true;
        transition.StartTransitionIn();
        isLoading = false;
    }

    public void UpdateProgressDone()
    {
        progressSlider.value = 1f;
        progressText.text = "Loading: Done!";
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

    public void UpdateProgress(float val)
    {
        progressSlider.value = val;
        progressText.text = $"Loading: {Mathf.CeilToInt(val * 100f)}%";
    }
}