using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Unity.Jobs;
using Unity.Collections;

public class Async : MonoBehaviour
{
    private void Start()
    {
        Debug.LogFormat("현 쓰레드 : {0}", Thread.CurrentThread.ManagedThreadId);
        StartCoroutine(CoJob());
    }

    IEnumerator CoJob()
    {
        var A = new NativeArray<int>(100, Allocator.TempJob);
        var B = new NativeArray<int>(100, Allocator.TempJob);
        var Result = new NativeArray<int>(100, Allocator.TempJob);
        for (int i = 0; i < 100; i++) { A[i] = i; yield return null; }
        for (int i = 0; i < 100; i++) { B[i] = i; yield return null; }

        TestJob testJob;
        testJob.a = A;
        testJob.b = B;
        testJob.result = Result;
        JobHandle Handle = testJob.Schedule(100, 20);

        while (!Handle.IsCompleted)
        {
            yield return null;
        }
        Handle.Complete();

        for (int i = 0; i < 100; i++) { Debug.LogFormat("결과 :{0}", Result[i]); yield return null; }

        A.Dispose();
        B.Dispose();
        Result.Dispose();
    }

    struct TestJob : IJobParallelFor
    {
        public NativeArray<int> a;
        public NativeArray<int> b;
        public NativeArray<int> result;

        public void Execute(int index)
        {
             result[index]= a[index] + b[index];
        }
    }
}
