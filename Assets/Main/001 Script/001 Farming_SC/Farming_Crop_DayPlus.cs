using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Farming_Crop_DayPlus : MonoBehaviour
{
    /*
     * �ɾ��� �۹��� �Ϸ簡 ������ 1�쾿 �Ե��� +1���ִ� ������ JobSystem�� ���� ���
     */
    int[] CropLs = new int[100];

    private void Start()
    {
        for (int i = 0; i < 100; i++)
            CropLs[i] = Random.Range(0, 5);

        NextDay();
    }

    void NextDay()
    {
        NativeArray<int> cropLs = new NativeArray<int>(CropLs, Allocator.TempJob);

        NextDayJob nextDayJob;
        nextDayJob.CropLs = cropLs;

        JobHandle handle = nextDayJob.Schedule(100, 32);
        handle.Complete();

        for (int i = 0; i < 100; i++)
            CropLs[i] = nextDayJob.CropLs[i];

        cropLs.Dispose();
    }

    struct NextDayJob : IJobParallelFor
    {
        public NativeArray<int> CropLs;

        public void Execute(int index)
        {
            CropLs[index] += 1;
        }
    }
}
