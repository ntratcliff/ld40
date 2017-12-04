using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public GameObject CollectTitle, PaintTitle;
    public KatamariBall Ball;
    public SizeMeter SizeMeter;
    public float CollectTitleDelay;

    private float _collectTitleTime;
    
    private void Start()
    {
        //Ball.OnPickup.AddListener(_onPickup);
        SizeMeter.OnGoalReached.AddListener(_onGoalReached);
        _collectTitleTime = CollectTitleDelay;
    }

    //private void _onPickup()
    //{

    //}
    private void Update()
    {
        if (!CollectTitle.activeSelf && _collectTitleTime > 0)
            _collectTitleTime -= Time.deltaTime;
        else if (!CollectTitle.activeSelf)
        {
            CollectTitle.SetActive(true);
            //todo: sound
        }

    }

    private void _onGoalReached()
    {
        PaintTitle.SetActive(true);
        // todo: sound
    }
}