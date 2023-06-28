using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Application = UnityEngine.Application;

namespace OnePassword
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

		private readonly Queue<(Action<OnePasswordManager> commandAction, Action onFinished)> executionQueue =
			new Queue<(Action<OnePasswordManager> commandAction, Action onFinished)>();

		private bool isRunningCommand;

		[SerializeField]
		private string[] windowsPaths;

		[SerializeField]
		private string[] macOsPaths;
		
		private void Awake()
		{
			string[] paths;
			string executable;
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				paths = macOsPaths;
				executable = "op";
			}
			else if (Application.platform == RuntimePlatform.WindowsEditor ||
			         Application.platform == RuntimePlatform.WindowsPlayer)
			{
				paths = windowsPaths;
				executable = "op.exe";
			}
			else
			{
				Debug.LogError($"Current platform {Application.platform} is not supported.");
				return;
			}

			string path = null;
			foreach (string checkPath in paths)
			{
				var combinedPath = Path.Combine(checkPath, executable);
				bool doesExist = File.Exists(combinedPath);

				if (doesExist)
				{
					path = checkPath;
					Debug.Log($"Found 1Password Executable at: {checkPath}");
					break;
				}
			}

			if (path == null)
			{
				Debug.LogError($"Could not find 1Password Executable. Searched in paths: {string.Join(',', paths)}");
				enabled = false;
				return;
			}
			
			MainThreadHelper.Init();
			manager = new OnePasswordManager(path, executable, true, true);
		}

		private void Update()
		{
			MainThreadHelper.Instance.Update();
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