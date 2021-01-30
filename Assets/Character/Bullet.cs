using System;
using UnityEngine;

namespace Character
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        private float _speed;
        private LayerMask _playerLayer;
        private LayerMask _enemyLayer;

        private void Start()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
            _enemyLayer = LayerMask.NameToLayer("Enemy");
        }

        private void Update()
        {
            rigidbody.velocity = _speed * transform.forward;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (CollidedWithPlayer(other) || CollidedWithEnemy(other))
            {
                EntityController entityController = other.gameObject.GetComponent<EntityController>();
                if (entityController == null)
                {
                    Debug.LogException(new Exception("Collisionamos contra un game object de layer entity que no tiene EntityController"));
                    return;
                }

                entityController.ReceiveDamage();
            }
            
            Destroy(gameObject);
        }

        private bool CollidedWithEnemy(Collision other)
        {
            return other.gameObject.layer.Equals(_enemyLayer);
        }

        private bool CollidedWithPlayer(Collision other)
        {
            return other.gameObject.layer.Equals(_playerLayer);
        }

        public void SetSpeed(float bulletSpeed)
        {
            _speed = bulletSpeed;
        }
    }
}