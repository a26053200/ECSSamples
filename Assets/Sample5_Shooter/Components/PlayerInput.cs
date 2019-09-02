using System.Numerics;
using Unity.Entities;
using Quaternion = UnityEngine.Quaternion;

namespace Sample5_Shooter
{
    public struct PlayerInput : IComponentData
    {
        public float Horizontal;
        public float Vertical;
        public Quaternion Rotation;
    }
}