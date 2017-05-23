using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSLee.Utils
{
	public class ObservableSortedDoubleDictionary<TKey, TValue> : 
		IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
	{ // From http://blogs.microsoft.co.il/shimmy/2010/12/26/observabledictionarylttkey-tvaluegt-c/
		private const string CountString = "Count";
		private const string IndexerName = "Item[]";
		private const string KeysName = "Keys";
		private const string ValuesName = "Values";

		private SortedDictionary<TKey, TValue> forwardSortedDictionary;
		internal Dictionary<TValue, TKey> reverseDictionary;

		#region Constructors

		public ObservableSortedDoubleDictionary()
		{
			forwardSortedDictionary = new SortedDictionary<TKey, TValue>();
			reverseDictionary = new Dictionary<TValue, TKey>();
		}
		public ObservableSortedDoubleDictionary(IDictionary<TKey, TValue> dictionary)
		{
			forwardSortedDictionary = new SortedDictionary<TKey, TValue>(dictionary);
			reverseDictionary = new Dictionary<TValue, TKey>(dictionary.ToDictionary(
				kvp => kvp.Value, kvp => kvp.Key));
		}
		//public ObservableDictionary(IComparer<TKey> comparer, IEqualityComparer<TKey> equalityComparer)
		//{
		//	forwardSortedDictionary = new SortedDictionary<TKey, TValue>(comparer);
		//	reverseDictionary = new Dictionary<TValue, TKey>(equalityComparer);
		//}
		//public ObservableDictionary(int capacity)
		//{
		//	forwardSortedDictionary = new SortedDictionary<TKey, TValue>(capacity);
		//	reverseDictionary = new Dictionary<TKey, TValue>(capacity);
		////}
		//public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
		//{
		//	forwardSortedDictionary = new SortedDictionary<TKey, TValue>(dictionary, comparer);
		//}
		//public ObservableDictionary(int capacity, IComparer<TKey> comparer)
		//{
		//	forwardSortedDictionary = new SortedDictionary<TKey, TValue>(capacity, comparer);
		//}

		#endregion

		#region IDictionary<TKey, TValue> Members

		public void Add(TKey key, TValue value)
		{
			Insert(key, value, true);
		}

		public bool ContainsKey(TKey key)
		{
			return forwardSortedDictionary.ContainsKey(key);
		}

		public ICollection<TKey> Keys
		{
			get { return forwardSortedDictionary.Keys; }
		}

		public bool Remove(TKey key)
		{
			if (key == null) throw new ArgumentNullException("key");

			TValue value;
			forwardSortedDictionary.TryGetValue(key, out value);
			var isRemoved = forwardSortedDictionary.Remove(key);
			if (isRemoved)
			{
				reverseDictionary.Remove(value);
				OnCollectionChanged();
			}

			return isRemoved;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return forwardSortedDictionary.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values
		{
			get { return forwardSortedDictionary.Values; }
		}

		public TValue this[TKey key]
		{
			get { return forwardSortedDictionary[key]; }
			set { Insert(key, value, false); }
		}

		#endregion

		#region ICollection<KeyValuePair<TKey, TValue>> Members

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Insert(item.Key, item.Value, true);
		}

		public void Clear()
		{
			if (forwardSortedDictionary.Count != 0)
			{
				forwardSortedDictionary.Clear();
				reverseDictionary.Clear();
				OnCollectionChanged();
			}
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return forwardSortedDictionary[item.Key].Equals(item.Value);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			forwardSortedDictionary.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return forwardSortedDictionary.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
			//get { return forwardSortedDictionary.IsReadOnly; }
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return Remove(item.Key);
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey, TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return forwardSortedDictionary.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)forwardSortedDictionary).GetEnumerator();
		}

		#endregion

		#region INotifyCollectionChanged Members

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public void AddRange(IDictionary<TKey, TValue> items)
		{
			if (items == null) throw new ArgumentNullException("items");

			if (items.Count != 0)
			{
				if (forwardSortedDictionary.Count != 0)
				{
					if (items.Keys.Any(x => forwardSortedDictionary.ContainsKey(x)))
					{
						throw new ArgumentException("An item with the same key has already been added.");
					}

					foreach (var item in items)
					{
						forwardSortedDictionary.Add(item.Key, item.Value);
						reverseDictionary.Add(item.Value, item.Key);
					}
				}
				else
				{
					forwardSortedDictionary = new SortedDictionary<TKey, TValue>(items);
					reverseDictionary = new Dictionary<TValue, TKey>(items.ToDictionary(
						kvp => kvp.Value, kvp => kvp.Key));

				}

				OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
			}
		}

		private void Insert(TKey key, TValue value, bool add)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (forwardSortedDictionary.TryGetValue(key, out TValue item))
			{
				if (add) throw new ArgumentException("An item with the same key has already been added.");
				if (Equals(item, value)) return;
				forwardSortedDictionary[key] = value;
				reverseDictionary[value] = key;

				OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, item));

			}
			else
			{
				forwardSortedDictionary[key] = value;
				reverseDictionary[value] = key;

				OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
			}
		}

		private void OnPropertyChanged()
		{
			OnPropertyChanged(CountString);
			OnPropertyChanged(IndexerName);
			OnPropertyChanged(KeysName);
			OnPropertyChanged(ValuesName);
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void OnCollectionChanged()
		{
			OnPropertyChanged();
			if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
		{
			OnPropertyChanged();
			if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem));
		}

		private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
		{
			OnPropertyChanged();
			if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
		}

		private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
		{
			OnPropertyChanged();
			if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems));
		}
	}
}
