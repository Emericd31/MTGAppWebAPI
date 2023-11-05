/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models.Builders
{
	/// <summary>Class that uses builder pattern to build set object.</summary>
	public class SetBuilder
	{
		#region Private Properties

		/// <summary>Set object.</summary>
		private readonly Set _set = new Set();

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor that initializes necesary values.</summary>
		public SetBuilder()
		{
			_set.Name = string.Empty;
			_set.Code = string.Empty;
			_set.Type = string.Empty;
			_set.ImgURL = string.Empty;
			_set.ReleaseDate = string.Empty;
			_set.CardCount = 0;
			_set.ParentSetCode = string.Empty;
			_set.IsDigital = false;
		}

		#endregion Constructor

		#region Builders

		/// <summary>Adds name to the set.</summary>
		/// <param name="name">Name.</param>
		/// <returns>The <see cref="SetBuilder"/> object.</returns>
		public SetBuilder AddName(string name)
		{
			_set.Name = name;
			return this;
		}

		/// <summary>Adds code to the set.</summary>
		/// <param name="code">Code.</param>
		/// <returns>The <see cref="SetBuilder"/> object.</returns>
		public SetBuilder AddCode(string code)
		{
			_set.Code = code;
			return this;
		}

		/// <summary>Adds type to the set.</summary>
		/// <param name="type">Type.</param>
		/// <returns>The <see cref="SetBuilder"/> object.</returns>
		public SetBuilder AddType(string type)
		{
			_set.Type = type;
			return this;
		}

		/// <summary>Adds image URL to the set.</summary>
		/// <param name="ImgUrl">Image URL.</param>
		/// <returns>The <see cref="SetBuilder"/> object.</returns>
		public SetBuilder AddImgUrl(string ImgUrl)
		{
			_set.ImgURL = ImgUrl;
			return this;
		}

		/// <summary>Adds release date to the set.</summary>
		/// <param name="releaseDate">Release date.</param>
		/// <returns>The <see cref="SetBuilder"/> object.</returns>
		public SetBuilder AddReleaseDate(string releaseDate)
		{
			_set.ReleaseDate = releaseDate;
			return this;
		}

		/// <summary>Adds card count to the set.</summary>
		/// <param name="cardCount">Card count.</param>
		/// <returns>The <see cref="SetBuilder"/> object.</returns>
		public SetBuilder AddCardCount(long cardCount)
		{
			_set.CardCount = cardCount;
			return this;
		}

		/// <summary>Adds parent set code to the set.</summary>
		/// <param name="parentSetCode">Parent set code.</param>
		/// <returns>The <see cref="SetBuilder"/> object.</returns>
		public SetBuilder AddParentSetCode(string parentSetCode)
		{
			_set.ParentSetCode = parentSetCode;
			return this;
		}

		/// <summary>Adds boolean indicating if the set is digital to the set.</summary>
		/// <param name="isDigital">Boolean indicating if the set is digital.</param>
		/// <returns>The <see cref="SetBuilder"/> object.</returns>
		public SetBuilder AddIsDigital(bool isDigital)
		{
			_set.IsDigital = isDigital;
			return this;
		}

		#endregion Builders

		#region Build Method

		/// <summary>Build method.</summary>
		/// <returns>The built <see cref="Set"/> object.</returns>
		public Set Build()
		{
			return _set;
		}

		#endregion Build Method
	}
}
