using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Cryonis : MonoBehaviour, IRune
{
    public new string name;
    public GameObject iceBlock;
    public List<GameObject> iceBlockList;

    private RaycastHit hit;
    private Camera cam;
    private bool isActive = false;
    private bool shouldActivate = false;
    private int iceBlockCount = 0;

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
        cam = Camera.main;
        iceBlockList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldActivate == false && Input.GetButtonDown("Use Rune"))
        {
            shouldActivate = true;
        }
        else if (shouldActivate == true && Input.GetButtonDown("Use Rune"))
        {
            shouldActivate = false;
        }

        if (isActive && shouldActivate)
        {
            ActivateCryonis();
        }
        else if(!isActive || !shouldActivate)
        {
            DeactivateCryonis();
        }
    }

    private void ActivateCryonis()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Physics.Raycast(ray, out hit);

        if(hit.transform != null)
        {
            if (hit.transform.tag == "Water" && Input.GetButtonDown("Use Rune 3"))
            {
                SpawnCube();
            }
        }
    }

    private void DeactivateCryonis()
    {
        bool hasDeactivated;


    }

    private void SpawnCube()
    {
        GameObject block;
        block = Instantiate(iceBlock, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

        block.transform.Translate(transform.up * 1.35f);

        //Makes block look at player's x and z position
        //block.transform.LookAt(new Vector3(transform.position.x, block.transform.position.y, transform.position.z));

        iceBlockList.Add(block);
        iceBlockCount++;

        if (iceBlockCount > 3)
        {
            var i = iceBlockList[0];

            iceBlockList.Remove(i);
            Destroy(i);
        }
    }
}
