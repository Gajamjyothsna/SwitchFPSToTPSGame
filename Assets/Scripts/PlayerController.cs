using UnityEngine;

namespace TPSGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private float _playerSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _gravity = 9.81f; // Adjusted to standard gravity
        [SerializeField] private float _jumpHeight = 3f;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _cameraTransform; // Reference to the camera's transform

        private Vector3 _moveDirection = Vector3.zero;

        private void Start()
        {
            if (_characterController == null)
            {
                _characterController = GetComponent<CharacterController>();
            }
           
        }

        private void Update()
        {
            HandleRotation();
            HandleMovement();
            HandleAnimation();
        }

        private void HandleRotation()
        {
            // Get horizontal and vertical input values
            float horizontalValue = Input.GetAxis("Horizontal");
            float verticalValue = Input.GetAxis("Vertical");

            // Only rotate if there is movement input
            if (Mathf.Abs(horizontalValue) > 0.01f || Mathf.Abs(verticalValue) > 0.01f)
            {
                // Calculate target direction based on input values
                Vector3 targetDirection = new Vector3(horizontalValue, 0f, verticalValue);

                // Convert input direction from local to world space
                targetDirection = transform.TransformDirection(targetDirection);

                // Ensure target direction is normalized
                if (targetDirection.magnitude > 0)
                {
                    // Calculate target rotation
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                    // Smoothly rotate towards the target direction
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleMovement()
        {
            // Get horizontal and vertical input values
            float horizontalValue = Input.GetAxis("Horizontal");
            float verticalValue = Input.GetAxis("Vertical");

            // Calculate movement direction relative to the player's orientation
            Vector3 moveDirection = new Vector3(horizontalValue, 0f, verticalValue).normalized;
            moveDirection = transform.TransformDirection(moveDirection);

            // Apply movement speed
            _characterController.Move(moveDirection * _playerSpeed * Time.deltaTime);

            // Apply gravity
            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= _gravity * Time.deltaTime;
            }
        }

        private void HandleAnimation()
        {
            // Update animations based on movement
            _playerAnimator.SetFloat("moveAmount", _characterController.velocity.magnitude / _playerSpeed); // Adjust based on your animation setup
        }

        public void PlayAttack()
        {
            // Trigger attack animation
            _playerAnimator.SetBool("isAttacking", true);
        }
    }
}
