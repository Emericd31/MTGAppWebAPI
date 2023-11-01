/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models.Builders
{
	public class SetBuilder
	{
		private readonly Set _set = new Set();

		public SetBuilder()
		{
			_set.Name = "Name";
			_set.Code = "Code";
			_set.Type = "Type";
			_set.ImgURL = "ImgUrl";
			_set.ReleaseDate = "ReleaseDate";
			_set.CardCount = 0;
			_set.ParentSetCode = "ParentSetCode";
			_set.IsDigital = false;
		}

		public SetBuilder AddName(string name)
		{
			_set.Name = name;
			return this;
		}

		public SetBuilder AddCode(string code)
		{
			_set.Code = code;
			return this;
		}

		public SetBuilder AddType(string type)
		{
			_set.Type = type;
			return this;
		}

		public SetBuilder AddImgUrl(string ImgUrl)
		{
			_set.ImgURL = ImgUrl;
			return this;
		}

		public SetBuilder AddReleaseDate(string releaseDate)
		{
			_set.ReleaseDate = releaseDate;
			return this;
		}

		public SetBuilder AddCardCount(long cardCount)
		{
			_set.CardCount = cardCount;
			return this;
		}

		public SetBuilder AddParentSetCode(string parentSetCode)
		{
			_set.ParentSetCode = parentSetCode;
			return this;
		}

		public SetBuilder AddIsDigital(bool isDigital)
		{
			_set.IsDigital = isDigital;
			return this;
		}

		public Set Build()
		{
			return _set;
		}
	}
}
