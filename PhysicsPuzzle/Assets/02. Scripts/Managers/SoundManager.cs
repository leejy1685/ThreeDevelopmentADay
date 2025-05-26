using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class SoundManager : Singleton<SoundManager>
{
   
    [Header("씬별 사운드 라이브러리")]
    public List<SoundLibrary> libraries;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    // 런타임에 사용 중인 사운드
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

    // 씬이 로드될 때마다 호출
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadLibraryForScene(scene.name);
    }

    // 해당 씬 이름에 맞는 라이브러리를 찾아 적용
    private void LoadLibraryForScene(string sceneName)
    {
        // 이전 사운드 해제
        foreach (var s in currentSounds)
        {
            if (s.source != null)
                Destroy(s.source);
        }

        var lib = libraries.FirstOrDefault(x => x.sceneName == sceneName);
        if (lib == null)
        {
            Debug.LogWarning($"SoundManager: Scene '{sceneName}'용 SoundLibrary가 없습니다.");
            currentSounds = new Sound[0];
            return;
        }

        currentSounds = lib.sounds;

        // AudioSource 세팅
        foreach (var s in currentSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = s.mixerGroup;
        }

        // 예시: 플레이 시작 시 자동으로 BGM이름이 "BGM"인 사운드를 재생
        //Play("BGM");
    }

    // 사운드 재생
    public void Play(string name)
    {
        var s = currentSounds.FirstOrDefault(x => x.name == name);
        if (s != null)
            s.source.Play();
    }

    // 사운드 정지
    public void Stop(string name)
    {
        var s = currentSounds.FirstOrDefault(x => x.name == name);
        if (s != null)
            s.source.Stop();
    }

    // 볼륨 조절 메서드 (Master/BGM/SFX)
    // UI 슬라이더에 연결하는 방향으로 구현하였습니다.
    public void SetMasterVolume(float sliderValue)
        => audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
    public void SetBGMVolume(float sliderValue)
        => audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
    public void SetSFXVolume(float sliderValue)
        => audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
}
