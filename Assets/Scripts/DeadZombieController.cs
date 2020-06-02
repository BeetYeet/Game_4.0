using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZombieController : MonoBehaviour
{
    [SerializeField]
    private List<ZombiePartExplode> parts = new List<ZombiePartExplode>();

    public void Initialize(float force)
    {
        parts.ForEach(x =>
        {
            x.velocity *= force;
        });
    }
}