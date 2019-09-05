using System.ComponentModel;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Sample5_Shooter
{
    public class ClearShootingSystem : JobComponentSystem
    {
        private struct ClearShootingJob : IJobForEach<Firing>
        {
            public float CurrentTime;
//            public EntityQuery EntityQuery;
            public EntityCommandBuffer.Concurrent EntityCommandBuffer;
//            public ComponentDataFromEntity<Firing> ComponentDataFromEntity;
//            public void Execute(ref Firing firing, ref IsFiring isFiring)
//            {
//                
//            }

            public void Execute(ref Firing firing)
            {
                if (CurrentTime - firing.FireStartTime > 0.5f)
                {
                    //EntityCommandBuffer.RemoveComponent<Firing>(index,entity);
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new ClearShootingJob()
            {
                CurrentTime = Time.time,
//                EntityQuery = GetEntityQuery(ComponentType.ReadWrite<Firing>()),
                EntityCommandBuffer = new EntityCommandBuffer.Concurrent(),
//                ComponentDataFromEntity = GetComponentDataFromEntity<Firing>()
            };
            return job.Schedule(this,inputDeps);
        }
    }
}