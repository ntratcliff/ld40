using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariController : MonoBehaviour
{
    public KatamariBall Ball;
    public GameObject Player;

    public LayerMask PlayerRayMask;
    public float PlayerRayLerpSpeed;

    public float RotSpeed;
    public float RotAccel;
    public float RotDampening;

    public float MoveAccel;

    public float PlayerZOffset = -0.5f;

    private float _rotVelo;

    private Vector3 _targetPlayerPos;

    private void Update()
    {
        _updateRot();
        _updateVelo();
        _updatePlayer();

        // go to ball pos
        Vector3 pos = transform.position;
        pos = Ball.transform.position;
        pos.y = transform.position.y;
        transform.position = pos;
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

        // apply to ball velo
        Ball.ApplyVelo(transform.forward * accel);

        // dampen
        if (vert == 0)
            Ball.DampenVelo();
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