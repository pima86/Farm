using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class Farming_Crop_Main : MonoBehaviour
{
    //���߿� ���� ��
    #region Static_Inst
    public static Farming_Crop_Main Inst { get; private set; }
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
    public int AGE
    {
        set
        {
            age = value;
            Crop_SR.sprite = Crop_SP[age];
        }
        get
        {
            return age;
        }
    }
    int age;
}
