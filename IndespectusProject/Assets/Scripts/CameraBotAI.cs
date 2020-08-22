using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBotAI : MonoBehaviour
{
    // Scout variables
    public Transform scoutLocationsParent;
    public List<Transform> scoutLocations;
    public Transform nextLocation;
    public Transform lookTarget;
    public float speed = 1;
    private bool isWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        // Add each scout point to scoutLocations list
        foreach (Transform child in scoutLocationsParent)
        {
            scoutLocations.Add(child);
        }

        DecideNewLocation();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(lookTarget);
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,nextLocation.position, step);

        if(Vector3.Distance(nextLocation.position, transform.position) < 1f && !isWaiting)
        {
            //isWaiting = true;
            //StartCoroutine(wait());
            DecideNewLocation();
        }
    }

    // Bot wait at location before moving to next
    //IEnumerator wait()
    //{
        //yield return new WaitForSeconds(Random.Range(0, 3));
        //DecideNewLocation();
        //isWaiting = false;
    //}

    void DecideNewLocation()
    {
        // Pick random nextLocation
        int randomIndex = Random.Range(0, scoutLocations.Count);
        nextLocation = scoutLocations[randomIndex];
    }
}
