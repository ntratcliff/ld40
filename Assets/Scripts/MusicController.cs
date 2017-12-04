using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour 
{
    public AudioSource A, B;
    public float fadeSpeed;

    private bool _fading = false;
    private float _t = 0;

    // fade from A to B
    public void CrossfadeTracks()
    {
        if (_t >= 1) return;

        _fading = true;
    }

    private void Update()
    {
        if (!_fading) return;

        _t += fadeSpeed * Time.deltaTime;

        A.volume = Mathf.Lerp(1, 0, Mathf.Lerp(0, 1, _t));
        B.volume = Mathf.Lerp(0, 1, Mathf.Lerp(0, 1, _t));

        if (_t >= 1) _fading = false;
    }
}