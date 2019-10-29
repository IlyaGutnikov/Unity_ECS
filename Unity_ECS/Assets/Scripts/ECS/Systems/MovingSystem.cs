using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MovingSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((ref Translation translation, ref MoveSpeedComponent moveSpeed) =>
		{
			translation.Value.y += moveSpeed.Speed * Time.deltaTime;

			if (translation.Value.y > 5f)
			{
				moveSpeed.Speed = -math.abs(moveSpeed.Speed);
			}
			
			if (translation.Value.y < -5f)
			{
				moveSpeed.Speed = +math.abs(moveSpeed.Speed);
			}
		});
	}
}