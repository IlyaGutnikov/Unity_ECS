using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class TestingJobOutput : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var job = new SimpleJob
        {
            A = 1,
            B = 2
        };

        var jobHandle = job.Schedule();
        jobHandle.Complete();
        
        Debug.Log(job.Result);
    }
    
}

public struct SimpleJob : IJob
{
    public int A;
    public int B;
    public int Result;
    
    public void Execute()
    {
        Result = A + B;
    }
}