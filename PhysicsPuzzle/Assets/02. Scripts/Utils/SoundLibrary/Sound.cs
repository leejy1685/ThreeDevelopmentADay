using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;           // ���� �ĺ��� �̸�
    public AudioClip clip;           // ����� ����� Ŭ��
    [Range(0f, 1f)]
    public float volume = 1f;    // �⺻ ����(0~1)
    public bool loop = false;   // ���� ����
    public AudioMixerGroup mixerGroup;     // �Ҵ��� AudioMixerGroup

    [HideInInspector]
    public AudioSource source;         // ��Ÿ�ӿ� ������ AudioSource
}
