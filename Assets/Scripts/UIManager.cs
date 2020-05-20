using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI Label
    {
        get
        {
            if (_label == null) _label = GetComponentInChildren<TextMeshProUGUI>();
            return _label;
        }
        private set => _label = value;
    }

    private TMPro.TextMeshProUGUI _label;
    private void Awake()
    {
        GameManager.Instance.OnBlockPlaced += () => { Label.text = $"{GameManager.Instance.Score}"; };
        GameManager.Instance.OnBlockFailed += () => { Label.text = $"{GameManager.Instance.Score}\nTap to start"; };
        GameManager.Instance.OnRestart += () => { Label.text = $"{GameManager.Instance.Score}\nTap to place block"; };
    }
}
