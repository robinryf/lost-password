using UnityEngine;
using UnityEngine.Networking;

namespace RobinBird.Utilities.Unity.Helper
{
	public class MailHelper
	{
		/// <summary>
		/// Opens the mail app with prefilled text
		/// </summary>
		public static void OpenMailDraft(string email,string subject,string body)
		{
			subject = MyEscapeURL(subject);
			body = MyEscapeURL(body);
			Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
		}

		private static string MyEscapeURL(string url)
		{
			return UnityWebRequest.EscapeURL(url).Replace("+","%20");
		}
	}
}