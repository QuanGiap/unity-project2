using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private const float MaxTimer = 1.0f;
    private float timer = MaxTimer;

    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get 
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }

    private RectTransform rect = null;
    protected RectTransform Rect 
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>();
                if (rect == null)
                {
                    rect = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect;
        }
    }
    public Transform Target { get; protected set; } = null;
    private Transform player = null;

    private IEnumerator IE_CountDown = null;
    private Action unRegister = null;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;
    public void Register(Transform target, Transform player, Action unRegister)
    {
        this.Target = target;
        this.player = player;
        this.unRegister = unRegister;
        StartCoroutine(RotateToTheTarget());
        StartTimer();
    }
    public void Restart()
    {
        timer = MaxTimer;
        StartTimer();
    }
    private void StartTimer()
    {
        if (IE_CountDown != null) { StopCoroutine(IE_CountDown);}
        IE_CountDown = CountDown();
        StartCoroutine(IE_CountDown);
    }

    private IEnumerator CountDown()
    {
        while (CanvasGroup.alpha < 1.0f)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }
        while (CanvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        unRegister();
        Destroy(gameObject);
    }

    IEnumerator RotateToTheTarget()
    {
        while (enabled)
        {
            if (Target)
            {
                tPos = Target.position;
                tRot = Target.rotation;
            }
            Vector3 direction = player.position - tPos;
            tRot = Quaternion.LookRotation(direction);
            tRot.z = -tRot.y;                          
            tRot.x = 0;
            tRot.y = 0;
            Vector3 nortDirection = new Vector3(0, 0, player.eulerAngles.y);
            Rect.localRotation = tRot * Quaternion.Euler(nortDirection);
            yield return null;
        }
    }
}
