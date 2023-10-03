using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Purchasing;

public class Player_State : MonoBehaviour
{
    [Header("HP & MP")]
    public int HP;
    public int MP;

    public void HP_Damage(int damage)
    {
        NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

        DamageJob damageJob;
        damageJob.hp = HP;
        damageJob.damage = damage;
        damageJob.result = result;
        JobHandle handle = damageJob.Schedule();

        handle.Complete();

        HP = result[0];

        result.Dispose();
    }


    struct DamageJob : IJob
    {
        public int hp;
        public int damage;
        public NativeArray<int> result;

        public void Execute()
        {
            int hp_damage;
            if (hp < damage)
                hp_damage = 0;
            else
                hp_damage = hp + damage;

            result[0] = hp_damage;
        }
    }
}
