using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSGame
{
    public class CameraSwitcher : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private GameObject _tpsCamera;
        [SerializeField] private GameObject _fpsCamera;
        [SerializeField] private GameObject _tpsVirtualCamera;

        private bool isTPS = false;
        #endregion

        #region Private Methods
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                SwitchView();
            }
        }

        private void SwitchView()
        {
            isTPS = !isTPS;

            _tpsCamera.SetActive(isTPS);
            _tpsVirtualCamera.SetActive(isTPS); 
            _fpsCamera.SetActive(!isTPS);
        }
        #endregion
    }
}
