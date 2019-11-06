using Unity.Entities;
using UnityEngine;

namespace UnityTemplateProjects.ECS.Systems
{
	[UpdateInGroup(typeof(TestComponentSystemGroup))]
	public class TestSystemB : ComponentSystem
	{
		protected override void OnUpdate()
		{
			Debug.Log("TestSystemB");
		}
	}
}