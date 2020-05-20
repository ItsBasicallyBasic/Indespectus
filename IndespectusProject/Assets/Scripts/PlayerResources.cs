using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {
    
    #region Health System

    [SerializeField]
    [Tooltip("Maximum Health of player - int")]
    private int MAX_HEALTH; // Maximum Health of player - int
    private int currentHealth; // Current Health of player - int

    // Fetch Current Health of the player
    public int GetHealth() {
        return currentHealth;
    }

    // Set Current Health of the player
    public void SetHealth(int newHealth) {
        currentHealth = newHealth;
    }

    // Reduce Current Health of the player
    public void LooseHealth(int dmg) {
        currentHealth -= dmg;
        if(currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        throw new NotImplementedException();
    }

    // Increase Current Health of the player
    public void GainHealth(int val) {
        currentHealth += val;
    }

    #endregion


    #region Essence System

    [SerializeField]
    [Tooltip("Maximum Essence of player - int")]
    private int MAX_ESSENCE; // Maximum Essence of player - int
    private int currentEssence; // Current Essence of player - int

    // Fetch Current Essence of the player
    public int GetEssence() {
        return currentEssence;
    }

    // Set Current Essence of the player
    public void SetEssence(int newEssence) {
        currentEssence = newEssence;
    }

    // Reduce Current Essence of the player
    public void LooseEssence(int dmg) {
        currentEssence -= dmg;
    }

    // Use Current Essence of the player
    public bool UseEssence(int val) {
        if(currentEssence > val) {
            currentEssence -= val;
            return true; // enough essence, can do action, essence used
        } else {
            return false; // not enough essence, cant do action, no essence used
        }
    }

    // Increase Current Essence of the player
    public void GainEssence(int val) {
        currentEssence -= val;
    }

    #endregion

    // Reset Current Essence ans Health values to MAX values
    public void ResetResources() {
        currentHealth = MAX_HEALTH;
        currentHealth = MAX_ESSENCE;
    }
    
    // Start is called before the first frame update
    void Start() {
        ResetResources();
    }
}
