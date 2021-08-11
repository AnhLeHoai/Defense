using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIImageFill : MonoBehaviour
{
    [SerializeField] private Image image;

    private Tween tween;
    

    public void SetValue(float percent, float valueChangeTime = 0.5f)
    {
        percent = Mathf.Clamp(percent, 0, 1);
        tween?.Kill();
        if (valueChangeTime == 0)
        {
            image.fillAmount = percent;
        }
        else
        {
            tween = image.DOFillAmount(percent, valueChangeTime).SetEase(Ease.InOutCubic);
            if (float.IsNaN(image.fillAmount))
            {
                image.fillAmount = percent;
            }
        }
        
    }
}