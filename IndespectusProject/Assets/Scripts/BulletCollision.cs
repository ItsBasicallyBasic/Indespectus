using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    public GameObject bulletHit;
    private int rebounds = 0;
    public int maxRebounds = 1;

    private void OnCollisionEnter(Collision collision)
    {
        // If bullet hits player it calls Hit and destroys itself.
        if (collision.gameObject.tag == "Player")
        {
            Hit(collision);
            Destroy(gameObject);
        }

        // Adding to rebound
        rebounds++;

        // If the bullet collides with a sword, then rebound is reduced by 1 to allow for an extra rebound for bullet deflection.
        if (collision.gameObject.tag == "Sword")
        {
            rebounds --;
        }

        // If the current amount of rebounds is more than the maximum allowed rebounds OR the bullet collides with the shield.
        if (rebounds > maxRebounds || collision.gameObject.tag == "Shield")
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
