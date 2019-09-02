using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Sample4_Pure
{
    public class PlayerMovementSystem : JobComponentSystem
    {
        private struct PlayerMovementJob : IJobForEach<MoveSpeed, PlayerInput, Translation>
        {
            public float DeltaTime;
            
            public void Execute(ref MoveSpeed moveSpeed, ref PlayerInput input, ref Translation translation)
            {
                translation.Value.xyz += new float3(moveSpeed.Speed * input.Horizontal * DeltaTime,0,0);
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new PlayerMovementJob()
            {
                DeltaTime = Time.deltaTime
            };
            return job.Schedule(this,inputDeps);
        }
    }
}