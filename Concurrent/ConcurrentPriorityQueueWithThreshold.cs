using System;
using System.Collections.Generic;
using System.Text;

namespace VSLee.Utils
{
	public class ConcurrentPriorityQueueWithThreshold<TKey, TValue> :
		ConcurrentPriorityQueue<TKey, TValue>
		where TKey : IComparable<TKey>
	{
		int threshold;
		public ConcurrentPriorityQueueWithThreshold(int threshold)
		{
			this.threshold = threshold;
		}
	}
}
