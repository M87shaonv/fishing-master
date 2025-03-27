using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_HideSelf : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(HideSelf), 2f);
    }

    private void HideSelf()
    {
        gameObject.SetActive(false);
    }
}