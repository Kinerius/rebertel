
using System;
using UnityEngine;

namespace Character
{
    public class EntityController : MonoBehaviour
    {
        public event Action<int, int> OnHealthChangedEvent;
        public event Action OnCollidedWithSomething;
        
        private const float BulletSpawnDistance = .5f;
        [SerializeField] private CharacterController characterController;

        [Header("Stats")]
        [SerializeField] private int life = 3;
        
        [Header("Movement")] 
        [SerializeField] private float movementSpeed = 5;

        [SerializeField] private float dashDistance = 3;
        [SerializeField] private float dashTime = .5f;
        [SerializeField] private AnimationCurve dashCurve;
        
        [Header("Shooting")]
        [SerializeField] private GameObject projectile;
        [SerializeField] private float bulletsPerSecond = 2;
        [SerializeField] private float bulletSpeed = 10;

        [Header("Spawning")] [SerializeField] private float spawnTime = 0;

        private float _bulletTimer;
        private bool _isDashing = false;
        private float _dashTimer = 0;
        private float _lastDashDistance;
        private bool _isShieldActive = false;
        private int _currentLife = 0;
        private Vector3 _lastMoveDirection;
        private Vector3 _inputMovement;

        private void Start()
        {
            _currentLife = life;
        }

        public void SetMovementDirection(Vector3 direction)
        {
            _inputMovement = direction;
        }
        public void ShootAt(Vector3 position)
        {
            if (_isShieldActive) return;
            if (_bulletTimer > 0) return;
            
            var entityPosition = transform.position;
            var hipHeight = entityPosition.y + 1;
            var targetPosition = position;
            targetPosition.y = hipHeight;
            var direction = (targetPosition - entityPosition).normalized;
            var spawnPosition = entityPosition + Vector3.up + direction * BulletSpawnDistance;

            var bulletIsntance = Instantiate(projectile, spawnPosition, Quaternion.LookRotation(direction));
            var bullet = bulletIsntance.GetComponent<Bullet>();
            bullet.SetSpeed(bulletSpeed);
            _bulletTimer = 1 / bulletsPerSecond;
        }

        public void Dash()
        {
            _isDashing = true;
            _dashTimer = 0;
            _lastDashDistance = 0;
        }
        
        public void ToggleShield(bool isActive)
        {
            if (isActive)
                EnableShield();
            else
                DisableShield();
        }
        
        void Update()
        {
            if (!_isDashing) 
                HandleMovement();
            else
                HandleDash();
            
            UpdateCooldowns();
        }

        private void EnableShield()
        {
            _isShieldActive = true;
        }
        private void DisableShield()
        {
            _isShieldActive = false;
        }

        private void HandleDash()
        {
            if (_dashTimer >= dashTime)
            {
                _isDashing = false;
                return;
            }
            
            var currentDashDistance = dashCurve.Evaluate(_dashTimer/dashTime);
            var dashDelta = currentDashDistance - _lastDashDistance;
            var positionDelta = dashDelta * dashDistance * _lastMoveDirection;
            
            CollisionFlags flags = characterController.Move(positionDelta);

            if ((flags & CollisionFlags.Sides) != 0)
            {
                _isDashing = false;
                return;
            }
            
            _dashTimer += Time.deltaTime;
            _lastDashDistance = currentDashDistance;
        }

        private void UpdateCooldowns()
        {
            _bulletTimer = Mathf.Max(_bulletTimer - Time.deltaTime, 0);
        }

        private void HandleMovement()
        {
            if (_inputMovement.magnitude <= 0) return;
            
            var finalSpeed = Time.deltaTime * movementSpeed * _inputMovement;
            _lastMoveDirection = _inputMovement;
            var collisions = characterController.Move(finalSpeed);
            if (collisions != CollisionFlags.None)
                OnCollidedWithSomething?.Invoke();
        }

        public void ReceiveDamage()
        {
            if (_isShieldActive) return;
            _currentLife--;
            OnHealthChanged(life, _currentLife);
            if (_currentLife <= 0) Destroy(gameObject);
        }

        protected virtual void OnHealthChanged(int max, int current)
        {
            OnHealthChangedEvent?.Invoke(max, current);
        }
    }
}
