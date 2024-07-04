using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TPSGame
{
    public class PlayerController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private float _playerSpeed;
        [SerializeField] private float rotationSpeed = 700f;
        private Vector3 movement;
        #endregion

        #region Monobehaviour Methods
        private void Start()
        {
            
        }

        private void Update()
        {
            float horizontalValue = Input.GetAxis("Horizontal"); //Getting Horizontal Axis value
            float verticalValue = Input.GetAxis("Vertical"); //Getting Vertical Axis value

            movement = new Vector3(horizontalValue, 0, verticalValue) * _playerSpeed * Time.deltaTime;

            if (movement.magnitude > 0 )
            {
                _playerAnimator.SetFloat("moveAmount", 1); //Playing Walking Animation
                transform.Translate(movement);
            }
            else if(movement.magnitude <= 0 )
            {
                _playerAnimator.SetFloat("moveAmount", 0); //Playing Idle Animation
            }
        }
        #endregion


        #region Private Methods
        private void RotatePlayer()
        {
            if (movement.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        #endregion
    }
}
