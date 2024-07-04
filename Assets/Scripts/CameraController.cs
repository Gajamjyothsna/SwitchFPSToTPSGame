using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TPSGame
{
    public class CameraController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private Transform playerBody;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private Quaternion targetRotation;
        private float xRotation = 0f;
     
        #endregion

        #region Monobehaviour Methods

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked; 
        }

        void Update()
        {
            // Handle camera rotation based on mouse movement
            HandleCameraRotation();
        }

        void HandleCameraRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate the player body horizontally based on the mouse X input
            playerBody.Rotate(Vector3.up * mouseX);

            // Rotate the camera vertically based on the mouse Y input
            if (virtualCamera != null && (Mathf.Abs(mouseX) > Mathf.Epsilon || Mathf.Abs(mouseY) > Mathf.Epsilon))
            {
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                // Get the current camera rotation
                var cameraTransform = virtualCamera.transform;

                // Calculate the new rotation
                Quaternion targetRotation = Quaternion.Euler(xRotation, cameraTransform.eulerAngles.y, 0f);

                // Apply the rotation to the camera
                cameraTransform.localRotation = targetRotation;
            }
        }
        #endregion
    }

}
