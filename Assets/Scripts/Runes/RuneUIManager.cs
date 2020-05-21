using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class RuneUIManager : MonoBehaviour
{
    public GameObject RuneUIPanel;
    public RectTransform scrollPanel;
    public Button[] bttn;
    public RectTransform center;
    public int startButton = 1;
    public float snapSpeed = 10f;

    private float[] distance;
    private bool dragging = false;
    private int bttnDistance;
    private int minButtonNum;
    private int buttonIndex = 0;
    private bool targetNearestButton = true;
    private bool shouldTimeStop = false;
    private bool runeUIActive = false;
    private bool hasLerped = false;

    private void Start()
    {
        int bttnLength = bttn.Length;
        distance = new float[bttnLength];

        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);

        scrollPanel.anchoredPosition = new Vector2((startButton - 1) * -125, 0f);
    }

    private void Update()
    {
        SetUIActiveInactive();
        GoToButton();

        for (int i = 0; i < bttn.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);
        }

        if (targetNearestButton)
        {
            float minDistance = Mathf.Min(distance);

            for (int a = 0; a < bttn.Length; a++)
            {
                if (minDistance == distance[a])
                {
                    minButtonNum = a;
                }
            }
        }

        if (!dragging)
        {
            LerpToBttn(minButtonNum * -bttnDistance);
        }
    }

    void LerpToBttn(int position)
    {
        float newX = Mathf.Lerp(scrollPanel.anchoredPosition.x, position, Time.unscaledDeltaTime * snapSpeed);
        Vector2 newPosition = new Vector2(newX, scrollPanel.anchoredPosition.y);

        scrollPanel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;

        targetNearestButton = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    public void GoToButton()
    {
        targetNearestButton = false;

        if (runeUIActive)
        {
            if (CrossPlatformInputManager.GetAxis("Controller X") > 0 && !hasLerped || CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") > 0 && !hasLerped)
            {
                if(buttonIndex > 0)
                {
                    buttonIndex = buttonIndex - 1;
                    minButtonNum = buttonIndex;
                    hasLerped = true;
                }
            }
            else if (CrossPlatformInputManager.GetAxis("Controller X") < 0 && !hasLerped || CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") < 0 && !hasLerped)
            {
                if(buttonIndex < 5)
                {
                    buttonIndex = buttonIndex + 1;
                    minButtonNum = buttonIndex;
                    hasLerped = true;
                }
            }
            else if(CrossPlatformInputManager.GetAxis("Controller X") == 0 && hasLerped)
            {
                hasLerped = false;
            }
        }
    }

    private void SetUIActiveInactive()
    {
        if (CrossPlatformInputManager.GetButton("Rune UI") || CrossPlatformInputManager.GetAxis("Controller Rune UI") > 0)
        {
            runeUIActive = true;
            RuneUIPanel.SetActive(true);
            shouldTimeStop = true;
        }
        else
        {
            runeUIActive = false;
            RuneUIPanel.SetActive(false);
            shouldTimeStop = false;
        }

        if (!shouldTimeStop)
        {
            Time.timeScale = 1;
        }
        else if(shouldTimeStop)
        {
            Time.timeScale = 0;
        }
    }
}
