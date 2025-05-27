using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
   
    [Header("Top UI")]
    [SerializeField] private GameObject _nowTImeIcon; //현재 시간 UI 우측 상단
    [SerializeField] private TextMeshProUGUI _stageNameText; // 스테이지 이름
    [SerializeField] private TextMeshProUGUI _timeText; // 진행 시간

    [Header("Bottom UI")]
    [SerializeField] private GameObject _changeGravityUI; // 중력 전환 아이콘
    [SerializeField] private GameObject _gravityUpIcon;   // 중력 Up 아이콘
    [SerializeField] private GameObject _gravityDownIcon; // 중력 Down 아이콘
    [SerializeField] private Image _gravityCooldown; //중력 쿨타임
    public float gravityCooldown = 3f;
    private bool _isGravity = false;
    private bool _isgravityCooldown = false;
    [SerializeField] private GameObject _changeTimeUI; // 시간 전환 아이콘

    [SerializeField] private Image _timecooldown;
    public float timechangeCooldown = 3f;
    
    private bool _istimeCooldown = false;
    
    [SerializeField] public GameObject InventoryBG; //인벤토리
    [SerializeField] private TextMeshProUGUI _itemText; //아이템 이름
    [SerializeField] private TextMeshProUGUI _itemdescription; // 아이템 설명
    private void Update()
    {
        ChangeGravity();
        ChangeTime();
    }


    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
    
    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
    }
    private IEnumerator HandleGravityCooldown()
    {
        _isgravityCooldown = true;
        float currentTime = gravityCooldown;
        _gravityCooldown.fillAmount = 1;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            _gravityCooldown.fillAmount = currentTime / gravityCooldown;
            yield return null;
        }

        _gravityCooldown.fillAmount = 0;
        _isgravityCooldown = false;
    }

    private IEnumerator HandleTimeCooldown()
    {
        _istimeCooldown = true;
        float currentTime = timechangeCooldown;
        _timecooldown.fillAmount = 1;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            _timecooldown.fillAmount = currentTime / timechangeCooldown;
            yield return null;
        }

        _timecooldown.fillAmount = 0;
        _istimeCooldown = false;
    }
    public void ChangeGravity() // tab키 입력씨 중력반전 UI전환
    {
        if (Input.GetKeyDown(KeyCode.Tab) &&!_isgravityCooldown)
        {
            _isGravity = !_isGravity;


            if (_isGravity) //중력이 정상일때
            {
                _gravityUpIcon.SetActive(false);
                _gravityDownIcon.gameObject.SetActive(true);
                

                


            }

            if (!_isGravity)
            {

                _gravityUpIcon.SetActive(true);
                _gravityDownIcon.gameObject.SetActive(false);

            }
            StartCoroutine(HandleGravityCooldown());
        }

    }

    public void ChangeTime() // g키 입력씨 시간 UI전환
    {
        if (Input.GetKeyDown(KeyCode.F) && !_istimeCooldown)
        {
            
            StartCoroutine(HandleTimeCooldown());
        }

    }


}
