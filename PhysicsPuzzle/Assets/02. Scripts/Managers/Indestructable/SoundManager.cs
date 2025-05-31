using UnityEngine;

namespace _02._Scripts.Managers.Indestructable
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Range(0f,1f)][SerializeField]private float pitchValue = 0.5f;
    
        [Header( "[BGM]" )]
        private AudioSource audioSource;
        [SerializeField] private AudioClip BGM;
        [Range(0f, 1f)] public float BGMVolume;
        public float SetBGMVolume
        {
            set
            {
                BGMVolume = Mathf.Clamp(value, 0f, 1f); 
                audioSource.volume = BGMVolume;
            }
        }

        [Header( "[SFX]" )]
        [SerializeField] private SoundSource soundPrefab;
        [Range(0f,1f)]public float SFXVolume = 0.5f;
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
            audioSource.volume = BGMVolume;
            audioSource.Play();
        }

        public static void PlaySFX(AudioClip clip)
        {
            SoundSource obj = Instantiate(Instance.soundPrefab);
            SoundSource soundSource = obj.GetComponent<SoundSource>();
            soundSource.Play(clip, Instance.SFXVolume, Instance.pitchValue);
        }
    }
}
