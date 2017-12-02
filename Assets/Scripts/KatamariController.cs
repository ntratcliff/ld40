using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariController : MonoBehaviour
{
    public GameObject Ball;
    public GameObject Player;

    public float RotSpeed;
    public float RotAccel;
    public float RotDampening;

    public float MoveSpeed;
    public float MoveAccel;
    public float MoveDampening;

    private float _rotVelo;
    private Vector3 _velo;

    private void Update()
    {
        _updateRot();
        _updateVelo();
        _updateBall();
    }

    private void _updateRot()
    {
        float horiz = Input.GetAxis("Horizontal");

        // calculate accel
        float accel = horiz * RotAccel;

        // apply to velo
        _rotVelo += accel * Time.deltaTime;

        // clamp velo
        _rotVelo = Mathf.Clamp(_rotVelo, -RotSpeed, RotSpeed);

        // apply
        transform.rotation *= Quaternion.Euler(0, _rotVelo * Time.deltaTime, 0);

        // dampen
        if (horiz == 0)
            _rotVelo *= RotDampening;
    }

    private void _updateVelo()
    {
        float vert = Input.GetAxis("Vertical");

        // calculate accel
        float accel = vert * MoveAccel;

        // apply to velo
        _velo += transform.forward * accel * Time.deltaTime;

        // clamp velo
        _velo = Vector3.ClampMagnitude(_velo, MoveSpeed);

        // apply
        transform.position += _velo * Time.deltaTime;

        // dampen
        if (vert == 0)
            _velo *= MoveDampening;
    }

    private void _updateBall()
    {
        // rotate relative to forward velo
        float c = Mathf.PI * Ball.transform.localScale.y; // we can use local scale because this is always 1
        Vector3 perp = Vector3.Cross(Vector3.up, _velo); // rotate about the perpendicular axis
        Ball.transform.Rotate(perp, _velo.magnitude * c * Time.deltaTime, Space.World);
    }
}