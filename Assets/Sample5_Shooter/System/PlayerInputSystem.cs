using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Sample5_Shooter
{
    public class PlayerInputSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Transform transform ,ref PlayerInput input) =>
            {
                input.Horizontal = Input.GetAxis("Horizontal");
                input.Vertical = Input.GetAxis("Vertical");
            });
        }
    }
}