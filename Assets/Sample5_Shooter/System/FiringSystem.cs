using Sample5_Shooter;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Sample5_Shooter
{
    public class FiringSystem : JobComponentSystem
    {
        private EntityQuery _entityQuery;
        private EndSimulationEntityCommandBufferSystem _barrier;
        private NativeArray<Entity> _weaponEntities;
        private NativeArray<LocalToWorld> _localToWorlds;
        
        protected override void OnCreateManager()
        {
            _entityQuery = GetEntityQuery(
                ComponentType.ReadWrite<Firing>(),
                ComponentType.ReadWrite<LocalToWorld>());
            _entityQuery.SetFilterChanged(ComponentType.ReadWrite<Firing>());
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        private struct FiringJob : IJobParallelFor
        {
            public float FireStartTime;
            [ReadOnly]
            public EntityCommandBuffer EntityCommandBuffer;
            public NativeArray<LocalToWorld> LocalToWorlds;
//            public NativeArray<Rotation> Rotations;
//            public NativeArray<Entity> Entities;

            public void Execute(int index)
            {
                Sample5.CreateBullet(FireStartTime, LocalToWorlds[index], EntityCommandBuffer);
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            _weaponEntities = _entityQuery.ToEntityArray(Allocator.TempJob);
            _localToWorlds = _entityQuery.ToComponentDataArray<LocalToWorld>(Allocator.TempJob);
            var job = new FiringJob()
            {
                LocalToWorlds = _localToWorlds,
//                Entities = _weaponEntities,
                FireStartTime = Time.time,
                EntityCommandBuffer = _barrier.CreateCommandBuffer()
            };
            return job.Schedule(_weaponEntities.Length, 64, inputDeps);
        }

        protected override void OnDestroyManager()
        {
            _weaponEntities.Dispose();
            _localToWorlds.Dispose();
        }
    }
}