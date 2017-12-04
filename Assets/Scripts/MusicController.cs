using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicController : MonoBehaviour 
{
    public AudioSource A, B;
    public float AOutSpeed;
    public float BInSpeed;


    private bool _fading = false;
    private float _aT, _bT;

    public UnityEvent OnCrossfadeFinished;

    // fade from A to B
    public void CrossfadeTracks()
    {
        if (_aT >= 1 || _bT >= 1) return;

        // start B
        B.Play();

        _fading = true;
    }

    private void Update()
    {
        if (!_fading) return;

        _aT += AOutSpeed * Time.deltaTime;
        _bT += BInSpeed * Time.deltaTime;

        A.volume = Mathf.Lerp(1, 0, Mathf.Lerp(0, 1, _aT));
        B.volume = Mathf.Lerp(0, 1, Mathf.Lerp(0, 1, _bT));

        if (_aT >= 1 && _bT >= 1) _onCrossfadeFinished();
    }

    private void _onCrossfadeFinished()
    {
        _fading = false;
        if (OnCrossfadeFinished != null)
            OnCrossfadeFinished.Invoke();
    }
}