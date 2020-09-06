using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class Magnesis : MonoBehaviour, IRune
{
    public new string name;
    public GameObject magnetSwivelPoint;
    public float magnesisSpeed = 5f;

    public float fRadius = 3.0f;

    private bool isActive = false;
    private bool isMagnesisReady = false;
    private bool isUsingMagnesis = false;
    private Camera cam;
    private RaycastHit hit;
    private GameObject targetLocation;


    public float turnSpeed = 4.0f;
    private Vector3 offset;

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
        offset = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 7.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            HandleMagnesis();
        }
        else if(!isActive)
        {
            UnreadyMagnesis();
        }

        if(isUsingMagnesis)
        {
            ActivateMagnesis();
        }
    }

    void LateUpdate()
    {
        if(isUsingMagnesis)
        {
            Around();
        }
    }

    private void HandleMagnesis()
    {
        if(Input.GetButtonDown("Use Rune") && !isMagnesisReady)
        {
            ReadyMagnesis();
        }
        else if(Input.GetButtonDown("Use Rune") && isMagnesisReady)
        {
            UnreadyMagnesis();
        }

        MagnesisRaycast();
        HandleCharacterRotation();
        HandleVFX();
    }

    private void ReadyMagnesis()
    {
        isMagnesisReady = true;
    }

    private void UnreadyMagnesis()
    {
        isMagnesisReady = false;
    }

    private void ActivateMagnesis()
    {
        //StartCoroutine(MagnesisTimeLimit());
    }

    private void DeactivateMagnesis()
    {
        isUsingMagnesis = false;
    }

    private void HandleCharacterRotation()
    {
        if(isMagnesisReady)
        {
            Vector3 lookPos = Camera.main.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            rotation *= Quaternion.Euler(0, 180, 0);
            transform.rotation = rotation;
        }
    }

    private void HandleVFX()
    {
        if(isUsingMagnesis)
        {
            magnetSwivelPoint.gameObject.SetActive(true);
        }
        else if(!isUsingMagnesis)
        {
            magnetSwivelPoint.gameObject.SetActive(false);
        }
    }

    private IEnumerator MagnesisTimeLimit()
    {
        yield return new WaitForSeconds(2);
        DeactivateMagnesis();
    }
    private void MagnesisRaycast()
    {
        if(isMagnesisReady)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetButtonDown("Use Rune 3") && hit.transform.tag == "Pickupable")
                {
                    isUsingMagnesis = true;
                    targetLocation = new GameObject();
                    targetLocation.transform.position = hit.transform.position;
                }
            }
        }
    }

    private void Around()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        targetLocation.transform.position = this.transform.position + offset;
        targetLocation.transform.LookAt(this.transform.position);
    }

}
