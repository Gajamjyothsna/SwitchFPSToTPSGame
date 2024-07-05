using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSGame
{
    public class EnemyController : MonoBehaviour
    {

        #region Private Variables
        [Header("UI Components")]
        [SerializeField] private float _enemySpeed;
        [SerializeField] private Animator _enemyAnimator;
        [SerializeField] private float attackDistance = 1f; // Distance threshold to start attacking

        [SerializeField] private TPSGameDataModel.PoolObjectType _enemyType;

        [Header("Animation Parameters")]
        [SerializeField] private string _enemyBlendValue;
        [SerializeField] private float _idleValue;
        [SerializeField] private float _walkValue;
        [SerializeField] private float _attackValue;
        [SerializeField] private float _dieValue;

        private Transform _target;
        private AudioSource _audioSource;
       
        private float fixedYPosition; // Fixed Y position for the enemy
        private float separationDistance = 1f; // Minimum distance between enemies
        private bool canAttack = true;
        private bool isAttacking = false; // To track if the enemy is currently attacking
        private bool isDead = false;
        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set
            {
                isDead = value;
            }
        }
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _target = GameObject.Find("Player").transform;
            _enemyAnimator.SetFloat(_enemyBlendValue, _idleValue);
            fixedYPosition = transform.position.y; // Store the initial Y position
            SoundManager.Instance.PlaySound(_audioSource, TPSGameDataModel.SoundType.EnemyRoar);
        }

        // Update is called once per frame
        void Update()
        {
            if (_target != null && !IsDead) MoveTowardsPlayer();


            // Ensure Y position is fixed
            Vector3 position = transform.position;
            position.y = fixedYPosition;
            transform.position = position;
        }

        private void MoveTowardsPlayer()
        {
            if (isAttacking || IsDead)
                return; // Don't move if attacking or dead

            Vector3 direction = _target.position - transform.position;
            float distance = direction.magnitude;
            if (distance > attackDistance)
            {
                // Move towards the player if outside attack distance
                _enemyAnimator.SetFloat(_enemyBlendValue, _walkValue); // Set animation to walking
                Vector3 moveDirection = direction.normalized; // Normalize direction vector

                // Apply separation vector
                Vector3 separationVector = GetSeparationVector();
                moveDirection += separationVector;

                // Normalize moveDirection to prevent NaN values
                if (moveDirection.magnitude > 0)
                {
                    moveDirection.Normalize();
                }
                else
                {
                    moveDirection = Vector3.zero;
                }

                Vector3 newPosition = transform.position + moveDirection * _enemySpeed * Time.deltaTime; // Calculate new position
                newPosition.y = fixedYPosition; // Ensure Y position is fixed
                transform.position = newPosition; // Move the enemy to the new position directly
                transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z)); // Rotate towards the player
            }
            else if (distance <= attackDistance && canAttack)
            {
                // Start attacking if within attack distance and can attack
                StartCoroutine(AttackAfterDelay());
            }
            else
            {
                // Idle state or other conditions
                _enemyAnimator.SetFloat(_enemyBlendValue, _idleValue); // Set animation to idle
            }
        }

        private Vector3 GetSeparationVector()
        {
            Vector3 separationVector = Vector3.zero;
            Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, separationDistance);
            foreach (Collider collider in nearbyEnemies)
            {
                if (collider != null && collider.transform != transform && collider.GetComponent<EnemyController>() != null)
                {
                    Vector3 awayFromEnemy = transform.position - collider.transform.position;
                    float magnitude = awayFromEnemy.magnitude;

                    if (magnitude < separationDistance && magnitude > 0)
                    {
                        separationVector += awayFromEnemy.normalized / magnitude;
                    }
                }
            }
            return separationVector;
        }

        private IEnumerator AttackAfterDelay()
        {
            canAttack = false; // Prevent further attacks until coroutine finishes
            isAttacking = true; // Set attacking flag to true
            _enemyAnimator.SetFloat(_enemyBlendValue, _attackValue); // Set animation to attack
            // Logic for attack action, e.g., dealing damage to the player
            yield return new WaitForSeconds(1f); // Attack duration
            isAttacking = false; // Reset attacking flag
            canAttack = true; // Allow attacking again
        }

        public void Die()
        {
            IsDead = true;
            Debug.Log("enemy type is killed" + _enemyType);
            SoundManager.Instance.PlaySound(_audioSource, TPSGameDataModel.SoundType.EnemyRoar);
            StartCoroutine(DelayAnimation());
        }
        IEnumerator DelayAnimation()
        {
            yield return new WaitForSeconds(1f);
            _enemyAnimator.SetFloat(_enemyBlendValue, _dieValue); // Set animation to idle or death
            yield return new WaitForSeconds(1f);
            GameObject.Find("SpawnController").GetComponent<SpawnController>().EnemyDefeated(gameObject, _enemyType); // Notify the SpawnController
            gameObject.SetActive(false);
        }
    }

        #endregion
    
}
