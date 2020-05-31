using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {
    
    #region Health System

    [SerializeField]
    [Tooltip("Maximum Health of player - float")]
    private float MAX_HEALTH; // Maximum Health of player - float
    [SerializeField]
    private float currentHealth; // Current Health of player - float

    // Fetch Current Health of the player
    public float GetHealth() {
        return currentHealth;
    }

    // Set Current Health of the player
    public void SetHealth(float newHealth) {
        currentHealth = newHealth;
    }

    // Reduce Current Health of the player
    public void LooseHealth(float dmg) {
        currentHealth -= dmg;
    }

    // Increase Current Health of the player
    public void GainHealth(float val) {
        currentHealth += val;
    }

    #endregion


    #region Essence System

    [SerializeField]
    [Tooltip("Maximum Essence of player - int")]
    private float MAX_ESSENCE; // Maximum Essence of player - int
    private float currentEssence; // Current Essence of player - int

    // Fetch Current Essence of the player
    public float GetEssence() {
        return currentEssence;
    }

    // Set Current Essence of the player
    public void SetEssence(float newEssence) {
        currentEssence = newEssence;
    }

    // Reduce Current Essence of the player
    public void LooseEssence(float dmg) {
        currentEssence -= dmg;
    }

    // Increase Current Essence of the player
    public void GainEssence(float val) {
        currentEssence += val;
    }

    #endregion

    // Reset Current Essence ans Health values to MAX values
    public void ResetResources() {
        currentHealth = MAX_HEALTH;
        currentEssence = MAX_ESSENCE;
    }
    
    // Start is called before the first frame update
    void Start() {
        ResetResources();
    }

    private void Update()
    {
        if (currentEssence > MAX_ESSENCE) currentEssence = MAX_ESSENCE;
        if (currentHealth > MAX_HEALTH) currentHealth = MAX_HEALTH;
    }
}
