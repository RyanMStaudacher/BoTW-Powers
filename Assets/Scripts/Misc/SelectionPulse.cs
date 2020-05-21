using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPulse : MonoBehaviour
{
    public float scaleSpeed = 3f;

    private RectTransform t;
    private bool isScaledUp = false;

    void Start()
    {
        t = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(t.localScale.x >= 1 && !isScaledUp)
        {
            t.localScale = Vector3.MoveTowards(t.localScale, new Vector3(1.2f, 1.2f, 1), Time.unscaledDeltaTime * scaleSpeed);
        }
        else if(t.localScale.x <= 1.2f && isScaledUp)
        {
            t.localScale = Vector3.MoveTowards(t.localScale, new Vector3(1, 1, 1), Time.unscaledDeltaTime * scaleSpeed);
        }

        if(t.localScale.x == 1)
        {
            isScaledUp = false;
        }
        else if(t.localScale.x == 1.2f)
        {
            isScaledUp = true;
        }
    }
}
