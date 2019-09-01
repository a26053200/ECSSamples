using UnityEngine;
using UnityEditor;
using Unity.Entities;

public class PlayerInputSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((HybirdPlayerInput input) =>
        {
            input.horizontal = Input.GetAxis("Horizontal");
        });
    }
}