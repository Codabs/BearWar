using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    //
    //Variable
    //
    [SerializeField] private Camera cam;
    [SerializeField] private CinemachineBrain _camBrain;
    [SerializeField] private CinemachineVirtualCamera _virtualCam;

    [SerializeField] int _moveSpeed;
    [SerializeField] private float _zoomSensibility;
    private Vector3 _dragOrgine;
    [SerializeField] private Vector2 _clampX;
    [SerializeField] private Vector2 _clampY;
    //
    //MONOBEHEVIORS
    //
    void Update()
    {
        PanCamera();
        CameraZoom();
    }

    //
    //FONCTION
    //
    public void SetCameraFocus(GameObject gameObject)
    {

    }
    public void RemoveCameraFocus()
    {

    }
    public void RecenterCameraAt(Vector3 vector3)
    {

    }
    public void SetCameraClamp() { }
    private void PanCamera()
    {
        //
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _dragOrgine = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        //
        if (Input.GetMouseButton(1))
        {
            Vector3 differance = _dragOrgine - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position += differance;
            ClampCamera();
        }
    }
    private void EdgeScrolling()
    {
        int _edgeScrollSize = 20;
        if (Input.mousePosition.x < _edgeScrollSize) { }
        if (Input.mousePosition.y < _edgeScrollSize) { }
        if (Input.mousePosition.x > Screen.width + _edgeScrollSize) { }
        if (Input.mousePosition.y > Screen.height + _edgeScrollSize) { }
    }
    private void CameraZoom()
    {
        _virtualCam.m_Lens.OrthographicSize += Input.mouseScrollDelta.y*-1 * _zoomSensibility;
        _virtualCam.m_Lens.OrthographicSize = Mathf.Clamp(_virtualCam.m_Lens.OrthographicSize, 1, 6);
    }
    private void ClampCamera()
    {
        cam.transform.position = new Vector3
            (Mathf.Clamp(cam.transform.position.x, _clampX.x, _clampX.y), 
            Mathf.Clamp(cam.transform.position.y, _clampY.x, _clampY.y), 
            cam.transform.position.z);
    }
}
