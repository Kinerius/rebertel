
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character
{
    public class EntityController : MonoBehaviour
    {
        public event Action<int, int> OnHealthChangedEvent;
        public event Action OnCollidedWithSomething;
        public event Action OnDeathEvent;
        public event Action OnNextLevelPortalEntered;
        public event Action OnNextLevelPortalExited;
        
        private const float BulletSpawnDistance = .5f;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator anim;

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

        private float _currentDashDistance;
        private float _currentDashTime;
        private float _bulletTimer;
        private bool _isDashing = false;
        private float _dashTimer = 0;
        private float _lastDashDistance;
        private bool _isShieldActive = false;
        private int _currentLife = 0;
        private Vector3 _lastMoveDirection;
        private Vector3 _inputMovement;
        private int _portalLayer;
        
        // shooting animation variables
        private float _shootStateTimeLeft = 0;
        private bool _hasShotRecently = false;
        private float _shootStateTotalTime = .5f;
        
        private static readonly int DirectionChanged = Animator.StringToHash("DirectionChanged");
        private static readonly int MoveDirection = Animator.StringToHash("MoveDirection");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int DashHash = Animator.StringToHash("Dash");
        private static readonly int IsDefending = Animator.StringToHash("IsDefending");
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

        private void Start()
        {
            _portalLayer = LayerMask.NameToLayer("Portal");
            _currentLife = life;
            anim.speed = .5f;
        }

        public void SetMovementDirection(Vector3 direction)
        {
            _inputMovement = direction.normalized;
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

            SetShootingAnimationState(direction);

            var bulletIsntance = Instantiate(projectile, spawnPosition, Quaternion.LookRotation(direction));
            var bullet = bulletIsntance.GetComponent<Bullet>();
            bullet.SetSpeed(bulletSpeed);
            _bulletTimer = 1 / bulletsPerSecond;
        }

        private void SetShootingAnimationState(Vector3 direction)
        {
            if (_hasShotRecently) return;
            _hasShotRecently = true;
            _shootStateTimeLeft = _shootStateTotalTime;
            anim.SetBool(IsAttacking, true);
            anim.SetTrigger(DirectionChanged);
            anim.SetInteger(MoveDirection, GetAnimationDirection(direction));
        }

        public void Dash()
        {
            if (_isDashing) return;
            anim.SetTrigger(DashHash);
            InitializeDashWithParams(dashTime, dashDistance);
        }

        public void Stun(Vector3 fromEnemyPosition)
        {
            _lastMoveDirection = (fromEnemyPosition - transform.position).normalized;
            _lastMoveDirection.y = transform.position.y;
            InitializeDashWithParams(.25f, -2);
        }

        private void InitializeDashWithParams(float time, float dist)
        {
            _isDashing = true;
            _dashTimer = 0;
            _lastDashDistance = 0;
            _currentDashTime = time;
            _currentDashDistance = dist;
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
            HandleShootState();
        }

        private void HandleShootState()
        {
            if (_hasShotRecently)
            {
                _shootStateTimeLeft -= Time.deltaTime;
                if (_shootStateTimeLeft <= 0)
                {
                    _hasShotRecently = false;
                    anim.SetBool(IsAttacking, false);
                    anim.SetTrigger(DirectionChanged);
                }
            }
        }

        private void EnableShield()
        {
            _isShieldActive = true;
            anim.SetBool(IsDefending, true);
            anim.SetTrigger(DirectionChanged);
        }

        private void DisableShield()
        {
            _isShieldActive = false;
            anim.SetBool(IsDefending, false);
            anim.SetTrigger(DirectionChanged);
        }

        private void HandleDash()
        {
            if (!_isDashing) return;
            if (_dashTimer >= _currentDashTime)
            {
                _isDashing = false;
                anim.SetTrigger(DirectionChanged);
                return;
            }
            
            var currentDashDistance = dashCurve.Evaluate(_dashTimer/_currentDashTime);
            var dashDelta = currentDashDistance - _lastDashDistance;
            var positionDelta = dashDelta * _currentDashDistance * _lastMoveDirection;
            
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
            if (_inputMovement.magnitude <= 0)
            {
                anim.SetBool(IsWalking, false);
                return;
            }
            anim.SetBool(IsWalking, true);
            SetMovementAnimation();

            var finalSpeed = Time.deltaTime * movementSpeed * _inputMovement;
            _lastMoveDirection = _inputMovement;
            var collisions = characterController.Move(finalSpeed);
            if (collisions != CollisionFlags.None)
                OnCollidedWithSomething?.Invoke();
        }

        private void SetMovementAnimation()
        {
            int animationIndex = 0;

            animationIndex = GetAnimationDirection(_lastMoveDirection);

            if (anim.GetInteger(MoveDirection) != animationIndex)
            {
                anim.SetTrigger(DirectionChanged);
                
                if (!_hasShotRecently) anim.SetInteger(MoveDirection, animationIndex);
            }
        }

        private int GetAnimationDirection(Vector3 direction)
        {
            var rightDot = Vector3.Dot(direction, Vector3.right);
            var leftDot = Vector3.Dot(direction, Vector3.left);
            var upDot = Vector3.Dot(direction, Vector3.forward);
            var downDot = Vector3.Dot(direction, Vector3.back);

            var dic = new Dictionary<int, float>()
            {
                {0, downDot},
                {1, rightDot},
                {2, leftDot},
                {3, upDot}
            };

            return dic.OrderByDescending(kvp => kvp.Value).First().Key;
        }

        public void ReceiveDamage()
        {
            if (_isShieldActive) return;
            _currentLife--;
            OnHealthChanged(life, _currentLife);
            if (_currentLife <= 0) Death();
        }

        private void Death()
        {
            OnDeath();
            Destroy(gameObject);
        }

        protected virtual void OnHealthChanged(int max, int current)
        {
            OnHealthChangedEvent?.Invoke(max, current);
        }

        protected virtual void OnDeath()
        {
            OnDeathEvent?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer.Equals(_portalLayer))
            {
                OnNextLevelPortalEntered?.Invoke();
            }
            if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            {
                var player = other.gameObject.GetComponent<EntityController>();
                if (!player._isShieldActive)
                {
                    player.ReceiveDamage();
                    player.Stun(gameObject.transform.position);
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer.Equals(_portalLayer))
            {
                OnNextLevelPortalExited?.Invoke();
            }
        }
    }
}
