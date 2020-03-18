using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{
    public LineRenderer lr;
    public Transform shootPoint;
    public bool isShooting = false;
    public LayerMask layerMask;

    private void Start()
    {
        bool enabled = true;
        if (lr == null)
        {
            Debug.LogError($"Unassigned line renderer!\nat {gameObject.name}");
            enabled = false;
        }
        if (shootPoint == null)
        {
            Debug.LogError($"Unassigned shoot point!\nat {gameObject.name}");
            enabled = false;
        }
        if (layerMask == 0x0)
        {
            Debug.LogError($"Layermask excludes everything!\nat {gameObject.name}");
            enabled = false;
        }
        this.enabled = enabled;
    }

    private void Update()
    {
        if (isShooting)
        {
            RaycastHit hit;
            if (!Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, 1000f, layerMask))
            {
                Debug.LogWarning($"Shoot laser cant find a wall, double check terrain and layermask!");
                return;
            }

            lr.enabled = true;
            lr.SetPositions(new Vector3[2] { shootPoint.position, hit.point });
        }
        else
        {
            lr.enabled = false;
        }
    }
}