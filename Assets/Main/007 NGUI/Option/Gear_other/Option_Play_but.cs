using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option_Play_but : MonoBehaviour
{
    [Header("�ɼ� �г�")]
    [SerializeField] GameObject Option_obj;

    public void OnPress()
    {
        Option_obj.SetActive(false);
    }
}
