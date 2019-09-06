using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Sample5_Shooter
{
    public class MoveForwardSystem : ComponentSystem
    {
//        private struct MoveForwardJob : IJobForEach<Translation, PlayerInput,MoveSpeed>
//        {
//            public void Execute(ref Translation translation, ref PlayerInput input, ref MoveSpeed moveSpeed)
//            {
//                var dir = input.Rotation.eulerAngles.normalized;
//                translation.Value.xyz += (float3)dir * moveSpeed.Speed;
//            }
//        }
//        
//        protected override JobHandle OnUpdate(JobHandle inputDeps)
//        {
//            var job = new MoveForwardJob();
//            return job.Schedule(this, inputDeps);
//        }

        protected override void OnUpdate()
        {
            Entities.ForEach(
                (ref Translation translation, ref MoveSpeed moveSpeed, ref WorldToLocal worldToLocal, ref LocalToWorld localToWorld) =>
                {
                    var old = worldToLocal.Value;
                    var s = Time.deltaTime * moveSpeed.Speed * worldToLocal.Forward;
                    var d3Position = worldToLocal.Position;
                    d3Position.xyz += s;
                    old.c3 = new float4(d3Position.xyz, old.c3.w);
                    worldToLocal.Value = old;
                    translation.Value = worldToLocal.Position;
                });
        }
    }
}