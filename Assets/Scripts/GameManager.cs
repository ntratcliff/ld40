using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class GameManager : MonoBehaviour 
{
    public GameObject MainTitle, CollectTitle, PaintTitle, HUD;
    public KatamariBall Ball;
    public SizeMeter SizeMeter;
    public PostProcessingProfile PPP;
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
        if(MainTitle.activeSelf)
        {
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                MainTitle.SetActive(false);
                HUD.SetActive(true);
            }

            return; // don't do anything else until title is gone!
        }

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