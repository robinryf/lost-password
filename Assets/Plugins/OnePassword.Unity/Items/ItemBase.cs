using Newtonsoft.Json;
using OnePassword.Common;

namespace OnePassword.Items
{

	/// <summary>
	/// Common base class that represents a 1Password item.
	/// </summary>
	public abstract class ItemBase : ITracked
	{
		/// <summary>
		/// The item title.
		/// </summary>
		[JsonProperty("title")]
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				TitleChanged = true;
			}
		}

		/// <summary>
		/// The item category.
		/// </summary>
		[JsonProperty("category")]
		public Category Category { get; private set; } = Category.Unknown;

		/// <summary>
		/// The item sections.
		/// </summary>
		[JsonProperty("sections")]
		public TrackedList<Section> Sections { get; private set; } = new();

		/// <summary>
		/// The item fields.
		/// </summary>
		[JsonProperty("fields")]
		public TrackedList<Field> Fields { get; private set; } = new();

		/// <summary>
		/// The item URLs.
		/// </summary>
		[JsonProperty("urls")]
		public TrackedList<Url> Urls { get; private set; } = new();

		/// <summary>
		/// The tags associated with the item.
		/// </summary>
		[JsonProperty("tags")]
		public TrackedList<string> Tags { get; private set; } = new();

		/// <summary>
		/// Returns <see langword="true"/> when the title has changed, <see langword="false"/> otherwise.
		/// </summary>
		internal bool TitleChanged { get; private set; }

		/// <inheritdoc />
		bool ITracked.Changed => TitleChanged
		                         | ((ITracked)Sections).Changed
		                         | ((ITracked)Fields).Changed
		                         | ((ITracked)Urls).Changed
		                         | ((ITracked)Tags).Changed;

		private string _title = "";

		/// <inheritdoc />
		void ITracked.AcceptChanges()
		{
			TitleChanged = false;
			((ITracked)Sections).AcceptChanges();
			((ITracked)Fields).AcceptChanges();
			((ITracked)Urls).AcceptChanges();
			((ITracked)Tags).AcceptChanges();
		}
	}
}