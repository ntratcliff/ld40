using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariController : MonoBehaviour
{
    public KatamariBall Ball;
    public GameObject Player;

    public LayerMask PlayerRayMask;
    public LayerMask BallRayMask;
    public int BallRayAccuracy = 3;
    public float BallRayLerpSpeed;
    public float PlayerRayLerpSpeed;

    public float RotSpeed;
    public float RotAccel;
    public float RotDampening;

    public float MoveSpeed;
    public float MoveAccel;
    public float MoveDampening;

    public float PlayerZOffset = -0.5f;

    private float _rotVelo;
    private Vector3 _velo;

    private float _targetBallY;
    private Vector3 _targetPlayerPos;

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

        // raycast from ground to ball
        RaycastHit minHit;
        _ballDistCast(0, 0, out minHit);

        // try some multiples of the radius as an offset, for accuracy
        for (float x = 1f; x <= BallRayAccuracy; x++)
        {
            for (float z = 1f; z <= BallRayAccuracy; z++)
            {
                float xOff = Ball.Radius / (BallRayAccuracy / x);
                float zOff = Ball.Radius / (BallRayAccuracy / z);

                RaycastHit hit;
                if (_ballDistCast(xOff, zOff, out hit) && hit.distance < minHit.distance)
                    minHit = hit;
                if (_ballDistCast(-xOff, zOff, out hit) && hit.distance < minHit.distance)
                    minHit = hit;
                if (_ballDistCast(-xOff, -zOff, out hit) && hit.distance < minHit.distance)
                    minHit = hit;
                if (_ballDistCast(xOff, -zOff, out hit) && hit.distance < minHit.distance)
                    minHit = hit;
            }
        }

        Debug.Log("Minhit: " + minHit);

        // set ball y to dist
        Vector3 localPos = Ball.transform.localPosition;
        localPos.y = Ball.Radius - minHit.distance;
        Ball.transform.localPosition = localPos;

        //_targetBallY = Ball.Radius - minHit.distance;

        //Debug.Log("TargetY: " + _targetBallY);

        // lerp to target y
        // TODO: get this working
        //Vector3 localPos = Ball.transform.localPosition;
        //localPos.y = MathHelper.Interpolate(Ball.transform.localPosition.y, _targetBallY, BallRayLerpSpeed * Time.deltaTime);
        //Ball.transform.localPosition = localPos;
    }

    private bool _ballDistCast(float xOff, float zOff, out RaycastHit hit)
    {
        // raycast up from radius to ball
        Vector3 pos = Ball.transform.position;
        pos.y -= Ball.Radius;
        pos.x += xOff;
        pos.z += zOff;

        Vector3 target = Ball.transform.position;
        target.x += xOff;
        target.z += zOff;

        Vector3 dir = target - pos;
        Debug.DrawRay(pos, dir, Color.cyan);

        if (Physics.Raycast(pos, dir, out hit, 100, BallRayMask))
        {
            Debug.DrawLine(pos, hit.point, Color.yellow);
            return true;
        }

        return false;
    }

    private void _updatePlayer()
    {
        RaycastHit bottomCast;
        RaycastHit midCast;

        RaycastHit smallestHit;
        _playerDistCast(2f, out smallestHit);

        if (_playerDistCast(1f, out midCast)
            && midCast.distance < smallestHit.distance)
            smallestHit = midCast;

        if (_playerDistCast(0f, out bottomCast)
            && bottomCast.distance < smallestHit.distance)
            smallestHit = bottomCast;

        Vector3 worldPos = smallestHit.distance != 0 ? smallestHit.point : transform.position - transform.forward * Ball.Radius;
        worldPos.y = Player.transform.position.y;
        worldPos -= transform.forward * PlayerZOffset;
        _targetPlayerPos = worldPos;

        Vector3 pos = Player.transform.position;
        pos = MathHelper.Interpolate(pos, _targetPlayerPos, PlayerRayLerpSpeed * Time.deltaTime);
        Player.transform.position = pos;
    }

    private bool _playerDistCast(float y, out RaycastHit hit)
    {
        Vector3 pos = Ball.transform.position - transform.forward * (Ball.Radius + 1f);
        pos.y = y;
        Vector3 ball = Ball.transform.position;
        ball.y = y;

        Vector3 direction = ball - pos;
        Debug.DrawRay(pos, direction, Color.cyan);
        Debug.DrawRay(pos + Vector3.up, Vector3.down, Color.red);

        if (Physics.Raycast(pos, direction, out hit, 100, PlayerRayMask))
        {
            Debug.DrawLine(pos, hit.point, Color.yellow);
            return true;
        }

        return false;
    }
}