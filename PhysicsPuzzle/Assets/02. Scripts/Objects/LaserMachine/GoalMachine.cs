using System;
using UnityEngine;


using System.Collections.Generic;
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
        [SerializeField] private List<Texture2D> textures;
        [SerializeField] private List<Texture2D> lightMaps;
        [SerializeField] private SerializedDictionary<LASER_COLOR, Texture2D> textureDictionary;
        [SerializeField] private SerializedDictionary<LASER_COLOR, Texture2D> lightMapDictionary;

        private float _lastHitTime;
        private float _emissionDuration = 0.05f;
        private GameManager gameManager;
        
        public bool IsActivate => isActivate;

        private void Awake()
        {
            var i = 0;
            foreach (var texture in textures) textureDictionary.TryAdd((LASER_COLOR)i++, texture);
            i = 0;
            foreach (var lightMap in lightMaps) lightMapDictionary.TryAdd((LASER_COLOR)i++, lightMap);
        }

        private void Start()
        {
            gameManager = GameManager.Instance;

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
                //if(gameManager.CheckLastPuzzle())
                //    gameManager.Puzzle.ClearPuzzle();
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
                render.material.mainTexture = textureDictionary[color];
                render.material.SetTexture("_EmissionMap", lightMapDictionary[color]);
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
