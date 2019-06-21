using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoomController : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField]
    private float minCameraDistance;
    [SerializeField]
    private float maxCameraDistance;

    [SerializeField]
    private float scrollingDuration;
    [SerializeField]
    private float zoomingSpeed;

    private float currentScrollingTime;

    private bool zoomingIn;
    private bool zoomingOut;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        DetectScrollInput();
        LerpCameraDistance();
    }

    private void DetectScrollInput()
    {
        if (NoMouseScrollDetected())
        {
            return;
        }
        else if (MouseScrollUpDetected())
        {
            currentScrollingTime = scrollingDuration;
            zoomingOut = false;
            zoomingIn = true;
        }
        else if (MouseScrollDownDetected())
        {
            currentScrollingTime = scrollingDuration;
            zoomingOut = true;
            zoomingIn = false;
        }
    }

    private bool MouseScrollDownDetected()
    {
        return Input.mouseScrollDelta.y < 0;
    }

    private bool MouseScrollUpDetected()
    {
        return Input.mouseScrollDelta.y > 0;
    }

    private bool NoMouseScrollDetected()
    {
        return Input.mouseScrollDelta.y == 0;
    }

    private void LerpCameraDistance()
    {
        if (ZoomingOutPossible())
        {
            ZoomOut();
        }
        else if (ZoomingInPossible())
        {
            ZoomIn();
        }
        else if (ZoomingNotPossible())
        {
            StopZooming();
        }
    }

    private bool ZoomingOutPossible()
    {
        return currentScrollingTime > 0 && zoomingOut;
    }

    private bool ZoomingInPossible()
    {
        return currentScrollingTime > 0 && zoomingIn;
    }

    private bool ZoomingNotPossible()
    {
        return currentScrollingTime <= 0;
    }

    private void ZoomOut()
    {
        currentScrollingTime -= Time.deltaTime;
        if (mainCamera.orthographicSize + Time.deltaTime * zoomingSpeed < maxCameraDistance)
        {
            mainCamera.orthographicSize += Time.deltaTime * zoomingSpeed;
        }
    }

    private void ZoomIn()
    {
        currentScrollingTime -= Time.deltaTime;
        if (mainCamera.orthographicSize - Time.deltaTime * zoomingSpeed > minCameraDistance)
        {
            mainCamera.orthographicSize -= Time.deltaTime * zoomingSpeed;
        }
    }

    private void StopZooming()
    {
        zoomingOut = false;
        zoomingIn = false;
    }
}
