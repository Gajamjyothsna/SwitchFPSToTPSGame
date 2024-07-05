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
        [SerializeField] private float _damageValue;
        [SerializeField] private Animator _enemyAnimator;
        [SerializeField] private float attackDistance = .5f; // Distance threshold to start attacking

        [Header("Animation Parameters")]
        [SerializeField] private string _enemyBlendValue;
        [SerializeField] private float _idleValue;
        [SerializeField] private float _walkValue;
        [SerializeField] private float _attackValue;

        private Transform _target;
        private Rigidbody _rigidbody;
        private float separationDistance = 5f; // Minimum distance between enemies
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
            _target = GameObject.Find("Player").transform;
            _enemyAnimator.SetFloat(_enemyBlendValue, _idleValue);
            _rigidbody = GetComponent<Rigidbody>(); // Ensure you have a Rigidbody component
        }

        // Update is called once per frame
        void Update()
        {
            if (_target != null && !IsDead) MoveTowardsPlayer();
        }

        private void MoveTowardsPlayer()
        {
            if (isAttacking || IsDead)
                return; // Don't move if attacking or dead

            Vector3 direction = _target.position - transform.position;
            float distance = direction.magnitude;

            if (distance > separationDistance)
            {
                // Move towards the player if outside separation distance
                _enemyAnimator.SetFloat(_enemyBlendValue, _walkValue); // Set animation to walking
                Vector3 moveDirection = direction.normalized; // Normalize direction vector
                Vector3 newPosition = transform.position + moveDirection * _enemySpeed * Time.deltaTime; // Calculate new position
                _rigidbody.MovePosition(newPosition); // Move the enemy to the new position
                transform.LookAt(_target.position); // Rotate towards the player
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
                    separationVector += awayFromEnemy.normalized / awayFromEnemy.magnitude;
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

        #endregion
    }
}
