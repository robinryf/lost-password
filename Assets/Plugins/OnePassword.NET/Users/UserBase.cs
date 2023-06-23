using Newtonsoft.Json;
using OnePassword.Common;

namespace OnePassword.Users
{

	/// <summary>
	/// Common base class that represents a 1Password user.
	/// </summary>
	public abstract class UserBase : ResultBase<IUser>, IUser
	{
		/// <summary>
		/// The user email.
		/// </summary>
		[JsonProperty("email")]
		public string Email { get; set; } = "";

		/// <summary>
		/// The user type.
		/// </summary>
		[JsonProperty("type")]
		public UserType Type { get; } = UserType.Unknown;

		/// <summary>
		/// The state of the user.
		/// </summary>
		[JsonProperty("state")]
		public State State { get; } = State.Unknown;
	}
}