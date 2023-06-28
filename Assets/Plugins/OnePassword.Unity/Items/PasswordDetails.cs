using Newtonsoft.Json;

namespace OnePassword.Items
{

	/// <summary>
	/// Represents details of a password.
	/// </summary>
	public sealed class PasswordDetails
	{
		/// <summary>
		/// The password strength.
		/// </summary>
		[JsonProperty("strength")]
		public string Strength { get; set; } = "";
	}
}