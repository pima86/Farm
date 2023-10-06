using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [Header("옵션 패널")]
    [SerializeField] GameObject Option_obj;

    public void OnPress()
    {
        Option_obj.SetActive(true);
    }
}
