using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Sample5_Shooter
{
    public class Sample5 : MonoBehaviour
    {
        public static Sample5 Instance;

        public static EntityArchetype FireEntityArchetype;
        
        public static EntityArchetype BulletEntityArchetype;
        
        [SerializeField]
        private GameObjectEntity gameObjectEntity;

        [SerializeField]
        private GameObjectEntity weaponEntity;
        
        [SerializeField]
        public Mesh mesh;

        [SerializeField]
        public Material material;
        
        [SerializeField]
        private float speed;

        void Start()
        {
            Instance = this;
            EntityManager entityManager = World.Active.EntityManager;

            FireEntityArchetype = entityManager.CreateArchetype(typeof(Firing));
            
            BulletEntityArchetype = entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld)
            );
            
            //实体的本地数组
            Entity entity = gameObjectEntity.Entity;
            entityManager.AddComponent<MoveSpeed>(entity);
            entityManager.AddComponent<PlayerInput>(entity);
            entityManager.AddComponent<Rotation>(entity);
            entityManager.AddComponent<LocalToWorld>(entity);
            entityManager.SetComponentData(entity, new MoveSpeed(){Speed =  speed});
            
            entity = weaponEntity.Entity;
            entityManager.AddComponent<Weapon>(entity);
            entityManager.AddComponent<LocalToWorld>(entity);
            //CreateBullet();
        }

        public static void CreateBullet(float fireStartTime, LocalToWorld localToWorld, EntityCommandBuffer buffer)
        {
            //Debug.Log("Generate a bullet");
            Entity entity = buffer.CreateEntity(BulletEntityArchetype);
            buffer.SetComponent(entity, localToWorld);
            buffer.SetComponent(entity, new Translation()
            {
                Value = localToWorld.Position
            });
            buffer.SetComponent(entity, new MoveSpeed
            {
                Speed = 6f
            });
//            entityManager.SetComponent(entity, new Firing
//            {
//                FireStartTime = fireStartTime
//            });
            buffer.SetSharedComponent(entity,new RenderMesh {
                mesh = Instance.mesh,
                material = Instance.material,
                castShadows = ShadowCastingMode.On,
                receiveShadows = true
            });
        }
    }
}
