using Unity.Entities;

namespace Sample5_Shooter
{
    public struct Firing : IComponentData
    {
        public float FireStartTime;
    }
    
    public struct IsFiring : IComponentData
    {
        public bool Value;
    }
}