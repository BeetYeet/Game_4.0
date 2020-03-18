using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator anim;
    public bool isShooting;
    public SpeedState state;

    public enum SpeedState { Idle, Walking, Running }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Shooting", isShooting);
        anim.SetInteger("MoveState", (int)state);
    }
}