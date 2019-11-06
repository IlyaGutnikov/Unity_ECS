using Unity.Entities;
using UnityEngine;

namespace UnityTemplateProjects.ECS.Systems
{
	[UpdateInGroup(typeof(TestComponentSystemGroup)), UpdateAfter(typeof(TestSystemB))]
	public class TestSystemA : ComponentSystem
	{
		protected override void OnUpdate()
		{
			Debug.Log("TestSystemA");
		}
	}
}