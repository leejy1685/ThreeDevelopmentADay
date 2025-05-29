using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPower = 50f; // 점프 파워 

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 기존 속도 무시하고 위로 점프
                rb.velocity = Vector3.zero; // 이전 힘 제거
                rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            }
        }
    }
}


 