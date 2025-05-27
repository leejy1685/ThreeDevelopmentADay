using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GoalMachine : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private bool _isActivate;
    [SerializeField] private LAZER_COLOR _color;

    void Awake()
    {
        _isActivate = false;
        _color = LAZER_COLOR.Blue;
        // 접근 시 자동으로 머티리얼 인스턴스를 복제
        _renderer.material.EnableKeyword("_EMISSION");
    }


    public void CheckActive(LAZER_COLOR color)
    {
        if (color == _color)
        {
            OnLight();
        }
        else
        {
            OffLight();
        }
    }

    private void OnLight()
    {
        Debug.Log("불켜짐");
        _isActivate = true;
        SetEmissionColor(Color.white);
    }
    private void OffLight()
    {
        Debug.Log("불꺼짐");
        _isActivate = false;
        SetEmissionColor(Color.black);
    }
    private void SetEmissionColor(Color color)
    {
        var mat = _renderer.material; 
        mat.SetColor("_EmissionColor", color);
    }
}
