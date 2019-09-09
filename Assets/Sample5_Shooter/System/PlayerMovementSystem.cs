using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Sample5_Shooter
{
    public class PlayerMovementSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation ,ref Rotation rotation, ref MoveSpeed moveSpeed, ref PlayerInput input) =>
            {
                var pos = translation.Value;
                pos += new float3(
                    input.Horizontal * Time.deltaTime * moveSpeed.Speed,
                    0,
                    input.Vertical * Time.deltaTime * moveSpeed.Speed);
                translation.Value = pos;
                var rot = input.Rotation;
                rotation.Value = rot;
            });
            Entities.ForEach((ref Translation translation ,ref Rotation rotation, ref Weapon weapon, ref PlayerInput input) =>
            {
                var rot = input.Rotation;
                rotation.Value = rot;
            });
        }
    }
}