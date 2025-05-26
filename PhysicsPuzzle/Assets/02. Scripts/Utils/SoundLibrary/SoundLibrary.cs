using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [Tooltip("이 라이브러리를 적용할 씬 이름")]
    public string sceneName;

    [Tooltip("이 씬에서 사용할 사운드 리스트")]
    public Sound[] sounds;
}
