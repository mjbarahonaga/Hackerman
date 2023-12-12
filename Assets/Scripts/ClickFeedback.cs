using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MEC;

public class ClickFeedback : MonoBehaviour
{
    public TextMeshProUGUI ValueClick;
    public CanvasGroup CanvasGroup;
    public ClickPool PoolReference;

    public float LifeTime = 2f;
    private float _startTime = 0f;
    private CoroutineHandle _updateCoroutine;

    public void InitClick(int amount, Vector3 pos)
    {
        transform.position = pos;
        ValueClick.text = "+" + amount.ToString();
        CanvasGroup.alpha = 0.5f;
        _startTime = Time.time;
        _updateCoroutine = Timing.RunCoroutine(MyUpdate());

    }

    public IEnumerator<float> MyUpdate()
    {
        float currentTime = 0f;
        float valueX = UnityEngine.Random.Range(-3f, 3f);
        float valueY = UnityEngine.Random.Range(-3f, 4f);
        float speedY = UnityEngine.Random.Range(1f, 3f);

        //Vector3 addPos = new Vector3(valueX, valueY, 0f);
        Vector3 upSpeed = new Vector3(0f, speedY, 0f);
        //transform.position += addPos;
        while (true)
        {
            currentTime = Time.time - _startTime;

            transform.position += upSpeed;
            CanvasGroup.alpha = Mathf.Lerp(1f, 0f, currentTime / LifeTime);
            if (currentTime >= LifeTime) PoolReference.Pool.Release(this);
            yield return Timing.WaitForOneFrame;
        }
        
    }


    public void TakeFromPool()
    { 
        gameObject.SetActive(true);

    }
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        Timing.KillCoroutines(_updateCoroutine);
    }
}
