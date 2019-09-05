using Sample5_Shooter;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Sample5_Shooter
{
//    public class FiringSystem : JobComponentSystem
//    {
//        private EntityQuery _entityQuery;
//
//        protected override void OnCreateManager()
//        {
//            _entityQuery = GetEntityQuery(
//                ComponentType.Exclude<Firing>(),
//                ComponentType.Exclude<IsFiring>());
//            _entityQuery.SetFilterChanged(ComponentType.ReadWrite<Firing>());
//        }
//        
//        private struct FiringJob : IJobForEach<Firing>
//        {
//            public float FireStartTime;
//            public void Execute(ref Firing firing)
//            {
//                Sample5.CreateBullet(FireStartTime);
//            }
//        }
//        protected override JobHandle OnUpdate(JobHandle inputDeps)
//        {
//            var job = new FiringJob()
//            {
//                FireStartTime = Time.time
//            };
//            return job.Schedule(_entityQuery, inputDeps);
//        }
//    }
}