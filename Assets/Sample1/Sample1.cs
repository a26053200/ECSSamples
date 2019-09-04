using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Sample1
{
    public class Sample1 : MonoBehaviour
    {
        [SerializeField]
        private Mesh mesh;

        [SerializeField]
        private Material material;

        void Start()
        {
            EntityManager entityManager = World.Active.EntityManager;

                EntityArchetype entityArchetype = entityManager.CreateArchetype(
                   typeof(LevelComponent),
                   typeof(Translation),
                   typeof(CompositeRotation),
                   typeof(Rotation ),
                   typeof(RotationEulerXYZ),
                   typeof(RenderMesh),
                   typeof(LocalToWorld),
                   typeof(MoveSpeedComponent)
                    );
            //实体的本地数组
            NativeArray<Entity> nativeArray = new NativeArray<Entity>(100, Allocator.Temp);
            entityManager.CreateEntity(entityArchetype, nativeArray);

            for (int i = 0; i < nativeArray.Length; i++)
            {
                entityManager.SetComponentData(nativeArray[i], new LevelComponent {
                    level = Random.Range(10, 20)
                });
                entityManager.SetComponentData(nativeArray[i], new MoveSpeedComponent {
                    speed = Random.Range(-2f, 2f)
                });
                entityManager.SetComponentData(nativeArray[i], new Translation{
                        Value = new float3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0)
                    });
                entityManager.SetComponentData(nativeArray[i], new RotationEulerXYZ
                {
                    Value = new float3(0, 135, 0),
                });

                entityManager.SetSharedComponentData(nativeArray[i],new RenderMesh {
                    mesh = mesh,
                    material = material,
                });
            }

            nativeArray.Dispose();
        }

        void Update()
        {
            
        }
    }
}
