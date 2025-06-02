using System;
using System.Collections.Generic;
using _02._Scripts.Camera;
using _02._Scripts.Character.Player;
using _02._Scripts.Managers.Destructable;
using _02._Scripts.Managers.Destructable.Room;
using _02._Scripts.Managers.Destructable.Stage;
using _02._Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02._Scripts.Managers.Indestructable
{
    public class GameManager : Singleton<GameManager>
    {
        private const string Laststage = "LastStage"; //마지막 스테이지
        private const string Lasttime = "LastTime"; //마지막 시간
        private const string Lastclearroom = "lastClearRoom"; //마지막 퍼즐
        private const string Lastpositionx = "lastPositionX";
        private const string Lastpositiony = "lastPositionY";
        private const string Lastpositionz = "lastPositionZ";
        
        [Header("[LoadData]")]
        [SerializeField] public bool isLoad;         //Load
        [SerializeField] private RoomManager[] roomManagers;
        [SerializeField] private Transform player;   //플레이어 좌표
        
        [Header("[PlayData]")]
        [SerializeField] private bool isGameActive;
        [SerializeField] public float playTime;
    
        [Header("Lobby Camera")]
        [SerializeField] public LobbyCamera lobbyCamera;   //로비 연출용 카메라
        
        // Fields
        private UIManager _uiManager;
        private SceneHandleManager _sceneHandle;
        private int _lastClearRoom;
        private bool _activeTimer;
        private Transform _savePoint;
        
        // Properties
        public bool IsGameActive => isGameActive;
        
        //testCode
        private int RoomId;
    
        protected override void Awake()
        {
            base.Awake();
            
            //데이터 초기화
            isGameActive = false;
            playTime = 0;
            isLoad = false;
            _activeTimer = false;
        }

        private void Start()
        {
            //시작 카메라 연출
            lobbyCamera.gameObject.SetActive(true);
            _uiManager = UIManager.Instance;
            _sceneHandle = SceneHandleManager.Instance;
            // 씬 매니저의 sceneLoaded에 체인을 건다
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                _uiManager.SetOptionUI();
            }

            if (_activeTimer)
            {
                playTime += Time.deltaTime;
                _uiManager.GameUI.UpdatePlayTime(playTime);
            }
        }
    
        // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //저장 기능을 위해 플레이어 위치 탐색
            player = FindAnyObjectByType<Player>()?.transform;
            
            //로비로 복귀 시 UI 셋팅
            if (scene.name == nameof(SCENE_TYPE.Lobby))
            {
                _savePoint = GameObject.FindWithTag("Respawn").transform;
                SoundManager.Instance.ChangeBGM();
                _uiManager.SetGameUI(false);
            }
            
            //게임 로드
            GameLoad_Scene(scene);
        }
        
        public void LoadLobbyScene()
        {
            //데이터 초기화
            playTime = 0;
            RoomId = 0;
            
            //마우스 커서 고정
            Cursor.lockState = CursorLockMode.Locked;
        
            _sceneHandle.LoadScene(SCENE_TYPE.Lobby);
        }

        #region GameStart&Load
        
        public void GameStart()
        {
            // 게임 시작
            isGameActive = true;
        
            //마우스 커서 고정
            Cursor.lockState = CursorLockMode.Locked;
        
            //카메라 이벤트 종료
            lobbyCamera = FindAnyObjectByType<LobbyCamera>();
            lobbyCamera?.DisableCamera();
            
            _uiManager.SetGameUI(false);
        }

        // 저장 데이터 불러오기
        public void GameLoad_Button()
        {
            // 게임 시작
            isGameActive = true;
        
            //마우스 커서 고정
            Cursor.lockState = CursorLockMode.Locked;
            
            //로드 중
            isLoad = true;

            //마지막 정보 불러오기
            playTime = PlayerPrefs.GetFloat(Lasttime);
            _lastClearRoom = PlayerPrefs.GetInt(Lastclearroom);

            //마지막 씬 불러오기
            var sceneName = PlayerPrefs.GetString(Laststage);
            var loadScene = SCENE_TYPE.Lobby;
            
            foreach (var scene in (SCENE_TYPE[])Enum.GetValues(typeof(SCENE_TYPE)))
            {
                 if(sceneName.Equals(scene.ToString())) loadScene = scene;
            }

            _sceneHandle.LoadScene(loadScene);
        }

        private void GameLoad_Scene(Scene scene)
        {
            Debug.Log("asdasd");
            
            if (scene.name != SCENE_TYPE.LoadingScene.ToString() && isLoad)
            {

                if (!player)
                    return;
                
                //세이브 포인트가 로비 일 때
                if (scene.name.Equals(SCENE_TYPE.Lobby.ToString()))
                {
                    player.position = _savePoint.position;
                }
                else
                {   //세이브 포인트가 스테이지 일 때
                    //방의 정보 가져오기 및 정렬
                    roomManagers = FindObjectsOfType<RoomManager>();
                    Array.Sort(roomManagers, (x, y) => x.RoomData.roomId.CompareTo(y.RoomData.roomId));

                    //해결 했던 퍼즐의 문 열림
                    for (int i = 0; i <= _lastClearRoom; i++)
                    {
                        roomManagers[i].OpenDoor();
                    }

                    float x = PlayerPrefs.GetFloat(Lastpositionx);
                    float y = PlayerPrefs.GetFloat(Lastpositiony);
                    float z = PlayerPrefs.GetFloat(Lastpositionz);
                    
                    //플레이어 배치
                    player.position = new Vector3(x, y, z);
                }
                
            }

            //로드 종료
            isLoad = false;
        }
        
        #endregion

        #region Stage

        public void StageStart()
        {
            _activeTimer = true;
            _uiManager.SetGameUI(true);
        }
        
        public void SaveData(int roomId)
        {
            //클리어 저장
            PlayerPrefs.SetString(Laststage,_sceneHandle.currentScene.ToString());
            PlayerPrefs.SetFloat(Lasttime,playTime);
            PlayerPrefs.SetInt(Lastclearroom,roomId);
            PlayerPrefs.SetFloat(Lastpositionx,player.position.x);
            PlayerPrefs.SetFloat(Lastpositiony,player.position.y);
            PlayerPrefs.SetFloat(Lastpositionz,player.position.z);
        }
        public void StageClear()
        {
            //마우스 고정 해제
            Cursor.lockState = CursorLockMode.None;
        
            //타이머 멈추기
            _activeTimer = false;

            //최고 시간초 UI에 설정
            var bestTime = PlayerPrefs.GetFloat(_sceneHandle.currentScene.ToString(), float.MaxValue);
            if (bestTime > playTime)
            {
                bestTime = playTime;
            }
            
            //UI 설정
            _uiManager.SetClearUI(playTime,bestTime);
            
            //모든 룸 클리어 시 로드 저장 위치는 Lobby
            PlayerPrefs.SetString(Laststage,SCENE_TYPE.Lobby.ToString());;
        }

        #endregion
    }
}
