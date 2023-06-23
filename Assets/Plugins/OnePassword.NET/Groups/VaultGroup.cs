using System.Collections.Generic;
using Newtonsoft.Json;
using OnePassword.Vaults;

namespace OnePassword.Groups
{

	/// <summary>
	/// Represents a 1Password group associated with a vault.
	/// </summary>
	public sealed class VaultGroup : GroupBase
	{
		/// <summary>
		/// The group's permissions for the vault.
		/// </summary>
		[JsonProperty("permissions")]
		public IReadOnlyList<VaultPermission> Permissions { get; } =
			new List<VaultPermission>();
	}
}