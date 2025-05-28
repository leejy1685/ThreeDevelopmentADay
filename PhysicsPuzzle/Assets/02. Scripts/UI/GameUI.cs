using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    [Header("Top UI")]
    [SerializeField] private GameObject timeIcon; //현재 시간 UI 우측 상단
    [SerializeField] private TextMeshProUGUI currentStage; // 스테이지 이름
    [SerializeField] private TextMeshProUGUI currentTime; // 진행 시간
    private float playTime = 0f; // 진행 시간 담을 변수

    [Header("Bottom UI")]
    [SerializeField] private GameObject changeGravityUI; // 중력 전환 아이콘
    [SerializeField] private GameObject gravityUpIcon; // 중력 Up 아이콘
    [SerializeField] private GameObject gravityDownIcon; // 중력 Down 아이콘

    [SerializeField] private Image gravityCooldownImage; //중력 쿨타임
    [SerializeField] private GameObject changeTimeUI; // 시간 전환 아이콘
    [SerializeField] private Image timeCooldownImage;

    [SerializeField] public GameObject InventoryUI; //인벤토리
    [SerializeField] private TextMeshProUGUI itemName; //아이템 이름
    [SerializeField] private Animator _animator; //애니메이터
    public bool _isMoonTime = false; // 현재 시간이 밤인지 여부
    public bool _isChangingTime = false; // 전환 중인지 여부

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
    private void Start()
    {
        _animator.SetBool("IsMoonTime_b", false); // 초기 낮 상태
    }

    private void Update()
    {
        TestChangeTime();// 테스트용
        UpdatePlayTime();
    }

    private void ChangeSceneName()
    {
        currentStage.text = SceneManager.GetActiveScene().name; // 스테이지 이름 
    }
    public void ChangeGravity(bool isPlayerUpsideDown) // g키 입력씨 중력반전 UI전환
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

    public void ChangeGravityIconFillAmount(float amount)
    {
        gravityCooldownImage.fillAmount = amount;
    }

    public void ChangeTimeIconFillAmount(float amount)
    {
        timeCooldownImage.fillAmount = amount;
    }
    public void UpdatePlayTime()
    {
        playTime += Time.deltaTime;

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

    public void TestChangeTime() // 애니메이션 작동 확인용 테스트 메서드
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _isMoonTime = !_isMoonTime;
            _animator.SetBool("IsMoonTime_b", _isMoonTime);
        }
    }


}