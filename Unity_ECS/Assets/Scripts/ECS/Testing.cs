using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class Testing : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    
    void Start()
    {
        var entityManager = World.Active.EntityManager;

        var entityArchetype = entityManager.CreateArchetype(
            typeof(LevelComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld));
        
        var entities = new NativeArray<Entity>(1, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entities);

        for (var i = 0; i < entities.Length; i++)
        {
            entityManager.SetComponentData(entities[i], new LevelComponent {Level = Random.Range(10, 200)});
            entityManager.SetSharedComponentData(entities[i], new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });
        }

        entities.Dispose();
    }
}
