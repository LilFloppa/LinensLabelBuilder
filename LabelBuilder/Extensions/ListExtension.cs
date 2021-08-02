using System;
using System.Collections.Generic;

namespace LabelBuilder.Models
{
	static class ListExtension
	{
		public static IEnumerable<List<T>> ChunkBy<T>(this List<T> list, int n)
		{
			for (int i = 0; i < list.Count; i += n)
				yield return list.GetRange(i, Math.Min(n, list.Count - i));
		}
	}
}
