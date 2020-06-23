using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPickup : MonoBehaviour
{
    public bool canPickUp = false;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.transform != null && other.gameObject.CompareTag("Pickupable"))
        {
            canPickUp = true;
        }
        else
        {
            canPickUp = false;
        }
    }
}
