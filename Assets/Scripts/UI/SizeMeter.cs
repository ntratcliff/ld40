using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeMeter : MonoBehaviour
{
    public float Goal;
    public Image GoalImage;
    public Image CurrentImage;
    public Text SizeText;

    public KatamariBall Ball;

    private Vector2 _goalSize;
    private Vector2 _meterStartSize;
    private string _sizeTextFormat;

    private float _ballSize;
    private float _sizeMm // size in mm
    {
        get { return _ballSize * 1000f; }
    }
    private float _sizeCm // size in cm
    {
        get { return _ballSize * 100f; }
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
    }

    // Update is called once per frame
    void Update()
    {
        // update ball size
        _ballSize = Ball.Diameter;

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

    }
}