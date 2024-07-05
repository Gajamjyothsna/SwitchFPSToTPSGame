using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSGame
{
    public class EnemySword : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private int damgeValue;
        #endregion

        #region Private Methods
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                //Take the damage of the health of the Player
                Debug.Log("Player Hit");
                UIController.Instance.UpdatePlayerHealth(damgeValue, false);
            }
        }
        #endregion
    }
}
