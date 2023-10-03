using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class Farming_Crop_State : MonoBehaviour
{
    //나중에 지울 것
    #region Static_Inst
    public static Farming_Crop_State Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
    }
    #endregion
    [Header("Sprite")]
    [SerializeField] Sprite[] Crop_SP;
    [SerializeField] SpriteRenderer Crop_SR;

    /*
     * 작물의 나이
     * 작물의 성장 모습이나 수확 가능 여부를 판단하는데 활용
     * 
     * 타 스크립트에서 AGE를 호출하여 age값을 set 혹은 get
     */
    public int Days
    {
        set
        {
            day = value;
            //Crop_SR.sprite = Crop_SP[day];
        }
        get
        {
            return day;
        }
    }
    int day;

    public string kind;
}
