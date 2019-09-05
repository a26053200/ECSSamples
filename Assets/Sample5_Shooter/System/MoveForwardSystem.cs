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
                (ref Translation translation, ref PlayerInput input, ref MoveSpeed moveSpeed, ref Rotation rotation) =>
                {
                    Vector3 dir = new Vector3(1, 0, 1);
                    Vector3 moveDir = input.Rotation * dir;
                    //rotation.Value = input.Rotation;
                    //translation.Value.xyz += math.forward(q) * moveSpeed.Speed;
                    float3 s = Time.deltaTime * moveSpeed.Speed * moveDir;
                    translation.Value.xyz += s;
                });
        }
    }
}