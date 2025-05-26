using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;           // 사운드 식별용 이름
    public AudioClip clip;           // 재생할 오디오 클립
    [Range(0f, 1f)]
    public float volume = 1f;    // 기본 볼륨(0~1)
    public bool loop = false;   // 루프 여부
    public AudioMixerGroup mixerGroup;     // 할당할 AudioMixerGroup

    [HideInInspector]
    public AudioSource source;         // 런타임에 생성할 AudioSource
}
