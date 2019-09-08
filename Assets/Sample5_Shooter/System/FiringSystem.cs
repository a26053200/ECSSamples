using Sample5_Shooter;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Sample5_Shooter
{
    public class FiringSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _barrier;

        protected override void OnCreateManager()
        {
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        private struct FiringJob : IJobForEachWithEntity_ECCC<Rotation, Firing, LocalToWorld>
        {
            public float FireStartTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, ref Rotation rotation, ref Firing firing, ref LocalToWorld localToWorld)
            {
                CreateBullet(FireStartTime,localToWorld, rotation, EntityCommandBuffer);
            }

            private void CreateBullet(float fireStartTime,LocalToWorld localToWorld, Rotation rotation, EntityCommandBuffer buffer)
            {
                //Debug.Log("Generate a bullet");
                Entity entity = buffer.CreateEntity(Sample5.BulletEntityArchetype);
                buffer.SetComponent(entity, rotation);
                buffer.SetComponent(entity, new Bullet
                {
                    StartTime = fireStartTime
                });
                buffer.SetComponent(entity, new MoveSpeed
                {
                    Speed = 6f
                });
                buffer.SetComponent(entity, new Translation()
                {
                    Value = localToWorld.Position
                });
                buffer.SetSharedComponent(entity, new RenderMesh
                {
                    mesh = Sample5.Instance.mesh,
                    material = Sample5.Instance.material,
                    castShadows = ShadowCastingMode.On,
                    receiveShadows = true
                });
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new FiringJob()
            {
                FireStartTime = Time.time,
                EntityCommandBuffer = _barrier.CreateCommandBuffer()
            };
            return job.Schedule(this, inputDeps);
        }
    }
}