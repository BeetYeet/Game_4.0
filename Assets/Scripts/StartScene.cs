﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    private void Start()
    {
        SceneHandler.instance.LoadScene("Main Menu", true);
    }
}