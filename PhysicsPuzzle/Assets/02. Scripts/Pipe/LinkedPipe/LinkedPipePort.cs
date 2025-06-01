using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.Utils;
using UnityEngine;

namespace _02._Scripts.Pipe.LinkedPipe
{
    public class LinkedPipePort : MonoBehaviour
    {
        public LinkedPipe parentPipe;

        private void Awake()
        {
            parentPipe = GetComponentInParent<LinkedPipe>();
            if (!parentPipe) Debug.LogError($"{name} → parentPipe is NULL!");
        }

        public bool IsConnectedToLaser(out LASER_COLOR color)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 0.2f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("LaserBody"))
                {
                    var laser = Helper.GetComponent_Helper<LaserMachine>(hit.gameObject);
                    if (laser)
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
}
