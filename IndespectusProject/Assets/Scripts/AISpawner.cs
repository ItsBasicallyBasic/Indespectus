using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {
    
    [SerializeField]
    private GameObject enemyPrefab;

    private float spawnTimer;
    private bool spawnable = false;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(spawnable && Time.time > spawnTimer) {
            Spawn();
            spawnable = false;
        }
    }

    private void Spawn() {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    public void Spawnable() {
        spawnTimer = Time.time + 5;
        spawnable = true;
    }
}
