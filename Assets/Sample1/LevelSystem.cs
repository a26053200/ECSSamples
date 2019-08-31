using Unity.Entities;
using UnityEngine;

public class LevelSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref LevelComponent levelComponent) =>
        {
            levelComponent.level = 1 + Time.time;
            //Debug.Log(levelComponent.level);
        });
    }
}
