using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Counter : MonoBehaviour
{
    TMPro.TextMeshProUGUI m_TextMeshPro;

    int _count = 0;
    int _required = 1;

    public void updateText()
    {
        m_TextMeshPro.text = $"{_count}/{_required}";
    }

    public int count
    {
        get
        {
            return _count;
        }
        set
        {
            _count = value;
            updateText();
        }
    }

    public int required
    {
        get { return _required; }
        set {
            _required = value;
            updateText();
        }
    }

    public void Start()
    {
        m_TextMeshPro = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        transform.LookAt(Player.instance._camera.transform);
    }
}
