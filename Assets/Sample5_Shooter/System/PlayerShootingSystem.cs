using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sample5_Shooter
{
    public class PlayerShootingSystem : JobComponentSystem
    {
        private EntityQuery _query;
        private EndSimulationEntityCommandBufferSystem _barrier;
        private NativeArray<Entity> _weaponEntities;

        protected override void OnCreate()
        {
            _query = GetEntityQuery(
                ComponentType.ReadWrite<Weapon>(),
                ComponentType.Exclude<Firing>());
            //_weaponEntities = _query.ToEntityArray(Allocator.TempJob);
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        
        //[BurstCompile]
        private struct PlayerShootingJob : IJobParallelFor
        {
            public float FireStartTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;
            //[DeallocateOnJobCompletion] 
            public NativeArray<Entity> Entities;
            //public EntityQuery Query;
            public void Execute(int index)
            {
                EntityCommandBuffer.AddComponent(Entities[index], new Firing()
                {
                    FireStartTime = FireStartTime
                });
            }

//            public void Execute(ref Weapon weapon)
//            {
//                EntityCommandBuffer.AddComponent(Query, ComponentType.ReadOnly<Firing>());
//            }

//            public void Execute()
//            {
//                if (FireStartTime > 0)
//                {
//                    var entity = EntityCommandBuffer.CreateEntity();
//                    EntityCommandBuffer.AddComponent<Firing>(entity);
//                    EntityCommandBuffer.SetComponent(entity, new Firing()
//                    {
//                        FireStartTime = FireStartTime
//                    });
//                }
//            }

            
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (Input.GetButton("Fire1"))
            {
                _weaponEntities = _query.ToEntityArray(Allocator.TempJob);
                var playerShootingJob = new PlayerShootingJob()
                {
                    //Query = _query,
                    Entities = _weaponEntities,
                    EntityCommandBuffer = _barrier.CreateCommandBuffer(),
                    FireStartTime = Time.time
                };
                inputDeps = playerShootingJob.Schedule(_weaponEntities.Length,64, inputDeps);
                _barrier.AddJobHandleForProducer(inputDeps);
            }
            return inputDeps;
        }

        protected override void OnDestroy()
        {
            _weaponEntities.Dispose();
        }
    }
}