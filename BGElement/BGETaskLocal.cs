using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace VSLee.Utils
{ // Copyright 2017 Victor Lee
	public class RoundRobinDirectory<TQueueKey>
	{
		private Dictionary<TQueueKey, int> directory;
		private int lastIndexAssigned = -1; // unmodded (%)
		private int threads;

		public RoundRobinDirectory(int numUnique, int threads)
		{
			this.directory = new Dictionary<TQueueKey, int>(numUnique);
			this.threads = threads;
		}

		public int GetAssignedNumber(TQueueKey key)
		{
			if (!directory.ContainsKey(key))
			{
				directory.Add(key, nextIndex());
			}
			return directory[key];
		}

		public void AddKeys(IEnumerable<TQueueKey> keys)
		{
			foreach (var key in keys)
			{
				GetAssignedNumber(key);
			}
		}

		private int nextIndex()
		{
			lastIndexAssigned++;
			return lastIndexAssigned % threads;
		}
	}

	/// <summary>
	/// Background Processor Element which uses TLocalState per task but common BC Queue
	/// by Victor Lee
	/// </summary>
	/// <typeparam name="TQueuedData">Type of data to be Queued</typeparam>
	/// <typeparam name="TCustom">Type of parameter in custom event</typeparam>
	/// <typeparam name="TLocalState">Type of TLocal per task</typeparam>
	public class BGETaskStateLocal<TQueuedData, TMessage, TCustom, TLocalState> :
		BGETaskFullLocal<TQueuedData, TMessage, TCustom, TLocalState, TQueuedData>, IDisposable
	{
		public BGETaskStateLocal(TLocalState[] tLocals, 
			string elementName, int numThreads, Predicate<CycleData> workCycle) :
			base(workCycle, elementName, numThreads, 1, tLocals, i => i)
		{ // will create threads.Count BC's but that's ok
		}

		public override void Add(TQueuedData data)
		{
			_taskQs[0].Add(new WorkItem(
				new TaskCompletionSource<TQueuedData>(), cTokenSource.Token, data));
			// do not call: base.Add(data);
		}

		protected override void Consume(object taskNumber)
		{
			int tNumber = (int)taskNumber;
			consumeCycle(queue: _taskQs[0], tLocal: _tLocals[tNumber], tNumber: tNumber);
			// do not call: base.Consume(taskNumber);
		}
	}

	/// <summary>
	/// Background Processor Element which uses local BC and TLocalState per task
	/// by Victor Lee
	/// </summary>
	/// <typeparam name="TQueuedData">Type of data to be Queued</typeparam>
	/// <typeparam name="TCustom">Type of parameter in custom event</typeparam>
	/// <typeparam name="TLocalState">Type of TLocal per task</typeparam>
	public class BGETaskFullLocal<TQueuedData, TMessage, TCustom, TLocalState, TQueueKey> : IDisposable
	{
		//const int bfCacheSize = 20;
		public int numProcessed { get; protected set; }
		public int numSkipped { get; protected set; }
		int numTotal = 0; // not sure why it was set to -1 before
		/// <summary>
		/// 
		/// </summary>
		/// <param name="numProcessed"></param>
		/// <param name="numSkipped"></param>
		/// <param name="modifiedTotal">numTotal - numSkipped</param>
		public delegate void ProgressD(int numProcessed, int numSkipped, int modifiedTotal);
		public event ProgressD ProgressMade;
		public event ProgressD BGECompleted;
		public event Action<TCustom> CustomEvent;
		protected Predicate<CycleData> workCycle;
		protected CancellationTokenSource cTokenSource = new CancellationTokenSource();
		private SynchronizationContext syncContext = SynchronizationContext.Current;
		private string elementName;
		Task[] consumers;
		private Func<TQueuedData, TQueueKey> dataToKeyFunc;

		protected BlockingCollection<WorkItem>[] _taskQs;
		protected TLocalState[] _tLocals;
		private int numThreads;
		public RoundRobinDirectory<TQueueKey> roundRobinDirectory;

		protected struct WorkItem
		{
			public readonly TaskCompletionSource<TQueuedData> TaskSource;
			public readonly CancellationToken? CancelToken;
			public readonly TQueuedData Data;
			//public TLocalState TLocal { get; set; }
			public WorkItem(TaskCompletionSource<TQueuedData> taskSource,
				CancellationToken? cancelToken,
				   TQueuedData Data) : this()
			{
				TaskSource = taskSource;
				//Action = action;
				CancelToken = cancelToken;
				//TLocal = default(TLocalState);
				this.Data = Data;
			}
		}

		protected enum EventType : byte
		{
			ProgressMade, Skipped, Message, CustomEvent
		}

		protected struct MainThreadParams
		{ // params used for executing on main thread
			public EventType EventType;
			public TMessage Message;
			public TCustom CustomParam;
		}

		public struct CycleData
		{
			/// <summary>
			/// Queued Data
			/// </summary>
			public TQueuedData Data { get; set; }
			public TLocalState TLocal { get; set; }
			public int TNumber { get; set; }
		}

		public event Action<TMessage> Message;

		public BGETaskFullLocal(Predicate<CycleData> workCycle, string elementName, int numThreads, int numUnique,
			TLocalState[] tLocals, Func<TQueuedData, TQueueKey> dataToKeyFunc) 
		{
			this.numThreads = numThreads;
			this.roundRobinDirectory = new RoundRobinDirectory<TQueueKey>(numUnique, numThreads);
			this.dataToKeyFunc = dataToKeyFunc;
			_taskQs = new BlockingCollection<WorkItem>[numThreads];
			_tLocals = tLocals;
			for (int i = 0; i < numThreads; i++)
			{
				_taskQs[i] = new BlockingCollection<WorkItem>();
			}

			this.workCycle = workCycle;
			this.elementName = elementName + ".";
			// Create and start a separate Task for each consumer:
			consumers = new Task[numThreads];
			for (int i = 0; i < numThreads; i++)
			{
				consumers[i] = Task.Factory.StartNew(Consume, (object)i, TaskCreationOptions.LongRunning);
			}
		}

		private void ExecuteOnMainThread(object p)
		{
			var mainThreadParams = (MainThreadParams)p;
			switch (mainThreadParams.EventType)
			{
				case EventType.ProgressMade:
					// normal report progress and increment
					numProcessed++;
					if (ProgressMade != null)
					{
						ProgressMade(numProcessed, numSkipped, numTotal - numSkipped);
					}
					CheckCompletion();
					break;
				case EventType.Skipped:
					// normal report progress and skip
					numSkipped++;
					if (ProgressMade != null)
					{
						ProgressMade(numProcessed, numSkipped, numTotal - numSkipped);
					}
					CheckCompletion();
					break;
				case EventType.Message:
					// raise message event
					if (Message != null)
					{
						Message(mainThreadParams.Message);
					}
					break;
				case EventType.CustomEvent:
					CustomEvent(mainThreadParams.CustomParam);
					break;
				default:
					break;
			}
		}

		public void Start(int numTotal)
		{
			numProcessed = 0;
			numSkipped = 0;
			this.numTotal = numTotal;
			CheckCompletion();
		}

		/// <summary>
		/// Fire BGECompleted Event if BGE is completed
		/// </summary>
		public void CheckCompletion()
		{
			if (IsCompleted() && BGECompleted != null) 
				BGECompleted(numProcessed, numSkipped, numTotal - numSkipped);
		}

		public bool IsCompleted()
		{
			return numProcessed + numSkipped == numTotal;
		}

		public void SkipOne()
		{
			Skip(1);
		}

		public void Skip(int NumToSkip)
		{
			for (int i = 0; i < NumToSkip; i++)
			{
				syncContext.Send(ExecuteOnMainThread, new MainThreadParams() 
									{ EventType = EventType.Skipped });
			}
		}

		public void RaiseMessageEvent(TMessage message)
		{
			syncContext.Send(ExecuteOnMainThread, new MainThreadParams() { EventType = EventType.Message, Message = message });
		}

		public void RaiseCustomEvent(TCustom customParam)
		{
			syncContext.Send(ExecuteOnMainThread, new MainThreadParams() 
				{ EventType = EventType.CustomEvent, CustomParam = customParam });
		}

		public virtual void Add(TQueuedData data)
		{
			int assignedNumber = roundRobinDirectory.GetAssignedNumber(dataToKeyFunc(data));
			WorkItem wi = new WorkItem(
				new TaskCompletionSource<TQueuedData>(), cTokenSource.Token, data);
			_taskQs[assignedNumber].Add(wi);
		}

		public void AddRange(IEnumerable<TQueuedData> data)
		{
			foreach (var dataItem in data)
			{
				Add(dataItem);
			}
		}

		public void AddAndIncrementRange(IEnumerable<TQueuedData> data)
		{
			foreach (var dataItem in data)
			{
				AddAndIncrement(dataItem);
			}
		}

		/// <summary>
		/// If bge completed, then reset to 0 first, then Add/Increment
		/// </summary>
		/// <param name="data"></param>
		public void AddAndIncrementOrResetRange(IEnumerable<TQueuedData> data)
		{
			if (IsCompleted())
			{
				Start(numTotal: data.Count());
			}
			foreach (var dataItem in data)
			{
				Add(dataItem);
			}
		}

		public void AddAndIncrement(TQueuedData data)
		{
			numTotal++;
			Add(data);
		}

		public void AddAndIncrementOrReset(TQueuedData data)
		{
			if (IsCompleted())
			{
				Start(numTotal: 0);
			}
			AddAndIncrement(data);
		}

		protected virtual void Consume(object taskNumber)
		{
			int tNumber = (int)taskNumber;
			consumeCycle(queue: _taskQs[tNumber], tLocal: _tLocals[tNumber], tNumber: tNumber);
		}

		protected void consumeCycle(BlockingCollection<WorkItem> queue, TLocalState tLocal, int tNumber)
		{
			SynchronizationContext.SetSynchronizationContext(syncContext);
			foreach (var workItem in queue.GetConsumingEnumerable())
				if (workItem.CancelToken.HasValue &&
					workItem.CancelToken.Value.IsCancellationRequested)
				{
					workItem.TaskSource.SetCanceled();
					syncContext.Send(ExecuteOnMainThread, new MainThreadParams() { EventType = EventType.Skipped });
				}
				else
					//try
					{
						if (workCycle(new CycleData() { Data = workItem.Data, TLocal = tLocal, TNumber = tNumber }))
						{
							syncContext.Send(ExecuteOnMainThread, new MainThreadParams() { EventType = EventType.ProgressMade });
						}
						else
						{
							syncContext.Send(ExecuteOnMainThread, new MainThreadParams() { EventType = EventType.Skipped });
						}

						workItem.TaskSource.SetResult(workItem.Data);   // Indicate completion
					}
					//catch (Exception ex)
					//{
					//    //RaiseMessageEvent(new MessageData(MessageType.Runtime_Error, elementName + "bgWorker in BGElement threw an " + ex.GetType().ToString() + ": " + ex.Message);
					//    syncContext.Send(ExecuteOnMainThread, new MainThreadParams() { EventType = EventType.Skipped });
					//    workItem.TaskSource.SetException(ex);// + do something like logging here...
					//}
		}

		public void WaitForCompletion()
		{ // + add a WhenComplete() http://stackoverflow.com/questions/6123406/waitall-vs-whenall
			foreach (var taskQ in _taskQs)
			{
				taskQ.CompleteAdding();
			}
			// + should we also CheckCompletion() here?
			Task.WaitAll(consumers);
		}

		public async Task WaitForCompletionAsync()
		{ // + add a WhenComplete() http://stackoverflow.com/questions/6123406/waitall-vs-whenall
			foreach (var taskQ in _taskQs)
			{
				taskQ.CompleteAdding();
			}
			// + should we also CheckCompletion() here?
			await Task.WhenAll(consumers);
		}

		public virtual void Dispose()
		{
			// TODO: fire progress event if no data was added for processing
			cTokenSource.Cancel();
			WaitForCompletion();
			foreach (var taskQ in _taskQs)
			{
				taskQ.Dispose();
			}
		}
	}
}