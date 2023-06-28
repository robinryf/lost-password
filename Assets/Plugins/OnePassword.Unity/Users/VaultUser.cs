using System.Collections.Generic;
using Newtonsoft.Json;
using OnePassword.Vaults;

namespace OnePassword.Users
{

	/// <summary>
	/// Represents a 1Password user associated with a vault.
	/// </summary>
	public sealed class VaultUser : UserBase
	{
		/// <summary>
		/// The user's permissions for the vault.
		/// </summary>
		[JsonProperty("permissions")]
		public IReadOnlyList<VaultPermission> Permissions { get; } =
			new List<VaultPermission>();
	}
}