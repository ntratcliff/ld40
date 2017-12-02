using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class CameraCrane : MonoBehaviour 
{
    public float Yaw; // camera Y rot
    public float Pitch; // camera X rot
    public float Roll; // camera Z rot

    public float ArmPitch; // arm X rot
    public float BaseYaw; // base Y rot
    public float CameraDistance; // camera Z pos

    public Transform Arm;
    public Transform Camera;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Arm && Camera != null)
            _updateTransforms();
	}

    private void _updateTransforms()
    {
        Camera.localRotation = Quaternion.Euler(Pitch, Yaw, Roll);

        Camera.transform.localPosition = new Vector3(
            Camera.transform.localPosition.x,
            Camera.transform.localPosition.y,
            -CameraDistance);

        Arm.rotation = Quaternion.Euler(
            ArmPitch,
            Arm.rotation.eulerAngles.y,
            Arm.rotation.eulerAngles.z);

        transform.rotation = Quaternion.Euler(
            transform.rotation.x,
            BaseYaw,
            transform.rotation.z);
    }
}