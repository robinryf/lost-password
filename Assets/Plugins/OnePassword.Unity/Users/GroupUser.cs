using Newtonsoft.Json;

namespace OnePassword.Users
{

	/// <summary>
	/// Represents a 1Password user associated with group.
	/// </summary>
	public sealed class GroupUser : UserBase
	{
		/// <summary>
		/// The user's role in the group.
		/// </summary>
		[JsonProperty("role")]
		public UserRole Role { get; } = UserRole.Unknown;
	}
}