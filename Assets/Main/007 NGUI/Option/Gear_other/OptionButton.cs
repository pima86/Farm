using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [Header("�ɼ� �г�")]
    [SerializeField] GameObject Option_obj;

    public void OnPress()
    {
        Option_obj.SetActive(true);
    }
}
