using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Sample5_Shooter
{
    public class ClearShootingSystem : JobComponentSystem
    {
        private EntityQuery _query;
        private EndSimulationEntityCommandBufferSystem _barrier;

        private NativeArray<Firing> _firings;
        private NativeArray<Entity> _firingEntities;
        protected override void OnCreate()
        {
            _query = GetEntityQuery(ComponentType.ReadOnly<Firing>());
            _firingEntities = _query.ToEntityArray(Allocator.TempJob);
            _firings = _query.ToComponentDataArray<Firing>(Allocator.TempJob);
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        private struct ClearShootingJob : IJobParallelFor
        {
            [DeallocateOnJobCompletion]
            public NativeArray<Entity> Entities;
            public float CurrentTime;
            [ReadOnly]
            public EntityCommandBuffer EntityCommandBuffer;
            public NativeArray<Firing> Firings;

            public void Execute(int index)
            {
                var entity = Entities[index];
                if (CurrentTime - Firings[index].FireStartTime > 0.5f)
                {
                    EntityCommandBuffer.RemoveComponent<Firing>(entity);
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            
            var job = new ClearShootingJob()
            {
                CurrentTime = Time.time,
                Entities = _firingEntities,
                Firings = _firings,
                EntityCommandBuffer = _barrier.CreateCommandBuffer(),
            };
            inputDeps = job.Schedule(_firingEntities.Length, 64, inputDeps);

            _barrier.AddJobHandleForProducer(inputDeps);
            return inputDeps;
        }

        protected override void OnDestroy()
        {
            _firings.Dispose();
            _firingEntities.Dispose();
        }
    }
}