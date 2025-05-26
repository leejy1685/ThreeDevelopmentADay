using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class SoundManager : Singleton<SoundManager>
{
   
    [Header("���� ���� ���̺귯��")]
    public List<SoundLibrary> libraries;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    // ��Ÿ�ӿ� ��� ���� ����
    private Sound[] currentSounds = new Sound[0];

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ���� �ε�� ������ ȣ��
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadLibraryForScene(scene.name);
    }

    // �ش� �� �̸��� �´� ���̺귯���� ã�� ����
    private void LoadLibraryForScene(string sceneName)
    {
        // ���� ���� ����
        foreach (var s in currentSounds)
        {
            if (s.source != null)
                Destroy(s.source);
        }

        var lib = libraries.FirstOrDefault(x => x.sceneName == sceneName);
        if (lib == null)
        {
            Debug.LogWarning($"SoundManager: Scene '{sceneName}'�� SoundLibrary�� �����ϴ�.");
            currentSounds = new Sound[0];
            return;
        }

        currentSounds = lib.sounds;

        // AudioSource ����
        foreach (var s in currentSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = s.mixerGroup;
        }

        // ����: �÷��� ���� �� �ڵ����� BGM�̸��� "BGM"�� ���带 ���
        //Play("BGM");
    }

    // ���� ���
    public void Play(string name)
    {
        var s = currentSounds.FirstOrDefault(x => x.name == name);
        if (s != null)
            s.source.Play();
    }

    // ���� ����
    public void Stop(string name)
    {
        var s = currentSounds.FirstOrDefault(x => x.name == name);
        if (s != null)
            s.source.Stop();
    }

    // ���� ���� �޼��� (Master/BGM/SFX)
    // UI �����̴��� �����ϴ� �������� �����Ͽ����ϴ�.
    public void SetMasterVolume(float sliderValue)
        => audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
    public void SetBGMVolume(float sliderValue)
        => audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
    public void SetSFXVolume(float sliderValue)
        => audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
}
