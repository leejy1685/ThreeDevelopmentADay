using System.Collections;
using _02._Scripts.Managers.Indestructable;
using _02._Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _02._Scripts.Managers.Destructable
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        [SerializeField] public Slider progressBar;
        [SerializeField] public Image fadeImage;
        [SerializeField] public TextMeshProUGUI text;

        private void Start()
        {
            StartCoroutine(LoadTargetScene_Coroutine());
            UIManager.Instance.ChangeState(UIState.LoadingScene);
        }

        IEnumerator LoadTargetScene_Coroutine()
        {
            var sceneName = SceneHandleManager.Instance.NextSceneName;
            // 비동기 로딩 작업할 씬 선택
            var operation = SceneManager.LoadSceneAsync(sceneName);

            // 비동기로 로딩할 씬 자동활성 잠금
            operation.allowSceneActivation = false;

            // 실제 로딩 작업과 진행률ui바 연결 0->0.9까지 증가
            while (operation.progress < 0.9f)
            {
                if (progressBar)
                    progressBar.value = operation.progress;

                yield return null;
            }

            // 0.9에서 로딩완료 => 1까지 직접 증가시켜주는 작업
            if (progressBar)
                progressBar.value = 1f;


            //1초 기다렸다 화면 전환, 클릭으로 화면 전환 받을 거면 이곳에서 구현
            yield return new WaitForSeconds(1f);

            text.text = "Press any key";

            yield return StartCoroutine(WaitUserInput_Coroutine());

            yield return StartCoroutine(FadeOut_Coroutine());

            // 화면 전환
            operation.allowSceneActivation = true;
        }

        IEnumerator WaitUserInput_Coroutine()
        {
            var elapsedTime = 0f;

            while (!Input.anyKeyDown && !Input.GetMouseButtonDown(0))
            {
                elapsedTime += Time.deltaTime;

                var alphaValue = Mathf.PingPong(elapsedTime * 1.5f, 1f);
                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0.2f, 1f, alphaValue));
                yield return null;
            }
        }

        IEnumerator FadeOut_Coroutine()
        {
            var elapsedTime = 0f;
            ;
            var fadeDuration = 1f;
            var color = fadeImage.color;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alphaValue = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                fadeImage.color = new Color(color.r, color.g, color.b, alphaValue);
                yield return null;
            }

            fadeImage.color = new Color(color.r, color.g, color.b, 1f);
        }
    }
}

