using System.Collections;
using UnityEngine;
using CollisionBlock = _02._Scripts.DayAndNight.CollisionBlock;
using VisibleBlock = _02._Scripts.DayAndNight.VisibleBlock;

namespace _02._Scripts.Environment
{
    //낮과 밤
    public enum TIME_TYPE
    {
        Day,
        Night,
    }

    public class DayAndNight : MonoBehaviour
    {
        // 하루 시간을 0 ~ 1로 치환
        // 길이를 늘리고 싶으면 fullDayLength를 조절
        [Header("Time Settings")] 
        [Range(0.0f, 1.0f)] [SerializeField] private float time;
        [SerializeField] private float fullDayLength;
        [SerializeField] private float startTime = 0.4f;
        [SerializeField] private Vector3 noon;
        private float _timeRate;

        [Header("Sun")] 
        [SerializeField] private Light sun;
        [SerializeField] private Gradient sunColor;
        [SerializeField] private AnimationCurve sunIntensity;
        private Coroutine _changeDayCoroutine;

        [Header("Moon")] 
        [SerializeField] private Light moon1;
        [SerializeField] private Light moon2;
        [SerializeField] private Gradient moonColor;
        [SerializeField] private AnimationCurve moonIntensity;
        private Coroutine _changeNightCoroutine;

        [Header("Other Lighting")] 
        [SerializeField] private AnimationCurve lightingIntensityMultiplier;
        [SerializeField] private AnimationCurve reflectionIntensityMultiplier;

        [Header("Transition Blocks")] 
        public TIME_TYPE currentTime;
        public VisibleBlock[] visibleBlocks;
        public CollisionBlock[] collisionBlocks;

        private void Awake()
        {
            visibleBlocks = FindObjectsByType<VisibleBlock>(FindObjectsSortMode.None);
            collisionBlocks = FindObjectsByType<CollisionBlock>(FindObjectsSortMode.None);
        }

        private void Start()
        {
            _timeRate = 1.0f / fullDayLength;
            time = startTime;
            currentTime = TIME_TYPE.Day;
            
            ChangeBlocks();
        }

        public void ChangeDayAndNight()
        {
            if (TIME_TYPE.Night == currentTime)
            {
                if (_changeDayCoroutine != null) StopCoroutine(_changeDayCoroutine);
                _changeDayCoroutine = StartCoroutine(ChangeDay_Coroutine());
            }
            else
            {
                if (_changeNightCoroutine != null) StopCoroutine(_changeNightCoroutine);
                _changeNightCoroutine = StartCoroutine(ChangeNight_Coroutine());
            }
        }

        private IEnumerator ChangeNight_Coroutine()
        {
            while (true)
            {
                EvaluateDayAndNight();

                //밤이 되면 탈출
                if (Mathf.Abs(time) < 0.01f)
                {
                    currentTime = TIME_TYPE.Night;
                    ChangeBlocks();
                    break;
                }
                yield return null;
            }
        }

        private IEnumerator ChangeDay_Coroutine()
        {
            while (true)
            {
                EvaluateDayAndNight();

                //낮이 되면 탈출
                if (Mathf.Abs(time - 0.5f) < 0.01f)
                {
                    currentTime = TIME_TYPE.Day;
                    ChangeBlocks();
                    break;
                }

                yield return null;
            }
        }

        private void EvaluateDayAndNight()
        {
            //시간 계산
            time = (time + _timeRate * Time.deltaTime) % 1.0f;

            //태양 위치 변경
            sun.transform.eulerAngles = (time - 0.25f) * 4.0f * noon;
            sun.color = sunColor.Evaluate(time);
            sun.intensity = sunIntensity.Evaluate(time);

            //달 밝기 변경
            moon1.color = moonColor.Evaluate(time);
            moon1.intensity = moonIntensity.Evaluate(time);

            moon2.color = moonColor.Evaluate(time);
            moon2.intensity = moonIntensity.Evaluate(time);

            RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
            RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
        }

        private void ChangeBlocks()
        {
            foreach (var visibleBlock in visibleBlocks)
            {
                visibleBlock.ChangeVisibleBlock(currentTime);
            }

            foreach (var collisionBlock in collisionBlocks)
            {
                collisionBlock.ChangeSetActive(currentTime);
            }
        }
    }
}