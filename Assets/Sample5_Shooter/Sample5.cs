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
            
//            EntityArchetype entityArchetype = entityManager.CreateArchetype(
//                typeof(MoveSpeed),
//                typeof(PlayerInput),
//                typeof(Translation),
//                typeof(CompositeRotation),
//                typeof(Rotation),
//                typeof(RotationEulerXYZ),
//                typeof(RenderMesh),
//                typeof(LocalToWorld)
//            );
            
            //实体的本地数组
            Entity entity = gameObjectEntity.Entity;
            entityManager.AddComponent<MoveSpeed>(entity);
            entityManager.AddComponent<PlayerInput>(entity);
            entityManager.SetComponentData(entity, new MoveSpeed(){Speed =  speed});
            
            entity = weaponEntity.Entity;
            entityManager.AddComponent<Weapon>(entity);
//            entityManager.AddComponent<IsFiring>(entity);

            //CreateBullet();
        }

        public static void CreateBullet(float fireStartTime, EntityCommandBuffer buffer)
        {
            //Debug.Log("Generate a bullet");
            EntityManager entityManager = World.Active.EntityManager;
            EntityArchetype entityArchetype = entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(PlayerInput),
                typeof(CompositeRotation),
                typeof(Rotation ),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Firing)
            );
            Entity entity = buffer.CreateEntity(entityArchetype);
            entityManager.SetComponentData(entity, new Translation{
                Value = float3.zero
            });
            entityManager.SetComponentData(entity, new MoveSpeed
            {
                Speed = 6f
            });
            entityManager.SetComponentData(entity, new Firing
            {
                FireStartTime = fireStartTime
            });
            entityManager.SetSharedComponentData(entity,new RenderMesh {
                mesh = Instance.mesh,
                material = Instance.material,
                castShadows = ShadowCastingMode.On,
                receiveShadows = true
            });
        }
    }
}
