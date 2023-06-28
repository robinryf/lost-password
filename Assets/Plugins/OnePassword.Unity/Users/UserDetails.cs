using System;
using Newtonsoft.Json;

namespace OnePassword.Users
{

	/// <summary>
	/// Represents a 1Password user with details.
	/// </summary>
	public sealed class UserDetails : UserBase
	{
		private DateTimeOffset? _lastAuthentication;

		/// <summary>
		/// The date and time when the user was created.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTimeOffset Created { get; }

		/// <summary>
		/// The date and time when the user was last updated.
		/// </summary>
		[JsonProperty("updated_at")]
		public DateTimeOffset Updated { get; }

		/// <summary>
		/// The date and time when the user was last authenticated.
		/// </summary>
		[JsonProperty("last_auth_at")]
		public DateTimeOffset? LastAuthentication
		{
			get => _lastAuthentication;
			set => _lastAuthentication = value == DateTimeOffset.MinValue ? null : value;
		}
	}
}