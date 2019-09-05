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
        private struct PlayerShootingJob : IJobParallelFor
        {
            //public bool IsFire;
            public float FireStartTime;
            public EntityCommandBuffer.Concurrent EntityCommandBuffer;
//            public void Execute(ref Firing firing, ref IsFiring isFiring)
//            {
//                if (IsFire)
//                {
//                    isFiring.Value = IsFire;
//                    firing.FireStartTime = FireStartTime;
//                    
//                }
//            }
            public void Execute(int index)
            {
                if (FireStartTime > 0)
                {
                    var entity = EntityCommandBuffer.CreateEntity(index);
                    EntityCommandBuffer.SetComponent(index, entity, new Firing()
                    {
                        FireStartTime = FireStartTime
                    });
                }
            }
        }
            
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new PlayerShootingJob();
            if(Input.GetButton("Fire1"))
            {
                Debug.Log("ClearShootingJob");
                //job.IsFire = true;
                job.EntityCommandBuffer = new EntityCommandBuffer.Concurrent();
                job.FireStartTime = Time.time;
            }

            return job.Schedule(1,64,inputDeps);
        }
    }
}