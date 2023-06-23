using System;
using Newtonsoft.Json;
using OnePassword.Common;

namespace OnePassword.Groups
{

	/// <summary>
	/// Common base class that represents a 1Password group.
	/// </summary>
	public abstract class GroupBase : ResultBase<IGroup>, IGroup
	{
		/// <summary>
		/// The group description.
		/// </summary>
		[JsonProperty("description")]
		public string Description { get; set; } = "";

		/// <summary>
		/// The state of the group.
		/// </summary>
		[JsonProperty("state")]
		public State State { get; } = State.Unknown;

		/// <summary>
		/// The date and time when the group was created.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTimeOffset Created { get; }
	}
}