using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    GameObject TextBox;
    [SerializeField]
    private List<Sprite> sentences;

    private Image _image;
    private int contador=0;
    
    void Start()
    {
        sentences = new List<Sprite>();
    }
    private void checkSentence()
    {
        TextBox.SetActive(false);
        _image = TextBox.GetComponent<Image>();
        if (contador < sentences.Count)
        {
            TextBox.SetActive(true);
            _image.sprite = sentences[contador];
            Time.timeScale = 0;
        }
    }
    public void setSentences(List<Sprite> sentences)
    {
        this.sentences = sentences;
        contador = 0;
        checkSentence();
    }
    public void NextSentence()
    {
        contador++;
        if (contador < sentences.Count)
        {
            TextBox.SetActive(true);
            _image.sprite = sentences[contador];
        }
        else
        {
            TextBox.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
