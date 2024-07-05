using System.Collections;
using TMPro;
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
        [SerializeField] private DoorController _doorController;
        [SerializeField] private GameObject _weapon;
        [SerializeField] private int damgeValue;


        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _climbOffset = Vector3.zero;
        private bool _isClimbing = false;
        private bool hasDisplayedMessage = false; // Track if the message has been displayed

        private void Start()
        {
            if (_characterController == null)
            {
                _characterController = GetComponent<CharacterController>();
            }

            // Print initial position for debugging
            Debug.Log($"Initial player position: {transform.position}");
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
                    Quaternion targetRotation;

                    if (verticalValue < -0.01f)
                    {
                        // If the backward button is pressed, rotate 180 degrees
                        targetRotation = Quaternion.LookRotation(-transform.forward);
                    }
                    else
                    {
                        // Calculate target rotation for other directions
                        targetRotation = Quaternion.LookRotation(targetDirection);
                    }

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
            else
            {
                // Reset y position when grounded
                _moveDirection.y = -_characterController.stepOffset; // Apply step offset to keep the player slightly above ground
            }

            // Apply move direction to character controller
            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        private void HandleAnimation()
        {
            // Get horizontal and vertical input values
            float horizontalValue = Input.GetAxis("Horizontal");
            float verticalValue = Input.GetAxis("Vertical");

            // Set moveAmount to 1 if there is input action, otherwise set to 0
            if (Mathf.Abs(horizontalValue) > 0.01f || Mathf.Abs(verticalValue) > 0.01f)
            {
                _playerAnimator.SetFloat("moveAmount", 1f);
            }
            else
            {
                _playerAnimator.SetFloat("moveAmount", 0f);
            }
        }

        public void PlayAttack()
        {
            // Trigger attack animation
            _weapon.SetActive(true);
            _playerAnimator.SetBool("isAttacking", true);
            StartCoroutine(ResetAttackAfterDelay(1f)); // Adjust the delay as needed
        }

        private IEnumerator ResetAttackAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _playerAnimator.SetBool("isAttacking", false);
            _weapon.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ClimbTrigger"))
            {
                _isClimbing = true;
                // Example: Adjust the player's position when entering climb trigger
                _climbOffset = new Vector3(0f, 1f, 0f); // Adjust this offset based on your step height
                Debug.Log("Enter ClimbTrigger");
            }

            if (other.CompareTag("DoorPoint"))
            {
                _doorController.OpenDoor();
                if(hasDisplayedMessage == false)
                {
                    UIController.Instance.DisplayPopUpMessage("Collect Blue Color Objects to Get the Keys");
                    hasDisplayedMessage = true;
                }
            }

            if (other.CompareTag("SmallEnemySword") || other.CompareTag("LargeEnemySword"))
            {
                //Take the damage of the health of the Player
                Debug.Log("Player Hit");
                UIController.Instance.UpdatePlayerHealth(damgeValue, false);
            }

            if (other.CompareTag("Key"))
            {
                string text = GetRandomAlphabet().ToString();
                UIController.Instance.DisplayKey(text);
                Destroy(other.gameObject);

                if (UIController.Instance.key.Length == 5)
                {
                    UIController.Instance.DisplayPopUpMessage("Go out of the room, the real game starts");
                    TPSGameManager.Instance.SwitchView();
                    transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                    //Instantiate the Enemies 
                    StartCoroutine(InvokeEnemiesAfterDelay());
                }
            }
        }

        private IEnumerator InvokeEnemiesAfterDelay()
        {
            yield return new WaitForSeconds(8f);
            TPSGameManager.Instance.StartSpawnController();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ClimbTrigger"))
            {
                _isClimbing = false;
                // Reset climb offset when exiting climb trigger
                _climbOffset = Vector3.zero;
                Debug.Log("Exit ClimbTrigger");
                // Reset player's y position to ground level when exiting climb trigger
                transform.position = new Vector3(transform.position.x, _characterController.stepOffset, transform.position.z);
            }
           
        }

        private void FixedUpdate()
        {
            // Move the character controller with climb offset if climbing
            if (_isClimbing)
            {
                _characterController.Move(_climbOffset * Time.deltaTime);
            }
        }

        public static char GetRandomAlphabet()
        {
            // Generate a random number between 0 and 25
            int randomNumber = Random.Range(0, 26);

            // Convert the random number to a character (A-Z)
            char randomAlphabet = (char)('A' + randomNumber);

            return randomAlphabet;
        }

    }
}
