using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blink_Font : MonoBehaviour
{
    /* ���� ù ȭ�鿡���� "press to start"�� ����
     * ��¦�̴� �ؽ�Ʈ ������Ʈ�� ���� �ִϸ��̼��Դϴ�.
     */
    TMP_Text txt;
    int plus_minus = 1;

    private void Start()
    {
        txt = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        txt.color += new Color(0,0,1) * plus_minus * Time.deltaTime;

        if (txt.color.a <= 0)
            plus_minus = 1;
        else if (txt.color.a >= 1)
            plus_minus = -1;
    }
}
