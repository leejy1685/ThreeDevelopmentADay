using System.Collections.Generic;
using _02._Scripts.Objects.LaserMachine;
using _02._Scripts.Utils;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _02._Scripts.Pipe.LinkedPipe
{
    public class LinkedPipe : MonoBehaviour
    {
        private readonly int _baseMap = Shader.PropertyToID("_BaseMap");
    
        [Header("Color Materials")]
        [SerializeField] private List<Material> materials;
        [SerializeField] private SerializedDictionary<LASER_COLOR, Material> materialDictionary;

        // 해당 파이프의 포트들, inspector 창에서 직접 연결
        [Header("Pipe Ports")]
        [SerializeField] public LinkedPipePort[] ports;
    
        [Header("Pipe Renderer")]
        [SerializeField] private Renderer render;
        [SerializeField] public LASER_COLOR receivedColor;
    
        private void Awake()
        {
            if (!render) render = Helper.GetComponent_Helper<Renderer>(gameObject);
            var i = 0;
            foreach(var material in materials) materialDictionary.TryAdd((LASER_COLOR)i++, material);
        }

        public void SetPipeColor(LASER_COLOR color)
        {
            receivedColor = color;
            if (!render) return;

            var material = materialDictionary[color];
            render.material = material;

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
}