using Unity.Entities;
using UnityEngine;

namespace UnityTemplateProjects.ECS.Systems
{
	[UpdateBefore(typeof(TestComponentSystemGroup))]
	public class TestSystemC : ComponentSystem
	{
		protected override void OnUpdate()
		{
			Debug.Log("TestSystemC");
		}
	}
}