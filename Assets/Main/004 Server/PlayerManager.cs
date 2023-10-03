using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Client;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int id;
    public string username;
}