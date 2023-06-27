using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnePassword;
using OnePassword.Vaults;
using RobinBird.Utilities.Unity.Helper;
using UnityEngine;

namespace LostPassword
{
	/// <summary>
	/// Unity Native Wrapper for the 1Password CommandLine calls.
	/// - Manages multiple requests in a Queue so we don't run CMD runs in parallel
	/// - Makes sure that callbacks are forwarded back onto the Unity/Main Thread
	/// - 1Password calls run in separate threads
	/// </summary>
	public class OnePasswordUnityManager : MonoBehaviour
	{
		private OnePasswordManager manager;

		private Queue<(Action<OnePasswordManager> commandAction, Action onFinished)> executionQueue =
			new Queue<(Action<OnePasswordManager> commandAction, Action onFinished)>();

		private bool isRunningCommand;
		
		private void Awake()
		{
			manager = new OnePasswordManager("/usr/local/bin", "op", true, true);
		}

		private void Update()
		{
			if (isRunningCommand == false && executionQueue.Count > 0)
			{
				isRunningCommand = true;
				var command = executionQueue.Dequeue();
				// Only run one command at a time
				RunCommand(command.commandAction, () =>
				{
					isRunningCommand = false;
					command.onFinished?.Invoke();
				});
			}
		}

		/// <summary>
		/// Call to execute a command against the 1Password CLI interface
		/// </summary>
		/// <param name="commandAction">Perform the action with 1Password. Runs on a own thread!</param>
		/// <param name="onFinished">Called on Unity/Main Thread to handle finished 1Password CLI call.</param>
		public void EnqueueCommand(Action<OnePasswordManager> commandAction, Action onFinished)
		{
			executionQueue.Enqueue((commandAction, onFinished));
		}

		private void RunCommand(Action<OnePasswordManager> commandAction, Action onFinished)
		{
			Task.Run(() =>
			{
				try
				{
					commandAction?.Invoke(manager);
				}
				catch (Exception ex)
				{
					Debug.LogError($"Got error when executing 1Password Command:\n{ex}");
				}
				MainThreadHelper.QueueOnMainThread(onFinished);
			});
		}
	}
}