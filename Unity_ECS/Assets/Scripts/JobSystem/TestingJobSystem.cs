using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class TestingJobSystem : MonoBehaviour
{
    [SerializeField]
    private bool _useJobs;
    
    // Update is called once per frame
    void Update()
    {
        var startTime = Time.realtimeSinceStartup;

        if (_useJobs)
        {
            var jobHandles = new NativeList<JobHandle>(Allocator.Temp);

            for (int i = 0; i < 10; i++)
            {
                var handle = JobTest();
                jobHandles.Add(handle);
            }
            
            JobHandle.CompleteAll(jobHandles);
            jobHandles.Dispose();
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                ReallyToughTask();
            }
        }

        Debug.Log((Time.realtimeSinceStartup - startTime) * 1000 + " ms");
    }

    private void ReallyToughTask()
    {
        var value = 0f;
        for (var i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }

    private JobHandle JobTest()
    {
        var job = new ReallyToughJob();
        return job.Schedule();
    }

    [BurstCompile]
    public struct ReallyToughJob : IJob
    {
        public void Execute()
        {
            var value = 0f;
            for (var i = 0; i < 50000; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }
    }
}
