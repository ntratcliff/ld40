using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariController : MonoBehaviour
{
    public KatamariBall Ball;
    public GameObject Player;

    public float RotSpeed;
    public float RotAccel;
    public float RotDampening;

    public float MoveSpeed;
    public float MoveAccel;
    public float MoveDampening;

    public float PlayerZOffset = -0.5f;

    private float _rotVelo;
    private Vector3 _velo;

    public Vector3 Velocity
    {
        get { return _velo; }
    }

    private void Update()
    {
        _updateRot();
        _updateVelo();
        _updateBall();
        _updatePlayer();
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
        Vector3 perp = Vector3.Cross(Vector3.up, _velo); // rotate about the perpendicular axis
        Ball.transform.Rotate(perp, _velo.magnitude * Ball.Circumference * Time.deltaTime, Space.World);
    }

    private void _updatePlayer()
    {
        RaycastHit bottomCast = _playerDistCast(0);
        RaycastHit midCast = _playerDistCast(1f);
        RaycastHit topCast = _playerDistCast(2f);

        RaycastHit smallestHit = topCast;
        if (midCast.distance != 0 && midCast.distance < smallestHit.distance)
            smallestHit = midCast;
        if (bottomCast.distance != 0 && bottomCast.distance < smallestHit.distance)
            smallestHit = bottomCast;

        Vector3 worldPos = smallestHit.point;
        worldPos.y = Player.transform.position.y;
        worldPos -= transform.forward * PlayerZOffset;
        Player.transform.position = worldPos;
    }

    private RaycastHit _playerDistCast(float y)
    {
        Vector3 pos = Ball.transform.position - transform.forward * (Ball.Radius + 1f);
        pos.y = y;
        Vector3 ball = Ball.transform.position;
        ball.y = y;

        Vector3 direction = ball - pos;
        Debug.DrawRay(pos, direction, Color.cyan);
        Debug.DrawRay(pos + Vector3.up, Vector3.down, Color.red);

        RaycastHit hit;
        if(Physics.Raycast(pos, direction, out hit))
        {
            Debug.DrawLine(pos, hit.point, Color.yellow);
        }

        return hit;
    }
}