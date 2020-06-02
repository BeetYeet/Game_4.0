using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    private MyInputSystem input;

    private void Awake()
    {
        input = new MyInputSystem();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(GoBack);
        button.onClick.AddListener(AudioHandler.Click);
        input.PlayerActionControlls.Menu.performed += delegate { ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler); };
    }

    private void GoBack()
    {
        SceneHandler.instance.LoadScene("Main Menu");
    }
}