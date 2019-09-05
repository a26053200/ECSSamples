using System.Threading;
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
        private NativeList<Entity> _deck;
        private EntityQuery _query;
        private EndSimulationEntityCommandBufferSystem _barrier;
        private NativeArray<Entity> _weaponEntities;

        protected override void OnCreate()
        {
            _deck = new NativeList<Entity>(64, Allocator.Persistent);
            _query = GetEntityQuery(ComponentType.ReadOnly<Weapon>());

            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        private struct AddToDeckJob : IJob
        {
            public NativeList<Entity> Deck;
            [DeallocateOnJobCompletion] public NativeArray<Entity> Entities;

            public void Execute()
            {
                for (var i = 0; i < Entities.Length; ++i)
                    Deck.Add(Entities[i]);
            }
        }

        private struct PlayerShootingJob : IJobParallelFor
        {
            public float FireStartTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;
            [DeallocateOnJobCompletion] public NativeArray<Entity> Entities;

            public void Execute(int index)
            {
                if (FireStartTime > 0)
                {
                    EntityCommandBuffer.AddComponent<Firing>(Entities[index]);
                    EntityCommandBuffer.SetComponent(Entities[index], new Firing()
                    {
                        FireStartTime = FireStartTime
                    });
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (Input.GetButton("Fire1"))
            {
                _weaponEntities = _query.ToEntityArray(Allocator.TempJob);
                var addToDeckJob = new AddToDeckJob
                {
                    Deck = _deck,
                    Entities = _weaponEntities,
                };
                var job = new PlayerShootingJob()
                {
                    Entities = _weaponEntities,
                    EntityCommandBuffer = _barrier.CreateCommandBuffer(),
                    FireStartTime = Time.time
                };
                inputDeps = addToDeckJob.Schedule(inputDeps);
//                Debug.Log("Create a bullet");
                inputDeps = job.Schedule(_weaponEntities.Length, 64, inputDeps);
                _barrier.AddJobHandleForProducer(inputDeps);
            }

            return inputDeps;
        }

        protected override void OnDestroy()
        {
            _deck.Dispose();
        }
    }
}