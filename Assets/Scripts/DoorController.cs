using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSGame
{
    public class DoorController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private Animator doorAnimator;
        private bool isOpen = false;
        #endregion

        #region MonoBehaviour Methods
        private void Start()
        {
            if (doorAnimator == null)
            {
                doorAnimator = GetComponent<Animator>();
            }
        }
        #endregion

        #region Public Method
        public void ToggleDoor()
        {
            if (isOpen)
            {
                doorAnimator.SetTrigger("Close");
            }
            else
            {
                doorAnimator.SetTrigger("Open");
            }
            isOpen = !isOpen;
        }
        #endregion
    }
}
