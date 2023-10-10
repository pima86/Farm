using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House_Col : MonoBehaviour
{
    [Header("In Object")]
    [SerializeField] GameObject[] in_obj;

    [Header("Out Object")]
    [SerializeField] GameObject[] out_obj;

    void OnTriggerEnter2D(Collider2D collider)
    {
        for(int i = 0; i < in_obj.Length; i++) 
            in_obj[i].SetActive(true);
        for (int i = 0; i < out_obj.Length; i++)
            out_obj[i].SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < in_obj.Length; i++)
            in_obj[i].SetActive(false);
        for (int i = 0; i < out_obj.Length; i++)
            out_obj[i].SetActive(true);
    }
}
