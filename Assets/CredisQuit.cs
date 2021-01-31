using Sound;
using UnityEngine;

public class CredisQuit : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayMusic(SoundManager.Instance.IntroMusic);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        } 
    }
}
