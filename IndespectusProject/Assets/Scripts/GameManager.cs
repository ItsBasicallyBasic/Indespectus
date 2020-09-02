using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public static GameManager GM = null;
    
    void Awake() {
        // Singleton
        if(GM == null) { 
             GM = this;
        } else if(GM != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
