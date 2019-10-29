using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class Testing : MonoBehaviour
{
    void Start()
    {
        var entityManager = World.Active.EntityManager;

        var entityArchetype = entityManager.CreateArchetype(
            typeof(LevelComponent),
            typeof(Translation));
        
        var entities = new NativeArray<Entity>(2000, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entities);

        for (var i = 0; i < entities.Length; i++)
        {
            entityManager.SetComponentData(entities[i], new LevelComponent {Level = Random.Range(10, 200)});
        }

        entities.Dispose();
    }
}
