using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovementSystem : ComponentSystem
{
    ComponentType[] componentTypes;

    protected override void OnCreate()
    {
        base.OnCreate();
        componentTypes = new ComponentType[] {
            typeof(Transform),
            typeof(HybirdPlayerInput),
            typeof(HybirdMoveSpeed)
        };
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((Transform transform, HybirdPlayerInput input, HybirdMoveSpeed moveSpeed) =>
        {
            var position = transform.position;
            var rotation = transform.rotation;

            position.x += moveSpeed.speed * input.horizontal * Time.deltaTime;
            rotation.w += math.clamp(input.horizontal * Time.deltaTime, 0.5f, 0.5f);

            transform.position = position;
            transform.rotation = rotation;
        });
    }
}
