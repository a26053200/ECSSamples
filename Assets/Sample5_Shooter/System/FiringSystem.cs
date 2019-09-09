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
//            Enabled = false;
        }

        private struct FiringJob : IJobForEachWithEntity_ECCC<Rotation, Firing, Translation>
        {
            public float FireStartTime;
            [ReadOnly] public EntityCommandBuffer EntityCommandBuffer;

            public void Execute(Entity entity, int index, ref Rotation rotation, ref Firing firing, ref Translation translation)
            {
                if (!firing.IsFired)
                {
                    CreateBullet(FireStartTime,translation, rotation, EntityCommandBuffer);
                    firing.IsFired = true;
                }
            }

            private void CreateBullet(float fireStartTime,Translation translation, Rotation rotation, EntityCommandBuffer buffer)
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
                    Speed = Sample5.Instance.bulletSpeed
                });
                buffer.SetComponent(entity, new Scale
                {
                    Value = Sample5.Instance.bulletScale
                });
                buffer.SetComponent(entity, translation);
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