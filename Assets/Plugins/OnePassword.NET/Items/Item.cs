using System;
using Newtonsoft.Json;
using OnePassword.Vaults;

namespace OnePassword.Items
{

	/// <summary>
	/// Represents a 1Password item.
	/// </summary>
	public sealed class Item : ItemBase, IItem
	{
		/// <inheritdoc />
		[JsonProperty("id")]
		public string Id { get; set; } = "";

		/// <summary>
		/// The item vault.
		/// </summary>
		[JsonProperty("vault")]
		public Vault? Vault { get; }

		/// <summary>
		/// The ID of the user that last edited the item.
		/// </summary>
		[JsonProperty("last_edited_by")]
		public string? LastEditedUserId { get; }

		/// <summary>
		/// The date and time when the item was created.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTimeOffset? Created { get; }

		/// <summary>
		/// The date and time when the item was last updated.
		/// </summary>
		[JsonProperty("updated_at")]
		public DateTimeOffset? Updated { get; }
	}
}