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

    private float speed = 1f;

    public class Dragon
    {
        public Transform transform;
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
                transform = dragon,
                moveY = Random.Range(1f, 5f)
            });
        }

        
    }

    void Update()
    {
        //float startTime = Time.realtimeSinceStartup;
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
        if (useJobs)
        {
            NativeArray<float3> positions = new NativeArray<float3>(dragonList.Count, Allocator.TempJob);
            NativeArray<float> moveYs = new NativeArray<float>(dragonList.Count, Allocator.TempJob);

            for (int i = 0; i < dragonList.Count; i++)
            {
                positions[i] = dragonList[i].transform.position;
                moveYs[i] = dragonList[i].moveY;
            }
            ReallyTouchJobParallelFor reallyTouchJobParallelFor = new ReallyTouchJobParallelFor
            {
                deltaTime = Time.deltaTime,
                positions = positions,
                moveYs = moveYs,
            };
            reallyTouchJobParallelFor.Schedule(dragonList.Count, 100).Complete();

            for (int i = 0; i < dragonList.Count; i++)
            {
                dragonList[i].transform.position = positions[i];
                dragonList[i].moveY = moveYs[i];
            }

            positions.Dispose();
            moveYs.Dispose();
        }
        else
        {
            for (int i = 0; i < dragonList.Count; i++)
            {
                Dragon dragon = dragonList[i];
                Vector3 pos = dragon.transform.position;
                pos += new Vector3(0, dragon.moveY * Time.deltaTime, 0);
                if (pos.y > 5)
                    dragon.moveY = -math.abs(dragon.moveY);
                if (pos.y < -5)
                    dragon.moveY = +math.abs(dragon.moveY);
                dragonList[i].transform.position = pos;
                //莫妮卡
                float value = 0f;
                for (int j = 0; j < 1000; j++)
                {
                    value = math.exp10(math.sqrt(value));
                }
            }
        }
        
        //Debug.Log((Time.realtimeSinceStartup - startTime) * 1000f + "ms");
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

[BurstCompile]
public struct ReallyTouchJobParallelFor : IJobParallelFor
{
    public NativeArray<float3> positions;
    public NativeArray<float> moveYs;
    [ReadOnly] public float deltaTime;

    public void Execute(int i)
    {
        positions[i] += new float3(0,moveYs[i] * deltaTime, 0);
        if (positions[i].y > 5)
            moveYs[i] = -math.abs(moveYs[i]);
        if (positions[i].y < -5)
            moveYs[i] = +math.abs(moveYs[i]);
        //莫妮卡
        float value = 0f;
        for (int j = 0; j < 1000; j++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}