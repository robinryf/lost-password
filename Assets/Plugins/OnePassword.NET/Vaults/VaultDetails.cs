using System;
using Newtonsoft.Json;

namespace OnePassword.Vaults
{

	/// <summary>
	/// Represents a 1Password vault with details.
	/// </summary>
	public sealed class VaultDetails : VaultBase
	{
		/// <summary>
		/// The vault type.
		/// </summary>
		[JsonProperty("type")]
		public VaultType Type { get; }

		/// <summary>
		/// The vault items.
		/// </summary>
		[JsonProperty("items")]
		public int Items { get; }

		/// <summary>
		/// The date and time when the vault was created.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTimeOffset Created { get; }

		/// <summary>
		/// The date and time when the vault was last updated.
		/// </summary>
		[JsonProperty("updated_at")]
		public DateTimeOffset Updated { get; }
	}
}