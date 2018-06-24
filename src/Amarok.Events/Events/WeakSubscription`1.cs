﻿/* Copyright(c) 2018, Olaf Kober
 * Licensed under GNU Lesser General Public License v3.0 (LPGL-3.0).
 * https://github.com/Amarok79/Events
 */

using System;
using System.Threading.Tasks;


namespace Amarok.Events
{
	/// <summary>
	/// Implementation class that represents a weak subscription. This weak subscription usually refers 
	/// via weak reference to another subscription, which again refers back to this subscription again 
	/// via weak reference.
	/// </summary>
	internal sealed class WeakSubscription<T> : Subscription<T>
	{
		/// <summary>
		/// a reference to the event source; necessary for disposal
		/// </summary>
		private readonly EventSource<T> mSource;

		/// <summary>
		/// a weak reference to another subscription referring to the handler
		/// </summary>
		private readonly WeakReference<Subscription<T>> mNextSubscription;


		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public WeakSubscription(EventSource<T> source, Subscription<T> subscription)
		{
			mSource = source;
			mNextSubscription = new WeakReference<Subscription<T>>(subscription);
		}


		/// <summary>
		/// Invokes the subscription's handler in a synchronous way.
		/// </summary>
		public override void Invoke(T value)
		{
			if (mNextSubscription.TryGetTarget(out var subscription))
			{
				// forward invocation to next subscription
				subscription.Invoke(value);
			}
			else
			{
				// otherwise, remove ourself from event source
				mSource.Remove(this);
			}
		}

		/// <summary>
		/// Invokes the subscription's handler in an asynchronous way.
		/// </summary>
		public override ValueTask InvokeAsync(T value)
		{
			if (mNextSubscription.TryGetTarget(out var subscription))
			{
				// forward invocation to next subscription
				return subscription.InvokeAsync(value);
			}
			else
			{
				// otherwise, remove ourself from event source
				mSource.Remove(this);
				return new ValueTask(Task.CompletedTask);
			}
		}

		/// <summary>
		/// Disposes the subscription; removes it from the event source.
		/// </summary>
		public override void Dispose()
		{
			// simply, remove ourself from event source
			mSource.Remove(this);
		}


		internal void TestingClearNextSubscription()
		{
			mNextSubscription.SetTarget(null);
		}
	}
}