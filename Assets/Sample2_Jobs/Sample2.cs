using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using Unity.Jobs;
using Unity.Burst;
using System.Collections.Generic;

public class Sample2 : MonoBehaviour
{
    //[SerializeField]
    //private Mesh mesh;

    //[SerializeField]
    //private Material material;

    [SerializeField]
    private Transform dragonPrefab;

    [SerializeField]
    private bool useJobs;

    private List<Dragon> dragonList;

    public class Dragon
    {
        public Transform dragon;
        public float moveY;
    }

    void Start()
    {
        EntityManager entityManager = World.Active.EntityManager;

        dragonList = new List<Dragon>();
        for (int i = 0; i < 1000; i++)
        {
            Transform dragon = Instantiate(dragonPrefab, new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0), Quaternion.identity);
            dragonList.Add(new Dragon
            {
                dragon = dragon,
                moveY = Random.Range(-5f, 5f)
            });
        }
    }

    void Update()
    {
        float startTime = Time.realtimeSinceStartup;
        //if (useJobs)
        //{
        //    NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(Allocator.Temp);
        //    for (int i = 0; i < 10; i++)
        //    {
        //        JobHandle jobHandle = ReallyTouchTaskJob();
        //        jobHandles.Add(jobHandle);
        //    }
        //    JobHandle.CompleteAll(jobHandles);
        //    jobHandles.Dispose();
        //}
        //else
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        ReallyTouchTask();
        //    }
        //}
        Debug.Log((Time.realtimeSinceStartup - startTime) * 1000f + "ms");
    }

    private void ReallyTouchTask()
    {
        float value = 0f;
        for (int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }

    private JobHandle ReallyTouchTaskJob()
    {
        ReallyTouchJob job = new ReallyTouchJob();
        return job.Schedule();
    }
}

[BurstCompile]
public struct ReallyTouchJob : IJob
{
    public void Execute()
    {
        float value = 0f;
        for (int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}