using UnityEngine;

namespace CameraMovement
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovementController : MonoBehaviour
    {
        [SerializeField]
        private float dragSpeed;

        private Camera mainCamera;
        private Vector3 dragOrigin;
        private Vector3 defaultCameraPosition;
        private Vector3 mousePosition;
        private Vector3 translation;

        private void Start()
        {
            mainCamera = GetComponent<Camera>();
            defaultCameraPosition = mainCamera.transform.position;
        }

        private void Update()
        {
            if (!Input.GetMouseButton(0))
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            mousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            translation = new Vector3(mousePosition.x, mousePosition.y, 0) * -dragSpeed;
            transform.Translate(translation, Space.World);
        }

        public void ResetView()
        {
            mainCamera.transform.position = defaultCameraPosition;
        }
    } 
}