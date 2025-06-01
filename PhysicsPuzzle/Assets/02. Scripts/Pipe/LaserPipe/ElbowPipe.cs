using UnityEngine;

public class ElbowPipe : Pipe
{  
    public override void OnInteract()
    {
        if (!isRotating)
            StartCoroutine(RotateAroundPivot(Vector3.forward));
    }
}
