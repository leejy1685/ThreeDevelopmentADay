using _02._Scripts.Objects.LaserMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedPipe : MonoBehaviour
{
    [Header("Color Textures")]
    [SerializeField] private Texture2D _whiteTex;
    [SerializeField] private Texture2D _redTex;
    [SerializeField] private Texture2D _blueTex;
    [SerializeField] private Texture2D _greenTex;
    [SerializeField] private Texture2D _purpleTex;
    [SerializeField] private Texture2D _yellowTex;

    // 해당 파이프의 포트들, inspector 창에서 직접 연결
    [SerializeField] public LinkedPipePort[] ports;


    [SerializeField] private Renderer _renderer;
    [SerializeField] public LASER_COLOR _receivedColor;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
            Debug.LogError($"{name}의 Renderer가 연결되지 않음");
    }

    public void SetPipeColor(LASER_COLOR color)
    {
        _receivedColor = color;
        if (_renderer == null) return;

        Texture2D texture = color switch
        {
            LASER_COLOR.White => _whiteTex,
            LASER_COLOR.Blue => _blueTex,
            LASER_COLOR.Green => _greenTex,
            LASER_COLOR.Purple => _purpleTex,
            LASER_COLOR.Red => _redTex,
            LASER_COLOR.Yellow => _yellowTex,
            _ => _whiteTex
        };

        Material mat = _renderer.material;
        mat.EnableKeyword("_EMISSION");
        mat.SetTexture("_BaseMap", texture);

        //Debug.Log($"{name} 색 변경: {_receivedColor}");
    }

    public void PropagateColor(LASER_COLOR color, HashSet<LinkedPipe> visited)
    {
        if (visited.Contains(this)) return;

        visited.Add(this);
        SetPipeColor(color);

        foreach (var port in ports)
        {
            Collider[] hits = Physics.OverlapSphere(port.transform.position, 0.2f);
            foreach (var hit in hits)
            {
                var otherPort = hit.GetComponent<LinkedPipePort>();
                if (otherPort == null) continue;

                var nextPipe = otherPort.parentPipe;
                if (nextPipe != null && nextPipe != this)
                {
                    nextPipe.PropagateColor(color, visited);
                }
            }
        }
    }

    public bool HasDirectLaser()
    {
        foreach (var port in ports)
        {
            Collider[] hits = Physics.OverlapSphere(port.transform.position, 0.2f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("LaserBody"))
                    return true;
            }
        }
        return false;
    }
}