using _02._Scripts.Utils;
using _02._Scripts.Utils.SoundLibrary;
using UnityEngine;

namespace _02._Scripts.Managers.Indestructable
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Components")]
        [SerializeField] private AudioSource audioSource;
        
        [Header("Sound Settings")]
        [Range(0f, 1f)] [SerializeField] private float pitchValue = 0.5f;
        [Range(0f, 1f)] [SerializeField] public float bgmVolume;
        [Range(0f, 1f)] [SerializeField] public float SFXVolume = 0.5f;
        
        [Header( "[BGM]" )]
        [SerializeField] private AudioClip BGM;
        
        [Header( "[SFX]" )] 
        [SerializeField] private SoundSource soundPrefab;
        
        // Properties
        public float SetBGMVolume
        {
            set
            {
                bgmVolume = Mathf.Clamp(value, 0f, 1f); 
                audioSource.volume = bgmVolume;
            }
        }
        
        public float SetSFXVolume
        {
            set { SFXVolume = Mathf.Clamp(value, 0f, 1f); }
        }

        protected override void Awake()
        {
            base.Awake();
        
            audioSource = GetComponent<AudioSource>();
        
            audioSource.loop = true;
            ChangeBGM(BGM);

        }

        public void ChangeBGM(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.volume = bgmVolume;
            audioSource.Play();
        }

        public static void PlaySfx(AudioClip clip)
        {
            SoundSource obj = Instantiate(Instance.soundPrefab);
            SoundSource soundSource = obj.GetComponent<SoundSource>();
            soundSource.Play(clip, Instance.SFXVolume, Instance.pitchValue);
        }
    }
}
