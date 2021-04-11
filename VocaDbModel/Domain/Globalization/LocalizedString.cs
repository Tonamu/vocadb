#nullable disable

using System;
using VocaDb.Model.DataContracts;
namespace VocaDb.Model.Domain.Globalization
{
	public class LocalizedString : ILocalizedString, IEquatable<LocalizedString>
	{
		private string _val;

		public LocalizedString()
		{
			Language = ContentLanguageSelection.Japanese;
			Value = string.Empty;
		}

		public LocalizedString(string val, ContentLanguageSelection language)
			: this()
		{
			Value = val;
			Language = language;
		}

		public virtual ContentLanguageSelection Language { get; set; }

		public virtual string Value
		{
			get => _val;
			set
			{
				ParamIs.NotNull(() => value);
				_val = value;
			}
		}

#nullable enable
		public virtual bool ContentEquals(ILocalizedString? another)
		{
			return (another != null && another.Language == Language && another.Value == Value);
		}

		public virtual bool ContentEquals(LocalizedString? another)
		{
			return (another != null && another.Language == Language && another.Value == Value);
		}

		public virtual bool ContentEquals(LocalizedStringContract? another)
		{
			return (another != null && another.Language == Language && another.Value == Value);
		}

		public virtual bool Equals(LocalizedString? another)
		{
			return ContentEquals(another);
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as LocalizedString);
		}
#nullable disable

		public override int GetHashCode()
		{
			return (Language + ":" + Value).GetHashCode();
		}

#nullable enable
		public override string ToString()
		{
			return Language + ": " + Value;
		}
#nullable disable
	}
}
