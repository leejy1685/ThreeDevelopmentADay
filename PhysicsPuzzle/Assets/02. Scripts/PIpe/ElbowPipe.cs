using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElbowPipe : Pipe
{  
    public override void OnInteract()
    {
        if (!isRotating)
            StartCoroutine(RotateAroundPivot(Vector3.forward));
    }

    private IEnumerator RotateAroundPivot(Vector3 axis)
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(axis * rotationAngle);

        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }
}
