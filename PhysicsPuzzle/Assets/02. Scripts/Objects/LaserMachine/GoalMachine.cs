using System;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Rendering;

namespace _02._Scripts.Objects.LaserMachine
{
    [Serializable] 
    public class GoalMachine : MonoBehaviour, ILaserReceiver
    {
        [Header("Components ")]
        [SerializeField] private bool isActivate;
        [SerializeField] private LASER_COLOR mainColor;

        [Header("Body Textures")]
        [SerializeField] private List<Renderer> renderers;
        [SerializeField] private List<Material> materials;
        [SerializeField] private SerializedDictionary<LASER_COLOR, Material> materialDictionary;

        private float _lastHitTime;
        private float _emissionDuration = 0.05f;

        public bool IsActivate {  get { return isActivate; } }

        private void Awake()
        {
            var i = 0;
            foreach (var material in materials) materialDictionary.TryAdd((LASER_COLOR)i++, material);
        }

        private void Start()
        {
            SetTextureOfBody(mainColor);
            OffLight();
        }

        private void Update()
        {
            if (isActivate && Time.time - _lastHitTime > _emissionDuration)
            {
                _lastHitTime = Time.time;
                OffLight(); // 너무 오래 레이저가 안 들어오면 자동 off
            }
        }

        public void CheckActive(LASER_COLOR color)
        {
            if (color == mainColor)
            {
                _lastHitTime = Time.time;
                OnLight(); // 지속 갱신

            }
            else
            {
                OffLight();
            }
        }
    
        private void OnLight()
        {
            isActivate = true;
            SetEmissionColor(Color.white);
        }
        private void OffLight()
        {
            isActivate = false;
            SetEmissionColor(Color.black);
        }

        private void SetTextureOfBody(LASER_COLOR color)
        {
            foreach (var render in renderers)
            {
                render.material = materialDictionary[color];
            }
        }
        
        private void SetEmissionColor(Color color)
        {
            foreach (var render in renderers)
            {
                render.material.SetColor("_EmissionColor", color);
            }
        }

        public void OnLaserHit(LaserBeam beam)
        {
            CheckActive(beam.colorType);
        }
    }
}
