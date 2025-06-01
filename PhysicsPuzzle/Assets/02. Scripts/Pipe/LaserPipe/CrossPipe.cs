using UnityEngine;

namespace _02._Scripts.Pipe.LaserPipe
{
    public class CrossPipe : global::_02._Scripts.Pipe.LaserPipe.Pipe
    {
    
        public override void OnInteract()
        {
            base.OnInteract();
            if (!isRotating)
                StartCoroutine(RotateAroundPivot(Vector3.up));
        }
    }
}
