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
                if (Input.GetButton("Mouse X"))
                {
                    var touchPosition = Input.mousePosition;
                    if (Camera.main != null)
                    {
                        var cameraRay = Camera.main.ScreenPointToRay(touchPosition);
                        var layerMask = LayerMask.GetMask("Floor");
                        if (Physics.Raycast(cameraRay, out var hit, 100, layerMask))
                        {
                            var forward = hit.point - transform.position;
                            var rotation = Quaternion.LookRotation(forward);
                            input.Rotation = new Quaternion(0,rotation.y,0, rotation.w);
                        }
                    }
                }
            });
        }
    }
}