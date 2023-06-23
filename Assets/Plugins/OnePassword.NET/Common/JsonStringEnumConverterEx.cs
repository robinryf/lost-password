using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace OnePassword.Common
{

	/// <summary>
	/// Converts between an enum and its string value.
	/// </summary>
	/// <typeparam name="TEnum">The enum type.</typeparam>
	/// <remarks>Originally authored by JasonBodley (https://github.com/JasonBodley) [https://github.com/dotnet/runtime/issues/31081#issuecomment-848697673]</remarks>
	internal sealed class JsonStringEnumConverterEx<TEnum> : JsonConverter<TEnum>
		where TEnum : struct, Enum
	{
		private readonly Dictionary<TEnum, string> _enumToString = new();
		private readonly Dictionary<string, TEnum> _stringToEnum = new();

		/// <summary>
		/// Initializes a new instance of <see cref="JsonStringEnumConverterEx{TEnum}"/>.
		/// </summary>
		public JsonStringEnumConverterEx()
		{
			foreach (var enumMemberValue in Enum.GetValues(typeof(TEnum)))
			{
				var enumMemberName = enumMemberValue.ToString();

				var enumMemberAttribute = typeof(TEnum).GetMember(enumMemberName).FirstOrDefault()
					?.GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>()
					.FirstOrDefault();
				if (enumMemberAttribute is { Value: not null })
				{
					var enumMemberString = enumMemberAttribute.Value.ToUpperInvariant()
						.Replace(" ", "_", StringComparison.InvariantCulture);
					_enumToString.Add((TEnum)enumMemberValue, enumMemberString);
					_stringToEnum.Add(enumMemberString, (TEnum)enumMemberValue);
				}
				else
				{
					var enumMemberString = enumMemberName.ToUpperInvariant();
					_enumToString.Add((TEnum)enumMemberValue, enumMemberString);
					_stringToEnum.Add(enumMemberString, (TEnum)enumMemberValue);
				}
			}
		}

		public override void WriteJson(JsonWriter writer, TEnum value, JsonSerializer serializer)
		{
			if (_enumToString.TryGetValue(value, out var enumValue))
				writer.WriteValue(enumValue);
			else
				throw new NotImplementedException("Enum does not have its string representation defined.");
		}

		public override TEnum ReadJson(JsonReader reader, Type objectType, TEnum existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			
			var stringValue = reader.Value as string ?? "Unknown";

			var enumMemberString = stringValue.ToUpperInvariant().Replace(" ", "_", StringComparison.InvariantCulture);
			if (_stringToEnum.TryGetValue(enumMemberString, out var enumValue))
				return enumValue;

			throw new NotImplementedException("Could not convert string value to its enum representation.");
		}
	}
}