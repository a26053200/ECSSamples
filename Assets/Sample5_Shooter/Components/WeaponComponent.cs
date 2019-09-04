using Unity.Entities;

namespace Sample5_Shooter
{

    public struct Weapon : IComponentData
    {
        
    }
    
    public class WeaponComponent : ComponentDataProxy<Weapon>
    {
        
    }
}