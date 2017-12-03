using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KatamariBall : MonoBehaviour 
{
    public float MaxSpeed;
    public float Dampening;
    public GameObject BallMesh;
    public AudioSource SFXSource;
    public AudioClip[] PickupClips;

    public UnityEvent OnPickup;

    private Bounds _bounds;
    public Bounds Bounds { get { return _bounds; } }
    public float Radius // largest extents
    {
        get
        {
            //float max = _bounds.extents.x;
            //if (_bounds.extents.y < max)
            //    max = _bounds.extents.y;
            //if (_bounds.extents.z < max)
            //    max = _bounds.extents.z;
            //return max;
            return _bounds.extents.magnitude / 2f;
        }
    }
    public float Diameter { get { return Radius * 2f; } }
    public float Circumference {  get { return Mathf.PI * Diameter; } }

    private Vector3 _velo;
    public Vector3 Velocity
    {
        get { return _velo; }
    }

    public void ApplyVelo(Vector3 v)
    {
        _velo += v * Time.deltaTime;
        _velo = Vector3.ClampMagnitude(_velo, MaxSpeed);
    }

    public void DampenVelo()
    {
        _velo *= Dampening;
    }

    private void Start()
    {
        _bounds = BallMesh.GetComponent<MeshRenderer>().bounds;
        Debug.Log("Start diameter: " + Diameter);
    }

    private void Update()
    {
        _bounds.center = transform.position + _velo * Time.deltaTime;
        _updatePos();
        //BallMesh.transform.localScale = (Radius * 0.7f) * Vector3.one;
    }

    private void _updatePos()
    {
        transform.position += _velo * Time.deltaTime;

        // rotate relative to forward velo
        Vector3 perp = Vector3.Cross(Vector3.up, _velo); // rotate about the perpendicular axis
        transform.Rotate(perp, _velo.magnitude * Circumference * Time.deltaTime, Space.World);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Bounds.center, Radius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // pickup things on collision
        if(collision.transform.tag == "Pickup" && Bounds.Intersects(collision.collider.bounds))
        {
            Destroy(collision.rigidbody);
            collision.transform.SetParent(transform);
            collision.gameObject.layer = gameObject.layer;

            // add to bounds
            _bounds.Encapsulate(collision.collider.bounds);
            Debug.Log("Diameter: " + Diameter);

            // play sound
            _playRandomPickupClip();

            OnPickup.Invoke();
        }
    }

    private void _playRandomPickupClip()
    {
        SFXSource.PlayOneShot(PickupClips[Random.Range(0, PickupClips.Length)]);
    }
}