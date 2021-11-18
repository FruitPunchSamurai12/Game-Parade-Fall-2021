using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToMenuButton : Button
{
    private static BackToMenuButton _instance;
    public static bool Pressed => _instance != null && _instance.IsPressed();

    protected override void OnEnable()
    {
        _instance = this;
        base.OnEnable();
    }
}


