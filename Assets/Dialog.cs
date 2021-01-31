using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    GameObject TextBox;

    public event Action OnSentensesComplete;

    private List<Sprite> _sentences;

    private Image _image;
    private int contador=0;

    private void checkSentence()
    {
        TextBox.SetActive(false);
        _image = TextBox.GetComponent<Image>();
        if (contador < _sentences.Count)
        {
            TextBox.SetActive(true);
            _image.sprite = _sentences[contador];
            Time.timeScale = 0;
        }
    }
    public void setSentences(List<Sprite> sentences)
    {
        _sentences = new List<Sprite>(sentences);
        contador = 0;
        checkSentence();
    }
    public void NextSentence()
    {
        contador++;
        if (contador < _sentences.Count)
        {
            TextBox.SetActive(true);
            _image.sprite = _sentences[contador];
        }
        else
        {
            TextBox.SetActive(false);
            Time.timeScale = 1;
            _sentences.Clear();
            OnSentensesComplete?.Invoke();
        }
    }
}
