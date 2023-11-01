/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Enums;

namespace MagicAppAPI.Models.Builders
{
	public class CardBuilder
	{
		private readonly Card _card = new Card();

		public CardBuilder()
		{
			_card.UID = "UID";
			_card.Name = "Name";
			_card.Text = "Text";
			_card.Rarity = Enums.ECardRarity.COMMON;
			_card.SetCode = "SetCode";
			_card.Artist = "Artist";
			_card.ImgUrl = "ImgURL";
			_card.USD = "USD";
			_card.USDFoil = "USDFoil";
			_card.EUR = "EUR";
			_card.EURFoil = "EURFoil";
			_card.Types = new List<Type>();
			_card.Colors = new List<Color>();
			_card.Keywords = new List<Keyword>();
		}

		public CardBuilder AddUID(string uid)
		{
			_card.UID = uid;
			return this;
		}

		public CardBuilder AddName(string name)
		{
			_card.Name = name;
			return this;
		}

		public CardBuilder AddText(string text)
		{
			_card.Text = text;
			return this;
		}

		public CardBuilder AddRarity(ECardRarity rarity)
		{
			_card.Rarity = rarity;
			return this;
		}

		public CardBuilder AddSetCode(string setCode)
		{
			_card.SetCode = setCode;
			return this;
		}

		public CardBuilder AddArtist(string artist)
		{
			_card.Artist = artist;
			return this;
		}

		public CardBuilder AddImgUrl(string imgUrl)
		{
			_card.ImgUrl = imgUrl;
			return this;
		}

		public CardBuilder AddUSD(string usd)
		{
			_card.USD = usd;
			return this;
		}

		public CardBuilder AddUSDFoil(string usdFoil)
		{
			_card.USDFoil = usdFoil;
			return this;
		}

		public CardBuilder AddEUR(string eur)
		{
			_card.EUR = eur;
			return this;
		}

		public CardBuilder AddEURFoil(string eurFoil)
		{
			_card.EURFoil = eurFoil;
			return this;
		}

		public CardBuilder AddTypes(List<Type> types)
		{
			_card.Types = types;
			return this;
		}

		public CardBuilder AddColors(List<Color> colors)
		{
			_card.Colors = colors;
			return this;
		}

		public CardBuilder AddKeywords(List<Keyword> keywords)
		{
			_card.Keywords = keywords;
			return this;
		}

		public Card Build()
		{
			return _card;
		}
	}
}
