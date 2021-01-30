using Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Image healthBarFill;
        private EntityController _player;

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
    }
}
