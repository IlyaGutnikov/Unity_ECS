using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class TestingJobOutput : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // В целях необходимости синхронизации данных между основным потоком и Job-ом
        var result = new NativeArray<int>(1, Allocator.TempJob);
        var job = new SimpleJob
        {
            A = 1,
            B = 2,
            Result = result
        };

        var jobHandle = job.Schedule();
        jobHandle.Complete();
        
        Debug.Log(job.Result[0]);
        result.Dispose();
    }
    
}

public struct SimpleJob : IJob
{
    public int A;
    public int B;
    public NativeArray<int> Result;
    
    public void Execute()
    {
        Result[0] = A + B;
    }
}