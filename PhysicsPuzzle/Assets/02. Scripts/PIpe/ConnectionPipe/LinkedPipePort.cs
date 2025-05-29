using _02._Scripts.Objects.LaserMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedPipePort : MonoBehaviour
{
    public LinkedPipe parentPipe;

    private void Awake()
    {
        parentPipe = GetComponentInParent<LinkedPipe>();
        if (parentPipe == null)
            Debug.LogError($"{name} → parentPipe is NULL!");
    }

    public bool IsConnectedToLaser(out LASER_COLOR color)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.2f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("LaserBody"))
            {
                var laser = hit.GetComponentInParent<LaserMachine>();
                if (laser != null)
                {
                    color = laser.laserColor;
                    return true;
                }
            }
        }
        color = LASER_COLOR.White;
        return false;
    }
}
