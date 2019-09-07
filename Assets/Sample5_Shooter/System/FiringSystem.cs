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
//        private NativeArray<Entity> _weaponEntities;
        private NativeArray<LocalToWorld> _localToWorlds;
        private NativeArray<WorldToLocal> _worldToLocals;
        private NativeArray<Rotation> _rotations;
        
        protected override void OnCreateManager()
        {
            _entityQuery = GetEntityQuery(
                ComponentType.ReadWrite<Firing>(),
                ComponentType.ReadWrite<LocalToWorld>(),
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadWrite<WorldToLocal>());
            _entityQuery.SetFilterChanged(ComponentType.ReadWrite<Firing>());
            _barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        private struct FiringJob : IJobParallelFor
        {
            public float FireStartTime;
            [ReadOnly]
            public EntityCommandBuffer EntityCommandBuffer;
            [ReadOnly]public NativeArray<LocalToWorld> LocalToWorlds;
            [ReadOnly]public NativeArray<WorldToLocal> WorldToLocals;
            [ReadOnly]public NativeArray<Rotation> Rotations;
//            public NativeArray<Entity> Entities;

            public void Execute(int index)
            {
                Sample5.CreateBullet(FireStartTime, LocalToWorlds[index], WorldToLocals[index],Rotations[index], EntityCommandBuffer);
            }
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
//            _weaponEntities = _entityQuery.ToEntityArray(Allocator.TempJob);
            _localToWorlds = _entityQuery.ToComponentDataArray<LocalToWorld>(Allocator.TempJob);
            _worldToLocals = _entityQuery.ToComponentDataArray<WorldToLocal>(Allocator.TempJob);
            _rotations = _entityQuery.ToComponentDataArray<Rotation>(Allocator.TempJob);
            var job = new FiringJob()
            {
                LocalToWorlds = _localToWorlds,
                WorldToLocals = _worldToLocals,
                Rotations = _rotations,
                FireStartTime = Time.time,
                EntityCommandBuffer = _barrier.CreateCommandBuffer()
            };
            return job.Schedule(_localToWorlds.Length, 64, inputDeps);
        }

        protected override void OnDestroyManager()
        {
            //_weaponEntities.Dispose();
            _worldToLocals.Dispose();
            _localToWorlds.Dispose();
            _rotations.Dispose();
        }
    }
}