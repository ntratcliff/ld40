using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SizeMeter : MonoBehaviour
{
    public float Goal;
    public Image GoalImage;
    public Image CurrentImage;
    public Text SizeText;
    public float MeterLerpSpeed;

    public KatamariBall Ball;

    public UnityEvent OnGoalReached;

    private Vector2 _goalSize;
    private Vector2 _meterStartSize;
    private Vector2 _meterTargetSize;
    private string _sizeTextFormat;

    private float _ballSize;
    private float _startSize;
    private float _sizeMm // size in mm
    {
        get { return _ballSize * 1000f; }
    }
    private float _sizeCm // size in cm
    {
        get { return _ballSize * 100f; }
    }

    private float _progress;
    public float Progress
    {
        get { return _progress; }
    }

    // Use this for initialization
    void Start()
    {
        // get size of current image
        _meterStartSize = CurrentImage.rectTransform.localScale;

        // get size of goal image (shouldn't change, but just in case)
        _goalSize = GoalImage.rectTransform.localScale;

        // get original size text for formatting later
        _sizeTextFormat = SizeText.text;

        // register event listener
        Ball.OnPickup.AddListener(OnPickup);

        // set initial goal
        SetGoal(Goal);

        // call OnPickup to init
        OnPickup();
    }

    public void SetGoal(float goal)
    {
        Goal = goal;
        _startSize = Ball.Diameter;
    }

    private void Update()
    {
        // update meter size
        CurrentImage.rectTransform.localScale = MathHelper.Interpolate(
            CurrentImage.rectTransform.localScale,
            _meterTargetSize,
            MeterLerpSpeed * Time.deltaTime);
    }

    // called when the ball collects something
    public void OnPickup()
    {
        // update ball size
        _ballSize = Ball.Diameter;

        // calculate progress towards goal
        _progress = (_ballSize - _startSize) / (Goal - _startSize);
        if (_progress >= 1f && OnGoalReached != null)
            OnGoalReached.Invoke();

        _updateText();
        _updateMeter();
    }

    private void _updateText()
    {
        Debug.Log("m: " + _ballSize);
        Debug.Log("cm: " + _sizeCm);
        Debug.Log("mm: " + _sizeMm);
        // break size into m, cm, and mm
        float m = Mathf.FloorToInt(_ballSize);
        float cm = Mathf.FloorToInt(_sizeCm % 100);
        float mm = Mathf.FloorToInt(_sizeMm % 100 % 10);
        SizeText.text = string.Format(_sizeTextFormat, m, cm, mm);
    }

    private void _updateMeter()
    {
        _meterTargetSize = MathHelper.Interpolate(_meterStartSize, _goalSize, _progress);
    }
}