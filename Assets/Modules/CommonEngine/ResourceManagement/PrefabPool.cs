using System.Collections.Generic;
using UnityEngine;

namespace CommonEngine.ResourceManagement
{
	/// <summary>
	/// Internal pool for PrefabManager
	/// </summary>
	/// <typeparam name="T">Type of prefab</typeparam>
	internal class PrefabPool<T> where T : Component
	{
		private Queue<T> queue;

		public PrefabPool()
		{
			queue = new Queue<T>();
		}

		public T RetrieveQueueObject()
		{
			if (queue == null)
				return null;
			if (queue.Count == 0)
				return null;
			return queue.Dequeue();
		}


		public void AddQueueObject(T newObj)
		{
			queue.Enqueue(newObj);
		}
	}
}

