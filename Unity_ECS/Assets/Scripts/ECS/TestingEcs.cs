using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Random = UnityEngine.Random;

public class TestingEcs : MonoBehaviour
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
            typeof(LocalToWorld),
            typeof(MoveSpeedComponent));
        
        var entities = new NativeArray<Entity>(5000, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entities);

        for (var i = 0; i < entities.Length; i++)
        {
            entityManager.SetComponentData(entities[i], new LevelComponent {Level = Random.Range(10, 200)});
            entityManager.SetComponentData(entities[i], new MoveSpeedComponent() {Speed = 2f});
            entityManager.SetComponentData(entities[i], new Translation() {Value = new float3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0f)});
            entityManager.SetSharedComponentData(entities[i], new RenderMesh
            {
                mesh = _mesh,
                material = _material
            });
        }

        entities.Dispose();
    }
}
