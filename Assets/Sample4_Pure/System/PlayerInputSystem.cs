using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Sample4_Pure
{
    public class PlayerInputSystem : JobComponentSystem
    {
        private struct PlayerInputJob : IJobForEach<PlayerInput>
        {
            public float Horizontal;
            
            public void Execute(ref PlayerInput input)
            {
                input.Horizontal = Horizontal;
            }
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new PlayerInputJob()
            {
                Horizontal = Input.GetAxis("Horizontal")
            };
            return job.Schedule(this, inputDeps);
        }
    }
}