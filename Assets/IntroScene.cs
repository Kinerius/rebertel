using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(SoundManager.Instance.gameObject);
        SoundManager.Instance.PlayMusic(SoundManager.Instance.IntroMusic);
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            changeScene();
        }
    }
    private void changeScene()
    {
        SceneManager.LoadScene("Opening");
    }
}
