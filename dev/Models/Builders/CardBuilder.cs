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
	/// <summary>Class that uses builder pattern to build card object.</summary>
	public class CardBuilder
	{
		#region Private Properties

		/// <summary>Card object.</summary>
		private readonly Card _card = new Card();

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor that initializes necesary values.</summary>
		public CardBuilder()
		{
			_card.UID = string.Empty;
			_card.Name = string.Empty;
			_card.Text = string.Empty;
			_card.Rarity = ECardRarity.COMMON;
			_card.SetCode = string.Empty;
			_card.Artist = string.Empty;
			_card.ImgUrl = string.Empty;
			_card.USD = string.Empty;
			_card.USDFoil = string.Empty;
			_card.EUR = string.Empty;
			_card.EURFoil = string.Empty;
			_card.Types = new List<Type>();
			_card.Colors = new List<Color>();
			_card.Keywords = new List<Keyword>();
		}

		#endregion Constructor

		#region Builders

		/// <summary>Adds unique identifier to the card.</summary>
		/// <param name="uid">Unique identifier.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddUID(string uid)
		{
			_card.UID = uid;
			return this;
		}

		/// <summary>Adds name to the card.</summary>
		/// <param name="name">Name.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddName(string name)
		{
			_card.Name = name;
			return this;
		}

		/// <summary>Adds text to the card.</summary>
		/// <param name="text">Text.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddText(string text)
		{
			_card.Text = text;
			return this;
		}

		/// <summary>Adds rarity to the card.</summary>
		/// <param name="rarity">Rarity.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddRarity(ECardRarity rarity)
		{
			_card.Rarity = rarity;
			return this;
		}

		/// <summary>Adds set code to the card.</summary>
		/// <param name="setCode">Set code.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddSetCode(string setCode)
		{
			_card.SetCode = setCode;
			return this;
		}

		/// <summary>Adds artist to the card.</summary>
		/// <param name="artist">Artist.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddArtist(string artist)
		{
			_card.Artist = artist;
			return this;
		}

		/// <summary>Adds image URL to the card.</summary>
		/// <param name="imgUrl">Image URL.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddImgUrl(string imgUrl)
		{
			_card.ImgUrl = imgUrl;
			return this;
		}

		/// <summary>Adds USD price to the card.</summary>
		/// <param name="usd">USD price.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddUSD(string usd)
		{
			_card.USD = usd;
			return this;
		}

		/// <summary>Adds USD price for foil card to the card.</summary>
		/// <param name="usdFoil">USD price for foil card.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddUSDFoil(string usdFoil)
		{
			_card.USDFoil = usdFoil;
			return this;
		}

		/// <summary>Adds EUR price to the card.</summary>
		/// <param name="eur">EUR price.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddEUR(string eur)
		{
			_card.EUR = eur;
			return this;
		}

		/// <summary>Adds EUR price for foil card to the card.</summary>
		/// <param name="eurFoil">EUR price for foil card.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddEURFoil(string eurFoil)
		{
			_card.EURFoil = eurFoil;
			return this;
		}

		/// <summary>Adds types to the card.</summary>
		/// <param name="types">List of types.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddTypes(List<Type> types)
		{
			_card.Types = types;
			return this;
		}

		/// <summary>Adds colors to the card.</summary>
		/// <param name="colors">List of colors.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddColors(List<Color> colors)
		{
			_card.Colors = colors;
			return this;
		}

		/// <summary>Adds keywords to the card.</summary>
		/// <param name="keywords">List of keywords.</param>
		/// <returns>The <see cref="CardBuilder"/> object.</returns>
		public CardBuilder AddKeywords(List<Keyword> keywords)
		{
			_card.Keywords = keywords;
			return this;
		}

		#endregion Builders

		#region Build Method

		/// <summary>Build method.</summary>
		/// <returns>The built <see cref="Card"/> object.</returns>
		public Card Build()
		{
			return _card;
		}

		#endregion Build Method
	}
}
