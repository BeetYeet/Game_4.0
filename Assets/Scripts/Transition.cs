using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public event System.Action OnTransitionOutEnd;

    public event System.Action OnTransitionInEnd;

    public void StartTransitionOut()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            TransitionOutDone();
            return;
        }
        animator.SetTrigger("TransitionOut");
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "StartScene")
        {
            StartTransitionIn();
        }
    }

    public void StartTransitionIn()
    {
        animator.SetTrigger("TransitionIn");
    }

    private void TransitionInDone()
    {
        OnTransitionInEnd?.Invoke();
    }

    private void TransitionOutDone()
    {
        OnTransitionOutEnd?.Invoke();
    }
}