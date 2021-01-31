using System;
using Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        private static UIController instance;
        public static UIController Instance => instance;
        
        [SerializeField] private Image healthBarFill;
        [SerializeField] private RectTransform countdownContainer;
        [SerializeField] private TextMeshProUGUI countdownText;
       
        [SerializeField] private Image defeatScreen;
        private EntityController _player;

        private void Awake()
        {
            instance = this;
            defeatScreen.gameObject.SetActive(false);
        }

        void Start()
        {
            _player = GameObject.Find("Player").GetComponent<EntityController>();
            _player.OnHealthChangedEvent += OnHealthChanged;

            healthBarFill.fillAmount = 1;
        }


        private void OnHealthChanged(int maxHealth, int minHealth)
        {
            healthBarFill.fillAmount = minHealth / (float)maxHealth;
        }

        public void SetCountdown(long countdown)
        {
            if (countdown < 0)
            {
                countdownContainer.gameObject.SetActive(false);
                return;
            }
            countdownContainer.gameObject.SetActive(true);
            countdownText.text = countdown.ToString();
        }

        public void ShowDefeat()
        {
            defeatScreen.gameObject.SetActive(true);
        }
    }
}
