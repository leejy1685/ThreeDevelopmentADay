using UnityEngine;

public class CrossPipe : Pipe
{
    
    public override void OnInteract()
    {
        if (!isRotating)
            StartCoroutine(RotateAroundPivot(Vector3.up));
    }
}
