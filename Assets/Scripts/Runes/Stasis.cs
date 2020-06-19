using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stasis : MonoBehaviour, IRune
{
    public new string name;

    private bool isActive = false;

    public string ButtonName()
    {
        return name;
    }

    public void isRuneActive(bool a)
    {
        isActive = a;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            print("Stasis is active");
        }
    }
}
