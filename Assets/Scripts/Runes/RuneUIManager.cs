using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;
using UnityEngine.Assertions.Must;

public class RuneUIManager : MonoBehaviour
{
    public GameObject RuneUIPanel;
    public RectTransform scrollPanel;
    public Button[] bttn;
    public RectTransform center;
    public int startButton = 1;
    public float snapSpeed = 10f;

    private IRune[] runes;
    private GameObject player;
    private int bttnDistance;
    private int minButtonNum;
    private int buttonIndex = 0;
    private bool shouldTimeStop = false;
    private bool runeUIActive = false;
    private bool hasLerped = false;

    private void Start()
    {
        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);

        scrollPanel.anchoredPosition = new Vector2((startButton - 1) * -125, 0f);

        player = GameObject.FindGameObjectWithTag("Player");

        var c = player.GetComponents<MonoBehaviour>();
        runes = (from a in c where a.GetType().GetInterfaces().Any(k => k == typeof(IRune)) select (IRune)a).ToArray();
    }

    private void Update()
    {
        SetUIActiveInactive();
        GoToButton();
        LerpToBttn(minButtonNum * -bttnDistance);
    }

    // Lerps the scroll panel to the appropriate position.
    private void LerpToBttn(int position)
    {
        float newX = Mathf.Lerp(scrollPanel.anchoredPosition.x, position, Time.unscaledDeltaTime * snapSpeed);
        Vector2 newPosition = new Vector2(newX, scrollPanel.anchoredPosition.y);

        scrollPanel.anchoredPosition = newPosition;
    }

    // Detects user input to determine which position to lerp to.
    private void GoToButton()
    {
        if (runeUIActive)
        {
            if (CrossPlatformInputManager.GetAxis("Controller X") > 0 && !hasLerped || CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") > 0 && !hasLerped)
            {
                if(buttonIndex > 0)
                {
                    buttonIndex = buttonIndex - 1;
                    minButtonNum = buttonIndex;
                    ActivateScript();
                    hasLerped = true;
                }
            }
            else if (CrossPlatformInputManager.GetAxis("Controller X") < 0 && !hasLerped || CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") < 0 && !hasLerped)
            {
                if(buttonIndex < 5)
                {
                    buttonIndex = buttonIndex + 1;
                    minButtonNum = buttonIndex;
                    ActivateScript();
                    hasLerped = true;
                }
            }
            else if(CrossPlatformInputManager.GetAxis("Controller X") == 0 && hasLerped)
            {
                hasLerped = false;
            }
        }
    }

    // As the name suggests, sets the Rune UI active or inactive depending on whether or not the Rune UI button is pressed. Also sets the time scale to 0 when the Rune UI is active and 1 when the Rune UI is inactive.
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

    // Sets the correct Rune script active and deactivates the rest. As of right now it just looks for the script with the same name as the button. 
    // This system is terrible and I am aware of the crappy jankiness of it. However, I am leaving it as is because...it works.
    private void ActivateScript()
    {
        foreach (IRune rune in runes)
        {
            if(bttn[minButtonNum].name == rune.ButtonName())
            {
                rune.isRuneActive(true);
            }
            else
            {
                rune.isRuneActive(false);
            }
        }
    }
}
