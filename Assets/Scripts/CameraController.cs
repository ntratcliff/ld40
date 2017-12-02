using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraCrane))]
public class CameraController : MonoBehaviour 
{
    public KatamariController Target;
    public float PosLerpSpeed;
    public float RotLerpSpeed;

    private CameraCrane _crane;

	// Use this for initialization
	void Start () 
	{
        _crane = GetComponent<CameraCrane>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        _updatePos();
        _updateRot();
	}

    private void _updatePos()
    {
        //transform.position = Vector3.Slerp(transform.position, Target.transform.position, PosLerpSpeed * Time.deltaTime);
        transform.position = _interpolate(transform.position, Target.transform.position, PosLerpSpeed * Time.deltaTime);
    }

    private float _lastTargetRot;
    private void _updateRot()
    {
        // apply
        float targetRot = Target.transform.rotation.eulerAngles.y;
        if (Mathf.Abs(targetRot - _lastTargetRot) > 180f) // target rot has likely rolled over
        {
            // roll crane yaw over
            _crane.BaseYaw += Mathf.Sign(targetRot - _lastTargetRot) * 360f;
        }

        //_crane.BaseYaw = Mathf.Lerp(_crane.BaseYaw, targetRot, RotLerpSpeed * Time.deltaTime);
        _crane.BaseYaw = _interpolate(_crane.BaseYaw, targetRot, RotLerpSpeed * Time.deltaTime);

        _lastTargetRot = targetRot;
    }

    private Vector3 _interpolate(Vector3 from, Vector3 to, float t)
    {
        return from + (to - from) * t;
    }

    private float _interpolate(float from, float to, float t)
    {
        return from + (to - from) * t;
    }
}