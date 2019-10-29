using UnityEngine;
using Unity.Entities;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var entityManager = World.Active.EntityManager;
        var entity = entityManager.CreateEntity(typeof(LevelComponent));
        
        entityManager.SetComponentData(entity, new LevelComponent {Level = 10});
    }
}
