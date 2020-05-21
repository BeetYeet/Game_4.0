using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGroundPlane : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<ParticleSystem>().collision.SetPlane(0, GameObject.FindGameObjectWithTag("Ground").transform);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}