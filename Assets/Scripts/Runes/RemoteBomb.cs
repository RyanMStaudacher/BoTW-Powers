using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteBomb : MonoBehaviour
{
    public Vector3 explosionSize;
    public float explosionSpeed;
    public float explosionForce;

    private Rigidbody rb;
    private SphereCollider coll;
    private bool isExploding = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        coll = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Detonate()
    {
        GetComponentInParent<MeshRenderer>().enabled = false;
        GetComponent<MeshRenderer>().enabled = true;
        foreach (Collider c in GetComponentsInParent<Collider>())
        {
            c.enabled = false;
        }
        coll.enabled = true;
        isExploding = true;
        rb.isKinematic = true;
        StartCoroutine(Explosion());
    }

    private IEnumerator Explosion()
    {
        while(transform.localScale != explosionSize)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, explosionSize, explosionSpeed * Time.deltaTime);
            yield return null;
        }

        if(transform.localScale == explosionSize)
        {
            StartCoroutine(DestroyDelay());
        }
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject.transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Pickupable") && isExploding)
        {
            other.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, 2.5f);
        }
    }
}
