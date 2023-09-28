using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedStorage : MonoBehaviour
{
    public static SeedStorage inst { get; private set; }
    private void Awake()
    {
        inst = this;
    }

    [Header("Corn PF")]
    [SerializeField] GameObject Carrot_PF;

    public GameObject Spawn_Carrot()
    {
        var PF = Instantiate(Carrot_PF);
        return PF;
    }
}
