using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    public GameObject bulletHit;
    private int rebounds = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        rebounds++;
        if (collision.gameObject.tag == "Sword")
        {
            rebounds --;
        }
        if (rebounds > 1 || collision.gameObject.tag == "Shield")
        {
            Hit(collision);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Instantiate(bulletHit, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void Hit(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;
        GameObject hit = Instantiate(bulletHit, position, transform.rotation);
        hit.transform.rotation = Quaternion.FromToRotation(hit.transform.up, contact.normal);
        Destroy(hit, 0.5f);
    }
}
