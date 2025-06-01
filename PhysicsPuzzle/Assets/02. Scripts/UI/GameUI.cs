using _02._Scripts.Managers.Indestructable;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _02._Scripts.UI
{
    public class GameUI : BaseUI
    {
        private readonly int IsMoonTime_b = Animator.StringToHash("IsMoonTime_b");

        [Header("Components")] [SerializeField]
        private Animator animator;

        [Header("Top UI")]
        [SerializeField] private GameObject currentInfo; //현재 시간 UI 우측 상단
        [SerializeField] private TextMeshProUGUI currentStage; // 스테이지 이름
        [SerializeField] private TextMeshProUGUI currentTime; // 진행 시간

        [Header("Bottom UI")] [SerializeField] private GameObject changeGravityUI; // 중력 전환 아이콘
        [SerializeField] private GameObject gravityUpIcon; // 중력 Up 아이콘
        [SerializeField] private GameObject gravityDownIcon; // 중력 Down 아이콘
        [SerializeField] private Image gravityCooldownImage; //중력 쿨타임
        [SerializeField] private GameObject changeTimeUI; // 시간 전환 아이콘
        [SerializeField] private Image timeCooldownImage;

        [Header("Inventory UI")] [SerializeField]
        private GameObject inventoryUI; //인벤토리

        [SerializeField] private TextMeshProUGUI itemName; //아이템 이름

        public Transform InventoryUITransform => inventoryUI.transform;

        protected override UIState GetUIState()
        {
            return UIState.Game;
        }

        public void ChangeGravity(bool isPlayerUpsideDown) // tab키 입력씨 중력반전 UI전환
        {
            if (!isPlayerUpsideDown)
            {
                gravityUpIcon.SetActive(false);
                gravityDownIcon.gameObject.SetActive(true);
            }
            else
            {
                gravityUpIcon.SetActive(true);
                gravityDownIcon.gameObject.SetActive(false);
            }
        }

        public void ChangeTime(bool isMoonTime)
        {
            animator.SetBool(IsMoonTime_b, isMoonTime);
        }

        public void ChangeGravityIconFillAmount(float amount)
        {
            gravityCooldownImage.fillAmount = amount;
        }

        public void ChangeTimeIconFillAmount(float amount)
        {
            timeCooldownImage.fillAmount = amount;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            ChangeSceneName();
        }

        public void ChangeSceneName()
        {
            currentStage.text = SceneManager.GetActiveScene().name; // 스테이지 이름 
        }

        public void UpdatePlayTime(float playTime)
        {
            int minutes = Mathf.FloorToInt(playTime / 60f);
            int seconds = Mathf.FloorToInt(playTime % 60f);

            currentTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }


        public void SetPromptText(string text)
        {
            itemName.gameObject.SetActive(true);

            itemName.text = text;
        }

        public void ClearPromptText()
        {
            itemName.gameObject.SetActive(false);

            itemName.text = string.Empty;
        }

        public void SetTimer(bool onOff)
        {
            currentInfo.SetActive(onOff);
        }
    }
}