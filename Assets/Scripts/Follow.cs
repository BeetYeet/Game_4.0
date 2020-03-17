using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	public Transform target;
	public Vector3 offset;
	public float follow = 0.5f;
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Vector3 targetPos = target.position + offset;
		transform.position = Vector3.Lerp(transform.position, targetPos, 1 - Mathf.Exp(-follow * Time.deltaTime));
	}
}
