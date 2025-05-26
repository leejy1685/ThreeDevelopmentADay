using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaserMachine : MonoBehaviour
{
    [SerializeField] Transform _pillarAngle;
    [SerializeField] Transform _colorBottle;

    public LineRenderer lineRenderer;
    public float maxDistance = 100f;
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) // Space 누르면 발사
        {
            Ray ray = new Ray(_pillarAngle.position, transform.up);
            RaycastHit hit;

            Vector3 endPosition = _pillarAngle.position + transform.up * maxDistance;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                endPosition = hit.point;
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, endPosition);
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
