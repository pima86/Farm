using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Farming_Crop_DayPlus : MonoBehaviour
{
    /*
     * 심어진 작물이 하루가 지나면 1살씩 먹도록 +1해주는 구간을 JobSystem을 통해 계산
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
