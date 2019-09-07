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
                (ref Translation translation, ref MoveSpeed moveSpeed, ref Rotation rotation, ref WorldToLocal localToWorld) =>
                {
                    //Debug.Log(localToWorld.Forward);
                    var dir = math.mul(rotation.Value , new float3(1, 0, 1));
                    translation.Value.xyz += Time.deltaTime * moveSpeed.Speed * localToWorld.Forward;
                });
        }
    }
}