using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFishUI : MonoBehaviour
{
    [SerializeField] RectTransform timerBar;
    private void OnEnable()
    {
        PlayerCombat.DeadFishUIEvent += TurnOnUI;
    }
    private void OnDisable()
    {
        PlayerCombat.DeadFishUIEvent -= TurnOnUI;
    }
    private void Start()
    {
        
    }

    void TurnOnUI()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        StartCoroutine(Countdown());
    }

    //Will tick visually every 10th of a second so it looks nice :D
    IEnumerator Countdown()
    {
        for(int i = 300; i > 0;  i--)
        {
            timerBar.localScale = new Vector3((float)i / 300f, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
