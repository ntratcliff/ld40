using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariBall : MonoBehaviour 
{
    public GameObject BallMesh;
    public AudioSource SFXSource;
    public AudioClip[] PickupClips;

    private Bounds _bounds;
    public Bounds Bounds { get { return _bounds; } }
    public float Radius // largest extents
    {
        get
        {
            float max = _bounds.extents.x;
            if (_bounds.extents.y > max)
                max = _bounds.extents.y;
            if (_bounds.extents.z > max)
                max = _bounds.extents.z;
            return max;
        }
    }
    public float Diameter { get { return Radius * 2f; } }
    public float Circumference {  get { return Mathf.PI * Diameter; } }

    private void Start()
    {
        _bounds = BallMesh.GetComponent<MeshRenderer>().bounds;
        Debug.Log("Start diameter: " + Diameter);
    }

    private void Update()
    {
        _bounds.center = transform.position;
        //_updatePos(); // handled in KatamariController.cs
    }

    private void _updatePos()
    {
        Vector3 pos = transform.localPosition;
        pos.y = Radius;
        transform.localPosition = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Bounds.center, Radius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // pickup things on collision
        if(collision.transform.tag == "Pickup")
        {
            Destroy(collision.rigidbody);
            collision.transform.SetParent(transform);
            collision.gameObject.layer = gameObject.layer;

            // add to bounds
            _bounds.Encapsulate(collision.collider.bounds);
            Debug.Log("Diameter: " + Diameter);

            // play sound
            _playRandomPickupClip();
        }
    }

    private void _playRandomPickupClip()
    {
        SFXSource.PlayOneShot(PickupClips[Random.Range(0, PickupClips.Length)]);
    }
}