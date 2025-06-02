using _02._Scripts.Managers.Indestructable;
using UnityEngine;
using UnityEngine.UI;

namespace _02._Scripts.UI
{
    public class OptionUI : BaseUI
    {
        [Header("[Option UI]")] [SerializeField]
        private Slider bgmSlider;

        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Button backButton;

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);

            backButton.onClick.AddListener(OnClickBackButton);
        }

        private void Update()
        {
            sfxSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SetSFXVolume = value);
            ;
            bgmSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SetBGMVolume = value);
            ;
        }

        protected override UIState GetUIState()
        {
            return UIState.Obtion;
        }


        public void OnClickBackButton()
        {
            if (uiManager.PrevState == UIState.Game)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            uiManager.ChangeState(uiManager.PrevState);
        }
    }
}