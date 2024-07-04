using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace TPSGame
{
    public class PlayerController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private float _playerSpeed;
        [SerializeField] private float _rotationSpeed = 1f;

        private Transform playerTransform;
        private Quaternion targetRotation;
        private Vector3 moveDirection = Vector3.zero;
        private Vector3 smoothMovementVelocity;
        private Rigidbody _rigidbody;
        #endregion

        #region Monobehaviour Methods
        private void Start()
        {
            playerTransform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float horizontalValue = Input.GetAxis("Horizontal");
            float verticalValue = Input.GetAxis("Vertical");

            // Calculate movement direction relative to player's forward direction
            Vector3 moveDirection = CalculateMoveDirection(horizontalValue, verticalValue);
            // Rotate player towards movement direction
            if (moveDirection.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
            // Move the player
            MovePlayer(moveDirection);

            // Set animation based on movement
            SetMovementAnimation(moveDirection);
        }
        #endregion


        #region Private Methods

        private Vector3 CalculateMoveDirection(float horizontalValue, float verticalValue)
        {
            // Calculate move direction relative to the camera's forward direction
            Vector3 forward = Vector3.Scale(_rigidbody.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 right = Vector3.Scale(_rigidbody.transform.right, new Vector3(1, 0, 1)).normalized;

            return (forward * verticalValue + right * horizontalValue).normalized;
        }

        void MovePlayer(Vector3 direction)
        {
            // Move the player using Rigidbody for physics-based movement
            Vector3 newPosition = _rigidbody.position + direction * _playerSpeed * Time.deltaTime;
            _rigidbody.MovePosition(newPosition);
        }
        void SetMovementAnimation(Vector3 moveDirection)
        {
            // Set animation based on movement direction
            if (moveDirection.magnitude > 0)
            {
                _playerAnimator.SetFloat("moveAmount", .5f); // Playing Walking Animation
            }
            else
            {
                _playerAnimator.SetFloat("moveAmount", 0); // Playing Idle Animation
            }
        }

        public void PlayAttack()
        {
            _playerAnimator.SetBool("isAttacking", true);
        }
        #endregion
    }
}
