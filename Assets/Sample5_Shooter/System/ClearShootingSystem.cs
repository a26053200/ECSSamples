using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Sample5_Shooter
{
    public class ClearShootingSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;

        protected override void OnCreate()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        private struct ClearShootingJob : IJobForEachWithEntity<Firing>
        {
            public float CurrentTime;
            [ReadOnly]
            public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, ref Firing firing)
            {
                if (CurrentTime - firing.FireStartTime < 0.5f)
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
                EntityCommandBuffer = _barrier.CreateCommandBuffer(),
            };
            inputDeps = job.Schedule(this, inputDeps);
            _barrier.AddJobHandleForProducer(inputDeps);
            return inputDeps;
        }
    }
}