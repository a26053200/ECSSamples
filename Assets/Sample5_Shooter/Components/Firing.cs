using Unity.Entities;

namespace Sample5_Shooter
{
    public struct Firing : IComponentData
    {
        public float FireStartTime;
        public bool IsFired;
    }
}