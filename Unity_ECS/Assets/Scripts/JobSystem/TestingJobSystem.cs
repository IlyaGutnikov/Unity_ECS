using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestingJobSystem : MonoBehaviour
{
    [SerializeField] private bool _useJobs;

    [SerializeField] private Transform _pfZombie;
    private List<Zombie> _zombieList = new List<Zombie>();
    
    public class Zombie
    {
        public Transform Transform;
        public float MoveY;
    }

    private void Start()
    {
        for (var i = 0; i < 1000; i++)
        {
            var zombieTransform = Instantiate(_pfZombie, new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f)),
                Quaternion.identity);
            
            _zombieList.Add(new Zombie
            {
                Transform = zombieTransform,
                MoveY = Random.Range(1f, 2f)
            });
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var startTime = Time.realtimeSinceStartup;

        if (_useJobs)
        {
            // Обрабатываем данные на куче джобов
            var posArray = new NativeArray<float3>(_zombieList.Count, Allocator.TempJob);
            var moveArray = new NativeArray<float>(_zombieList.Count, Allocator.TempJob);

            for (var i = 0; i < _zombieList.Count; i++)
            {
                posArray[i] = _zombieList[i].Transform.position;
                moveArray[i] = _zombieList[i].MoveY;
            }

            var paralelJob = new ReallyToughParallelJob
            {
                DeltaTime = Time.deltaTime,
                PositionArray = posArray,
                MoveYArray = moveArray
            };

            var handle = paralelJob.Schedule(_zombieList.Count, 100);
            handle.Complete();

            // Применяем данные к GO
            for (var i = 0; i < _zombieList.Count; i++)
            {
                _zombieList[i].Transform.position = posArray[i];
                _zombieList[i].MoveY = moveArray[i];
            }

            posArray.Dispose();
            moveArray.Dispose();
        }
        else
        {
            foreach (var zombie in _zombieList)
            {
                zombie.Transform.position += new Vector3(0, zombie.MoveY * Time.deltaTime);

                if (zombie.Transform.position.y > 5f)
                {
                    zombie.MoveY = -math.abs(zombie.MoveY);
                }
			
                if (zombie.Transform.position.y < -5f)
                {
                    zombie.MoveY = +math.abs(zombie.MoveY);
                }
            }
        }

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

[BurstCompile]
public struct ReallyToughParallelJob : IJobParallelFor
{
    public NativeArray<float3> PositionArray;
    public NativeArray<float> MoveYArray;
    public float DeltaTime;
    
    public void Execute(int index)
    {
        var position = PositionArray[index];
        var moveY = MoveYArray[index];
        
        position += new float3(0, moveY * DeltaTime, 0);

        if (position.y > 5f)
        {
            moveY = -math.abs(moveY);
        }
			
        if (position.y < -5f)
        {
            moveY = +math.abs(moveY);
        }

        PositionArray[index] = position;
        MoveYArray[index] = moveY;
    }
}
