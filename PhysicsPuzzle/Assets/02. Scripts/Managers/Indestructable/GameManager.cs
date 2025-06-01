using System;
using System.Collections.Generic;
using _02._Scripts.Camera;
using _02._Scripts.Character.Player;
using _02._Scripts.Managers.Destructable;
using _02._Scripts.Managers.Destructable.Room;
using _02._Scripts.Managers.Destructable.Stage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02._Scripts.Managers.Indestructable
{
    public class GameManager : Singleton<GameManager>
    {

        [Header("[Managers]")]
        private UIManager _uiManager;
        private SceneHandleManager _sceneHandle;
    
        [Header("[LoadData]")]
        public bool isLoad;         //Load
        private RoomManager[] _roomManagers;
        private Transform player;   //플레이어 좌표
        private int lastClearRoom;     
        private const string LASTSTAGE = "LastStage";   //마지막 스테이지
        private const string LASTTIME = "LastTime";     //마지막 시간
        private const string LASTCLEARROOM = "lastClearRoom";//마지막 퍼즐
        private const string LASTPOSITIONX = "lastPositionX";
        private const string LASTPOSITIONY = "lastPositionY";
        private const string LASTPOSITIONZ = "lastPositionZ";

        [Header("[PlayData]")]
        private bool isGameActive;
        public bool IsGameActive => isGameActive;
        private bool activeTimer;
        public float playTime;
    
        
        [SerializeField] public LobbyCamera _lobbyCamera;   //로비 연출용 카메라
        

        //testCode
        private int RoomId;
    
        protected override void Awake()
        {
            base.Awake();
            
            //데이터 초기화
            isGameActive = false;
            playTime = 0;
            isLoad = false;
            activeTimer = false;
        }

        private void Start()
        {
            //시작 카메라 연출
            _lobbyCamera.gameObject.SetActive(true);
            _uiManager = UIManager.Instance;
            _sceneHandle = SceneHandleManager.Instance;
            // 씬 매니저의 sceneLoaded에 체인을 건다
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                StageManager.Instance.RoomCleared(RoomId);
                RoomId++;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                _uiManager.SetOptionUI();
            }

            if (activeTimer)
            {
                playTime += Time.deltaTime;
                _uiManager.GameUI.UpdatePlayTime(playTime);
            }
        }
    
        // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //저장 기능을 위해 플레이어 위치 탐색
            player = FindAnyObjectByType<Player>()?.transform;
            
            //로비로 복귀 시 UI 셋팅
            if (scene.name == SCENE_TYPE.Lobby.ToString())
            {
                SoundManager.Instance.ChangeBGM();
                _uiManager.SetGameUI(false);
            }
            
            //게임 로드
            GameLoad_Scene(scene);
        }


        #region GameStart&Load
        public void GameStart()
        {
            // 게임 시작
            isGameActive = true;
        
            //마우스 커서 고정
            Cursor.lockState = CursorLockMode.Locked;
        
            //카메라 이벤트 종료
            _lobbyCamera = FindAnyObjectByType<LobbyCamera>();
            _lobbyCamera?.DisableCamera();
            
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
            playTime = PlayerPrefs.GetFloat(LASTTIME);
            lastClearRoom = PlayerPrefs.GetInt(LASTCLEARROOM);

            //마지막 씬 불러오기
            string sceneName = PlayerPrefs.GetString(LASTSTAGE);
            SCENE_TYPE loadScene = SCENE_TYPE.Lobby;
            for (int i = 0; i < (int)SCENE_TYPE.Count;i++)
            {
                if (sceneName == ((SCENE_TYPE)i).ToString())
                {
                    loadScene = (SCENE_TYPE)i;
                }
            }

            _sceneHandle.LoadScene(loadScene);
        }

        private void GameLoad_Scene(Scene scene)
        {
            if (scene.name != SCENE_TYPE.LoadingScene.ToString() && isLoad)
            {
                //방의 정보 가져오기 및 정렬
                _roomManagers = FindObjectsOfType<RoomManager>();
                Array.Sort(_roomManagers, (x, y) => x.RoomData.roomId.CompareTo(y.RoomData.roomId));
                
                if (player != null)
                {
                    float x = PlayerPrefs.GetFloat(LASTPOSITIONX);
                    float y = PlayerPrefs.GetFloat(LASTPOSITIONY);
                    float z = PlayerPrefs.GetFloat(LASTPOSITIONZ);
                    //플레이어 배치
                    player.position = new Vector3(x,y,z);
                }
                
                //해결 했던 퍼즐의 문 열림
                for (int i = 0; i <= lastClearRoom; i++)
                {
                    _roomManagers[i].OpenDoor();
                }
                
                //로드 종료
                isLoad = false;
            }
        }

        

        #endregion

        #region Stage

        public void StageStart()
        {
            activeTimer = true;
            _uiManager.SetGameUI(true);
        }
        
        public void SaveData(int roomId)
        {
            //클리어 저장
            PlayerPrefs.SetString(LASTSTAGE,_sceneHandle.currentScene.ToString());
            PlayerPrefs.SetFloat(LASTTIME,playTime);
            PlayerPrefs.SetInt(LASTCLEARROOM,roomId);
            PlayerPrefs.SetFloat(LASTPOSITIONX,player.position.x);
            PlayerPrefs.SetFloat(LASTPOSITIONY,player.position.y);
            PlayerPrefs.SetFloat(LASTPOSITIONZ,player.position.z);
        }
        public void StageClear()
        {
            //마우스 고정 해제
            Cursor.lockState = CursorLockMode.None;
        
            //타이머 멈추기
            activeTimer = false;

            //최고 시간초 UI에 설정
            float bestTime = PlayerPrefs.GetInt(_sceneHandle.currentScene.ToString(),0);
            if (bestTime > playTime)
            {
                bestTime = playTime;
            }
            
            //UI 설정
            _uiManager.SetClearUI(playTime,bestTime);
            
            //모든 룸 클리어 시 로드 저장 위치는 Lobby
            PlayerPrefs.SetString(LASTSTAGE,SCENE_TYPE.Lobby.ToString());;
        }

        #endregion

        public void LoadLobbyScene()
        {
            //데이터 초기화
            playTime = 0;
            RoomId = 0;
            
            //마우스 커서 고정
            Cursor.lockState = CursorLockMode.Locked;
        
            _sceneHandle.LoadScene(SCENE_TYPE.Lobby);
        }
        
    }
}
