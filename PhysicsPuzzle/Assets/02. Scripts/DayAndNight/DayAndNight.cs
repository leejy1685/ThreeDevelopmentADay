using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    // 하루 시간을 0 ~ 1로 치환
    // 길이를 늘리고 싶으면 fullDayLength를 조절
    [Range(0.0f, 1.0f)] public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    [SerializeField] private Vector3 noon;

    [Header("Sun")]
    public Light sun;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private AnimationCurve sunIntensity;
    private Coroutine ChangeDayCoroutine;

    [Header("Moon")]
    public Light moon1;
    public Light moon2;
    [SerializeField] private Gradient moonColor;
    [SerializeField] private AnimationCurve moonIntensity;
    private Coroutine ChangeNightCoroutine;

    [Header("Other Lighting")]
    [SerializeField] private AnimationCurve lightingIntensityMultiplier;
    [SerializeField] private AnimationCurve reflectionIntensityMultiplier;
    
    [Header("[Blocks]")]
    public TIME_TYPE isDay;
    public TimeBlock[] timeBlocks;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
        isDay = TIME_TYPE.Day;
        timeBlocks = FindObjectsByType<TimeBlock>(FindObjectsSortMode.None);
    }
    
    private void Update()
    {
        //test code
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeDayAndNight();
        }
    }

    public void ChangeDayAndNight()
    {
        if (TIME_TYPE.Night == isDay)
        {
            ChangeDayCoroutine = StartCoroutine(ChangeDay_Coroutine());
        }
        else
        {
            ChangeNightCoroutine = StartCoroutine(ChangeNight_Coroutine());
        }
        
    }
    

    IEnumerator ChangeNight_Coroutine()
    {
        while (true)
        {
            EvaluateDayAndNight();

            //밤이 되면 탈출
            if (Mathf.Abs(time) < 0.01f)
            {
                isDay = TIME_TYPE.Night;
                
                for (int i = 0; i < timeBlocks.Length; i++)
                {
                    timeBlocks[i].ChangeTimeBlock(isDay);
                }
                
                break;
            }


            yield return null;
        }
    }
    
    IEnumerator ChangeDay_Coroutine()
    {
        while (true)
        {
            EvaluateDayAndNight();

            //낮이 되면 탈출
            if (Mathf.Abs(time - 0.5f) < 0.01f)
            {
                for (int i = 0; i < timeBlocks.Length; i++)
                {
                    timeBlocks[i].ChangeTimeBlock(isDay);
                }
                
                isDay = TIME_TYPE.Day;
                break;
            }
            
            yield return null;
        }
    }

    private void EvaluateDayAndNight()
    {
        //시간 계산
        time = (time + timeRate * Time.deltaTime) % 1.0f;

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
}
