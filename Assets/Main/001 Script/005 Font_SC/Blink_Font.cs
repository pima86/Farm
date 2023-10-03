using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blink_Font : MonoBehaviour
{
    /* 게임 첫 화면에서의 "press to start"와 같이
     * 반짝이는 텍스트 오브젝트를 위한 애니메이션입니다.
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
