using System.Collections;
using _02._Scripts.Managers.Indestructable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _02._Scripts.Managers.Destructable
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        [SerializeField] public Slider progressBar;

        private void Start()
        {
            StartCoroutine(LoadTargetScene_Coroutine());
        }

        IEnumerator LoadTargetScene_Coroutine()
        {
            string sceneName = SceneHandleManager.Instance.NextSceneName;
            // 비동기 로딩 작업할 씬 선택
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            // 비동기로 로딩할 씬 자동활성 잠금
            operation.allowSceneActivation = false;

            // 실제 로딩 작업과 진행률ui바 연결 0->0.9까지 증가
            while (operation.progress < 0.9f)
            {
                if (progressBar != null)
                    progressBar.value = operation.progress;

                yield return null;
            }

            // 0.9에서 로딩완료 => 1까지 직접 증가시켜주는 작업
            if (progressBar != null)
                progressBar.value = 1f;


            //1초 기다렸다 화면 전환, 클릭으로 화면 전환 받을 거면 이곳에서 구현
            yield return new WaitForSeconds(1f);

            // 화면 전환
            operation.allowSceneActivation = true;
        }
    }
}