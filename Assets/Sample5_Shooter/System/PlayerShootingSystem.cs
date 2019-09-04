using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Sample5_Shooter
{
    public class PlayerShootingSystem : JobComponentSystem
    {
        public struct PlayerShootingJob : IJobParallelFor, JobForEachExtensions.IBaseJobForEach
        {
            [ReadOnly]public NativeArray<Entity> Entities;
            public EntityCommandBuffer.Concurrent EntityCommandBuffer;
            public bool IsFiring;
            
            public void Execute(int index)
            {
                if (IsFiring)
                {
                    EntityCommandBuffer.AddComponent(index, Entities[index], new Firing());
                }
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new PlayerShootingJob()
            {

            };
            return job.Schedule(this, inputDeps);
        }
    }
}