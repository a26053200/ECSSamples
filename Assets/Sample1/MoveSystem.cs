using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Sample1
{
    public class MoveSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref MoveSpeedComponent moveSpeed) =>
            {
                translation.Value.y += moveSpeed.speed * Time.deltaTime;
                if (translation.Value.y > 5f)
                    moveSpeed.speed = -math.abs(moveSpeed.speed);
                if (translation.Value.y < -5f)
                    moveSpeed.speed = +math.abs(moveSpeed.speed);
            });
        }
    }
}
