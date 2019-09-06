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
            Entities.ForEach((Transform transform ,ref Rotation rotation, ref MoveSpeed moveSpeed, ref PlayerInput input) =>
            {
                var pos = transform.position;
                pos += new Vector3(
                    input.Horizontal * Time.deltaTime * moveSpeed.Speed,
                    0,
                    input.Vertical * Time.deltaTime * moveSpeed.Speed);
                transform.position = pos;
                var rot = input.Rotation;
                transform.rotation = rot;
                rotation.Value = rot;
            });
        }
    }
}