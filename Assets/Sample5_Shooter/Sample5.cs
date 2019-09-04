using Sample4_Pure;
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
        [SerializeField]
        private GameObjectEntity gameObjectEntity;

        [SerializeField]
        private float speed;

        void Start()
        {
            EntityManager entityManager = World.Active.EntityManager;

            EntityArchetype entityArchetype = entityManager.CreateArchetype(
                typeof(MoveSpeed),
                typeof(PlayerInput),
                typeof(Translation),
                typeof(CompositeRotation),
                typeof(Rotation),
                typeof(RotationEulerXYZ),
                typeof(RenderMesh),
                typeof(LocalToWorld)
            );
            
            //实体的本地数组
            Entity entity = gameObjectEntity.Entity;
            entityManager.AddComponent<MoveSpeed>(entity);
            entityManager.AddComponent<PlayerInput>(entity);
            
            entityManager.SetComponentData(entity, new MoveSpeed(){Speed =  speed});
        }
    }
}
