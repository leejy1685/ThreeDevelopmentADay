using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [Tooltip("�� ���̺귯���� ������ �� �̸�")]
    public string sceneName;

    [Tooltip("�� ������ ����� ���� ����Ʈ")]
    public Sound[] sounds;
}
