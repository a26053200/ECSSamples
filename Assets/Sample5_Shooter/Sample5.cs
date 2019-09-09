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
        
        [SerializeField]
        public float bulletSpeed = 6f;
        
        [SerializeField]
        public float shootDeltaTime = 0.1f;
        
        [SerializeField]
        public float bulletScale = 0.2f;

        void Start()
        {
            Instance = this;
            EntityManager entityManager = World.Active.EntityManager;

            FireEntityArchetype = entityManager.CreateArchetype(typeof(Firing));
            
            BulletEntityArchetype = entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Rotation),
                typeof(WorldToLocal),
                typeof(Scale),
                typeof(Bullet)
            );
            
            //实体的本地数组
            Entity entity = gameObjectEntity.Entity;
            entityManager.AddComponent<Player>(entity);
            entityManager.AddComponent<Translation>(entity);
            entityManager.AddComponent<MoveSpeed>(entity);
            entityManager.AddComponent<PlayerInput>(entity);
            entityManager.AddComponent<Rotation>(entity);
            entityManager.AddComponent<LocalToWorld>(entity);
            entityManager.AddComponent<WorldToLocal>(entity);
            entityManager.SetComponentData(entity, new MoveSpeed(){Speed =  speed});
            
            entity = weaponEntity.Entity;
            entityManager.AddComponent<Weapon>(entity);
            entityManager.AddComponent<PlayerInput>(entity);
            entityManager.AddComponent<Rotation>(entity);
            entityManager.AddComponent<WorldToLocal>(entity);
            entityManager.AddComponent<LocalToWorld>(entity);
            //CreateBullet();
        }
    }
}
