using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSGame
{
    public class PlayerSword : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private int _gainLargeEnemyDamageValue;
        [SerializeField] private int _gainSmallEnemyDamageValue;
        #endregion

        #region Private Methods
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(TPSGameDataModel.PoolObjectType.LargeEnemy.ToString()) )
            {
                //Take Large enemy value;
                Debug.Log("Large Enemy Hit");
                other.gameObject.GetComponent<EnemyController>().Die();
                UIController.Instance.UpdatePlayerHealth(_gainLargeEnemyDamageValue, true);
            }
            if (other.CompareTag(TPSGameDataModel.PoolObjectType.SmallEnemy.ToString()))
            {
                //Take small enemy value;
                Debug.Log("Small Enemy Hit");
                other.gameObject.GetComponent<EnemyController>().Die();
                UIController.Instance.UpdatePlayerHealth(_gainSmallEnemyDamageValue, true);
            }
        }
        #endregion

    }
}
