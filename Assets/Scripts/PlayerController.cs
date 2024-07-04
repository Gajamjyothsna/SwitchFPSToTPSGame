using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSGame
{
    public class PlayerController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private float _playerSpeed;
        #endregion
        void Start()
        {
            
        }

        void Update()
        {
            float horizontalValue = Input.GetAxis("Horizontal");
            float verticalValue = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalValue, 0, verticalValue) * _playerSpeed * Time.deltaTime;

            transform.Translate(movement);

            if(movement.magnitude > 0 )
            {
                _playerAnimator.SetFloat("moveAmount", 1);
            }
            else if(movement.magnitude <= 0 )
            {
                _playerAnimator.SetFloat("moveAmount", 0);
            }
        }
    }
}
