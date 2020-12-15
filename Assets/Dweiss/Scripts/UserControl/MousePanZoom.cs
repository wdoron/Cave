using UnityEngine;
using System.Collections;

//AddComponentMenu("Camera-Control/Mouse Pan Zoom")]
public class MousePanZoom : MonoBehaviour {

	public float lookSpeed = 15.0f;
	public float  moveSpeed = 15.0f;
	
	public float  rotationX = 0.0f;
	public float  rotationY = 0.0f;

    [Tooltip("PanVertical PanHorizontal PanZoom Axis should be configured")]
    public bool usePan;
	public enum MouseButtons : int
	{
		Left = 0,
		Right = 1,
		Middle = 2
	}
	public MouseButtons mouseButton = MouseButtons.Right;


    public bool forwardOnFloorOnly = false;
	void Update ()
	{

		if (Input.GetMouseButton((int)mouseButton))
		{
			rotationX += ( Input.GetAxis ("Mouse X") ) * lookSpeed;
			rotationY += (Input.GetAxis ("Mouse Y") ) * lookSpeed;
			//rotationY = Mathf.Clamp (rotationY, -90, 90);
			

		}
		transform.localRotation = Quaternion.AngleAxis (rotationX, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis (rotationY, Vector3.left);

        if (usePan)
        {
            transform.position += transform.up * moveSpeed * Input.GetAxis("PanVertical") * Time.deltaTime;
            transform.position += transform.right * moveSpeed * Input.GetAxis("PanHorizontal") * Time.deltaTime;
        }
        var forward = transform.forward;
        if (forwardOnFloorOnly)
        {
            forward = Vector3.ProjectOnPlane(forward, Vector3.up);
        }
		transform.position += forward * moveSpeed * 
            ((usePan? Input.GetAxis ("PanZoom") : 0) + Input.GetAxis("Mouse ScrollWheel")) * Time.deltaTime;
	}
}
