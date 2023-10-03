using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class Farming_Crop_State : MonoBehaviour
{
    //���߿� ���� ��
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
     * �۹��� ����
     * �۹��� ���� ����̳� ��Ȯ ���� ���θ� �Ǵ��ϴµ� Ȱ��
     * 
     * Ÿ ��ũ��Ʈ���� AGE�� ȣ���Ͽ� age���� set Ȥ�� get
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
