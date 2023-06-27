using System;
using System.Threading.Tasks;
using RobinBird.Utilities.Runtime.Helper;

namespace RobinBird.Utilities.Unity.Extensions
{
	public static class TaskExtensions
	{
#if FIREBASE_PACKAGE
		/// <summary>
		/// Queue handler on main thread with error handling
		/// </summary>
		/// <param name="errorScope">UpperCaseCamelCase - Define a scope so that not all async errors are grouped in one exception on crashltyics</param>
		public static void ContinueWithErrorHandlingOnMainThread(this Task task, string errorScope, Action<Task> continuation, Action onError = null)
		{
			
			Firebase.Extensions.TaskExtension.ContinueWithOnMainThread(task, task1 =>
			{
				try
				{
					continuation(task1);
					if (task1.IsCanceled || task1.IsFaulted)
					{
						ErrorTracking.RecordException($"{errorScope}AsyncOperationFailed", exception: task1.Exception);
						onError?.Invoke();
					}
				}
				catch (Exception e)
				{
					ErrorTracking.RecordException($"{errorScope}AsyncOperationException", exception: e);
					onError?.Invoke();
				}
			});
		}
		
		public static void ContinueWithErrorHandlingOnMainThread<T>(this Task<T> task, string errorScope, Action<Task<T>> continuation, Action onError = null)
		{
			ContinueWithErrorHandlingOnMainThread((Task)task, errorScope, task1 =>
			{
				continuation((Task<T>)task1);
			}, onError);
		}
#endif
	}
}