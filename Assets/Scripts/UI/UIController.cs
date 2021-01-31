using System;
using Character;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        
        [SerializeField] private RectTransform bossBar;
        [SerializeField] private Image bossBarLife;

        private EntityController _player;

        private void Awake()
        {
            instance = this;
            defeatScreen.gameObject.SetActive(false);
            bossBar.gameObject.SetActive(false);
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
            defeatScreen.OnPointerClickAsObservable()
                .Do(_ => SceneManager.LoadScene("MenuOpening"))
                .Subscribe();
        }

        public void ShowBossBar(EntityController entity)
        {
            bossBar.gameObject.SetActive(true);
            entity.OnHealthChangedEvent += OnBossDamaged;
            entity.OnDeathEvent += () => OnBossDeath(entity);
            bossBarLife.fillAmount = 1;
        }

        private void OnBossDeath(EntityController entity)
        {
            entity.OnHealthChangedEvent -= OnBossDamaged;
            SceneManager.LoadScene("Credits");
        }

        private void OnBossDamaged(int max, int current)
        {
            bossBarLife.fillAmount = current / (float)max;
        }
    }
}
