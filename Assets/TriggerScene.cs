using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScene : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
