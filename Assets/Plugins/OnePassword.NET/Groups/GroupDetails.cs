using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OnePassword.Groups
{

	/// <summary>
	/// Represents a 1Password group with details.
	/// </summary>
	public sealed class GroupDetails : GroupBase
	{
		/// <summary>
		/// The group permissions.
		/// </summary>
		[JsonProperty("permissions")]
		public IReadOnlyList<GroupPermission> Permissions { get; } =
			new List<GroupPermission>();

		/// <summary>
		/// The group type.
		/// </summary>
		[JsonProperty("type")]
		public GroupType Type { get; } = GroupType.Unknown;

		/// <summary>
		/// The date and time when the group was last updated.
		/// </summary>
		[JsonProperty("updated_at")]
		public DateTimeOffset Updated { get; }
	}
}