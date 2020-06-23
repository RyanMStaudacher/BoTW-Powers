using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRemoteBomb : MonoBehaviour, IRune
{
    public new string name;
    public GameObject bomb;
    public float throwForwardForce = 10f;
    public float throwUpForce = 10f;
    public float setDownSpeed = 5f;

    private GameObject bombInstance;
    private bool isActive = true;
    private bool shouldHold = false;
    private bool hasReleasedBomb = false;
    private Vector3 setDownTargetPosition;
    private BombPickup pickUp;

    // Name that allows button to activate the correct script
    // This is bad
    public string ButtonName()
    {
        return name;
    }

    // Uses interface function to determine if the Rune is active
    // Trash, but it works :D
    public void isRuneActive(bool a)
    {
        isActive = a;
    }

    // Start is called before the first frame update
    void Start()
    {
        pickUp = GetComponentInChildren<BombPickup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            HandleBomb();
        }
    }

    // Uses player input to handle the bomb accordingly
    private void HandleBomb()
    {
        if (Input.GetButtonDown("Use Rune") && bombInstance == null)
        {
            SpawnBomb();
        }
        else if (Input.GetButtonDown("Cancel") && bombInstance != null)
        {
            PutBombAway();
        }
        else if (Input.GetButtonDown("Throw Bomb") && bombInstance != null && !hasReleasedBomb)
        {
            ThrowBomb();
        }
        else if (Input.GetButtonDown("Use Rune 2") && bombInstance != null && !hasReleasedBomb)
        {
            SetBombDown();
        }
        else if (Input.GetButtonDown("Use Rune 2") && bombInstance != null && hasReleasedBomb && pickUp.canPickUp)
        {
            PickBombUp();
        }
        else if (Input.GetButtonDown("Use Rune") && bombInstance != null && hasReleasedBomb)
        {
            DetonateBomb();
        }
        else if (bombInstance != null && shouldHold)
        {
            HoldBomb();
        }
    }

    // Instantiates an instance of the bomb prefab
    private void SpawnBomb()
    {
        bombInstance = Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z), Quaternion.identity);
        shouldHold = true;
        hasReleasedBomb = false;
    }

    // Destroys bomb prefab instance
    private void PutBombAway()
    {
        Destroy(bombInstance);
    }

    // Makes bomb instance a child of the player character to simulate "holding" the bomb
    private void HoldBomb()
    {
        bombInstance.transform.parent = this.transform;
        shouldHold = true;
    }

    private void ThrowBomb()
    {
        shouldHold = false;
        bombInstance.transform.parent = null;
        bombInstance.GetComponent<BoxCollider>().enabled = true;
        bombInstance.GetComponent<Rigidbody>().isKinematic = false;
        if (!hasReleasedBomb)
        {
            bombInstance.GetComponent<Rigidbody>().AddForce((transform.forward * throwForwardForce) + (transform.up * throwUpForce));
            hasReleasedBomb = true;
        }
    }

    private void SetBombDown()
    {
        shouldHold = false;
        bombInstance.transform.parent = null;
        StartCoroutine(SetDownPickUpDelay());
    }

    private void PickBombUp()
    {
        bombInstance.GetComponent<BoxCollider>().enabled = false;
        bombInstance.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(SetDownPickUpDelay());
    }

    private void DetonateBomb()
    {
        bombInstance.GetComponentInChildren<RemoteBomb>().Detonate();
    }

    private IEnumerator SetDownPickUpDelay()
    {
        if (!hasReleasedBomb)
        {
            setDownTargetPosition = transform.position + transform.TransformDirection(new Vector3(0, 0.2f, 0.5f));
        }
        else if (hasReleasedBomb)
        {
            setDownTargetPosition = transform.position + transform.TransformDirection(new Vector3(0f, 1.8f, 0f));
        }

        while (bombInstance.transform.position != setDownTargetPosition)
        {
            bombInstance.transform.position = Vector3.MoveTowards(bombInstance.transform.position, setDownTargetPosition, setDownSpeed * Time.deltaTime);
            yield return null;
        }

        if (bombInstance.transform.position == setDownTargetPosition)
        {
            if (!hasReleasedBomb)
            {
                bombInstance.GetComponent<BoxCollider>().enabled = true;
                bombInstance.GetComponent<Rigidbody>().isKinematic = false;
                hasReleasedBomb = true;
            }
            else if (hasReleasedBomb)
            {
                hasReleasedBomb = false;
                shouldHold = true;
                bombInstance.transform.parent = this.transform;
            }
        }
    }
}
