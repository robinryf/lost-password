using System;
using System.Globalization;
using Newtonsoft.Json;

namespace OnePassword.Items
{

	/// <summary>
	/// Represents a 1Password item section.
	/// </summary>
	public sealed class Section
	{
		/// <summary>
		/// The section ID.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; } = "";

		/// <summary>
		/// The section label.
		/// </summary>
		[JsonProperty("label")]
		public string Label { get; set; } = "";

		/// <summary>
		/// Initializes a new instance of <see cref="Section"/>.
		/// </summary>
		public Section()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Section"/> with the specified label.
		/// </summary>
		/// <param name="label">The section label.</param>
		public Section(string label)
		{
			Id = label.ToLower(CultureInfo.InvariantCulture).Replace(" ", "_");
			Label = label;
		}
	}
}