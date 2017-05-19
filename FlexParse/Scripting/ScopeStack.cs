using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FlexParse.Scripting
{
	public sealed class ScopeStack
	{
		private Stack<JObject> _frames = new Stack<JObject>();
		private Stack<JObject> _activeObjects = new Stack<JObject>();

		/// <summary>
		/// Allows access to the global variables, which are used if no local variable overrides them.
		/// </summary>
		public JObject GlobalFrame { get; }

		public JObject ActiveObject => _activeObjects.Peek();

		public ScopeStack()
		{
			GlobalFrame = new JObject();
			_frames.Push(GlobalFrame);
		}

		/// <summary>
		/// Gets or sets a local variable
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public JToken this[string propertyName]
		{
			get
			{
				foreach (JObject frame in _frames)
				{
					JToken value = frame[propertyName];
					if (value != null)
					{
						return value;
					}
				}
				return null;
			}
			set
			{
				_frames.Peek()[propertyName] = value;
			}
		}

		public IDisposable CreateActiveObjectScope(JObject frame)
		{
			if (_frames.Contains(frame))
			{
				throw new ArgumentException("Frame already pushed", nameof(frame));
			}
			_frames.Push(frame);
			_activeObjects.Push(frame);
			return new Scope(this, frame);
		}

		public IDisposable CreateAnonymousScope()
		{
			var frame = new JObject();
			_frames.Push(frame);
			return new Scope(this, frame);
		}

		private void Pop(JObject frame)
		{
			if (_frames.Peek() == frame)
			{
				_frames.Pop();
				if (_activeObjects.Peek() == frame)
				{
					_activeObjects.Pop();
				}
			}
			else
			{
				throw new InvalidOperationException("Disposing non-top frame, you probably forgot to dispose previous ones");
			}
		}

		private class Scope : IDisposable
		{
			private bool _disposed = false;
			public ScopeStack Stack { get; }
			public JObject Frame { get; }

			public Scope(ScopeStack stack, JObject activeFrame)
			{
				Stack = stack;
				Frame = activeFrame;
			}

			public void Dispose()
			{
				if (_disposed)
				{
					throw new InvalidOperationException("Already disposed the frame");
				}
				_disposed = true;
				Stack.Pop(Frame);
			}
		}
	}
}