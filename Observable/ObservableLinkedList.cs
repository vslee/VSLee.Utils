using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace VSLee.Utils
{
	/// <summary>
	/// This class is a LinkedList that can be used in a WPF MVVM scenario. Composition was used instead of inheritance,
	/// because inheriting from LinkedList does not allow overriding its methods.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObservableLinkedList<T> : INotifyCollectionChanged, IEnumerable,
		ICollection<T>, IEnumerable<T>, IReadOnlyCollection<T>
	{ // https://stackoverflow.com/questions/6996425/observable-linkedlist
		private LinkedList<T> m_UnderLyingLinkedList;

		#region Variables accessors
		/// <summary>
		/// Because the list (underlying LinkedList<T>) also maintains an internal count, getting the Count property is an O(1) operation.
		/// </summary>
		public int Count
		{
			get { return m_UnderLyingLinkedList.Count; }
		}

		public LinkedListNode<T> First
		{
			get { return m_UnderLyingLinkedList.First; }
		}

		public LinkedListNode<T> Last
		{
			get { return m_UnderLyingLinkedList.Last; }
		}

		public bool IsReadOnly => ((ICollection<T>)m_UnderLyingLinkedList).IsReadOnly;
		#endregion

		#region Constructors
		public ObservableLinkedList()
		{
			m_UnderLyingLinkedList = new LinkedList<T>();
		}

		public ObservableLinkedList(IEnumerable<T> collection)
		{
			m_UnderLyingLinkedList = new LinkedList<T>(collection);
		}
		#endregion

		#region LinkedList<T> Composition
		public LinkedListNode<T> AddAfter(LinkedListNode<T> prevNode, T value)
		{
			LinkedListNode<T> ret = m_UnderLyingLinkedList.AddAfter(prevNode, value);
			OnNotifyCollectionChanged();
			return ret;
		}

		public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			m_UnderLyingLinkedList.AddAfter(node, newNode);
			OnNotifyCollectionChanged();
		}

		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			LinkedListNode<T> ret = m_UnderLyingLinkedList.AddBefore(node, value);
			OnNotifyCollectionChanged();
			return ret;
		}

		public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
		{
			m_UnderLyingLinkedList.AddBefore(node, newNode);
			OnNotifyCollectionChanged();
		}

		public LinkedListNode<T> AddFirst(T value)
		{
			LinkedListNode<T> ret = m_UnderLyingLinkedList.AddFirst(value);
			OnNotifyCollectionChanged();
			return ret;
		}

		public IList<LinkedListNode<T>> AddFirstRange(IEnumerable<T> values)
		{
			var ret = new List<LinkedListNode<T>>();
			foreach (var value in values)
				ret.Add(m_UnderLyingLinkedList.AddFirst(value));
			OnNotifyCollectionChanged();
			return ret;
		}

		public void AddFirst(LinkedListNode<T> node)
		{
			m_UnderLyingLinkedList.AddFirst(node);
			OnNotifyCollectionChanged();
		}

		public LinkedListNode<T> AddLast(T value)
		{
			LinkedListNode<T> ret = m_UnderLyingLinkedList.AddLast(value);
			OnNotifyCollectionChanged();
			return ret;
		}

		public void AddLast(LinkedListNode<T> node)
		{
			m_UnderLyingLinkedList.AddLast(node);
			OnNotifyCollectionChanged();
		}

		public void Clear()
		{
			m_UnderLyingLinkedList.Clear();
			OnNotifyCollectionChanged();
		}

		public bool Contains(T value)
		{
			return m_UnderLyingLinkedList.Contains(value);
		}

		public void CopyTo(T[] array, int index)
		{
			m_UnderLyingLinkedList.CopyTo(array, index);
		}

		public bool LinkedListEquals(object obj)
		{
			return m_UnderLyingLinkedList.Equals(obj);
		}

		public LinkedListNode<T> Find(T value)
		{
			return m_UnderLyingLinkedList.Find(value);
		}

		public LinkedListNode<T> FindLast(T value)
		{
			return m_UnderLyingLinkedList.FindLast(value);
		}

		public Type GetLinkedListType()
		{
			return m_UnderLyingLinkedList.GetType();
		}

		public bool Remove(T value)
		{
			bool ret = m_UnderLyingLinkedList.Remove(value);
			OnNotifyCollectionChanged();
			return ret;
		}

		public void Remove(LinkedListNode<T> node)
		{
			m_UnderLyingLinkedList.Remove(node);
			OnNotifyCollectionChanged();
		}

		public void RemoveFirst()
		{
			m_UnderLyingLinkedList.RemoveFirst();
			OnNotifyCollectionChanged();
		}

		public void RemoveLast()
		{
			m_UnderLyingLinkedList.RemoveLast();
			OnNotifyCollectionChanged();
		}

		/// <summary>
		/// Keeps removing last until count is less than or equal to maxCount (will not remove if already lower)
		/// </summary>
		/// <param name="maxCount"></param>
		public void RemoveLastPRN(int maxCount)
		{
			while (m_UnderLyingLinkedList.Count > maxCount)
				m_UnderLyingLinkedList.RemoveLast();
			OnNotifyCollectionChanged();
		}
		#endregion

		#region INotifyCollectionChanged Members

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public void OnNotifyCollectionChanged()
		{
			if (CollectionChanged != null)
			{
				CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (m_UnderLyingLinkedList as IEnumerable).GetEnumerator();
		}

		public void Add(T item)
		{
			((ICollection<T>)m_UnderLyingLinkedList).Add(item);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((ICollection<T>)m_UnderLyingLinkedList).GetEnumerator();
		}

		#endregion
	}
}
