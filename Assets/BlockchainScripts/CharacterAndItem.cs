using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAndItem : MonoBehaviour
{
    // Singleton instance
    public static CharacterAndItem Instance { get; private set; }

    // Variable to store the resource boost value
    public bool tough = false;
    public bool agile = false;
    public bool noel = false;
    public bool pirate = false;
    public bool hat = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetBoostValue()
    {
        tough = false;
        agile = false;
        noel = false;
        pirate = false;
        hat = false;
    }
}
