using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	Animator anim;
	public bool shooting;
	public SpeedState state;
	public enum SpeedState { Idle, Walking, Running }
	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		anim.SetBool("Shooting", shooting);
		anim.SetInteger("MoveState", (int)state);
	}
}
