using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Sample4_Pure
{
    public class BootStrap : MonoBehaviour
    {
        
        [SerializeField]
        private Mesh mesh;

        [SerializeField]
        private Material material;
        
        [SerializeField]
        private float speed;

        void Start()
        {
            EntityManager entityManager = World.Active.EntityManager;

            EntityArchetype entityArchetype = entityManager.CreateArchetype(
                typeof(CompositeRotation),
                typeof(Rotation ),
                typeof(RotationEulerXYZ),
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(MoveSpeed),
                typeof(PlayerInput)
            );
            //实体的本地数组
            Entity entity = entityManager.CreateEntity(entityArchetype);
            
            entityManager.SetComponentData(entity, new MoveSpeed(){Speed =  speed});
            entityManager.SetComponentData(entity, new RotationEulerXYZ
            {
                Value = new float3(0, 135, 0),
            });
            entityManager.SetSharedComponentData(entity, new RenderMesh()
            {
                mesh = mesh,
                material = material,
                castShadows = ShadowCastingMode.On
            });
        }
    }
}
