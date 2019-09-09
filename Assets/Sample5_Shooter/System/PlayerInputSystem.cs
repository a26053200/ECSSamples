using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Sample5_Shooter
{
    public class PlayerInputSystem : ComponentSystem
    {
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        protected override void OnUpdate()
        {
            Entities.ForEach((Transform transform ,ref PlayerInput input) =>
            {
                input.Horizontal = Input.GetAxis(HORIZONTAL);
                input.Vertical = Input.GetAxis(VERTICAL);
            });
        }
    }
}