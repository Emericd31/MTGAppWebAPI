/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.Text.Json;
using MagicAppAPI.Enums;
using MagicAppAPI.Models;
using MagicAppAPI.Tools;

namespace MagicAppAPI.ExternalAPIs.ScryFall
{
	public static class ScryFallDataConverters
	{
		#region Sets

		public static Set SetConverter(dynamic? set)
		{
			var setObject = new Set
			{
				Name = "",
				Code = "",
				Type = "",
				ImgURL = "",
				ReleaseDate = "",
				CardCount = 0,
				ParentSetCode = "",
				IsDigital = false
			};

			try
			{
				JsonElement currentElement = new JsonElement();

				if (set?.TryGetProperty("name", out currentElement))
					setObject.Name = JsonReader.TryGetStringFromJsonElement(currentElement);

				if (set?.TryGetProperty("code", out currentElement))
					setObject.Code = JsonReader.TryGetStringFromJsonElement(currentElement);

				if (set?.TryGetProperty("set_type", out currentElement))
					setObject.Type = JsonReader.TryGetStringFromJsonElement(currentElement);

				if (set?.TryGetProperty("icon_svg_uri", out currentElement))
					setObject.ImgURL = JsonReader.TryGetStringFromJsonElement(currentElement);

				if (set?.TryGetProperty("released_at", out currentElement))
					setObject.ReleaseDate = JsonReader.TryGetStringFromJsonElement(currentElement);

				if (set?.TryGetProperty("card_count", out currentElement))
					setObject.CardCount = JsonReader.TryGetLongFromJsonElement(currentElement);

				if (set?.TryGetProperty("parent_set_code", out currentElement))
					setObject.ParentSetCode = JsonReader.TryGetStringFromJsonElement(currentElement);

				if (set?.TryGetProperty("digital", out currentElement))
					setObject.IsDigital = JsonReader.TryGetBoolFromJsonElement(currentElement);

				return setObject;
			}
			catch
			{
				return setObject;
			}
		}

		#endregion Sets

		#region Cards

		/// <summary>Converts data into card object.</summary>
		/// <param name="card">Card data.</param>
		/// <returns>Card object.</returns>
		public static Card CardConverter(dynamic? card)
		{
			var cardObject = new Card
			{
				Name = "",
				UID = "",
				Text = "",
				Rarity = ECardRarity.UNKNWOWN,
				SetCode = "",
				Artist = "",
				ImgUrl = "",
				USD = "",
				USDFoil = "",
				EUR = "",
				EURFoil = "",
				Types = new List<Models.Type>(),
				Colors = new List<Color>(),
				Keywords = new List<Keyword>(),
			};

			try
			{
				JsonElement currentElement = new JsonElement();

				// Gets the card name
				if (card?.TryGetProperty("name", out currentElement))
					cardObject.Name = JsonReader.TryGetStringFromJsonElement(currentElement);

				// Gets the card unique identifier
				if (card?.TryGetProperty("id", out currentElement))
					cardObject.UID = JsonReader.TryGetStringFromJsonElement(currentElement);

				// Gets the card text
				if (card?.TryGetProperty("oracle_text", out currentElement))
					cardObject.Text = JsonReader.TryGetStringFromJsonElement(currentElement);

				// Gets the card rarity
				if (card?.TryGetProperty("rarity", out currentElement))
				{
					var jsonCardRarity = JsonReader.TryGetStringFromJsonElement(currentElement);
					if (Enum.TryParse(jsonCardRarity.ToUpper(), out ECardRarity currentCardRarity))
						cardObject.Rarity = currentCardRarity;
				}

				// Gets the card set
				if (card?.TryGetProperty("set", out currentElement))
					cardObject.SetCode = JsonReader.TryGetStringFromJsonElement(currentElement);

				// Gets the card artist
				if (card?.TryGetProperty("artist", out currentElement))
					cardObject.Artist = JsonReader.TryGetStringFromJsonElement(currentElement);

				// Gets the card image URL
				if (card?.TryGetProperty("image_uris", out currentElement))
				{
					if (currentElement.TryGetProperty("normal", out currentElement))
						cardObject.ImgUrl = JsonReader.TryGetStringFromJsonElement(currentElement);
				}

				// Gets the card prices
				if (card?.TryGetProperty("prices", out currentElement))
				{
					JsonElement price = new JsonElement();
					if (currentElement.TryGetProperty("usd", out price))
						cardObject.USD = JsonReader.TryGetStringFromJsonElement(price);

					if (currentElement.TryGetProperty("usd_foil", out price))
						cardObject.USDFoil = JsonReader.TryGetStringFromJsonElement(price);

					if (currentElement.TryGetProperty("eur", out price))
						cardObject.EUR = JsonReader.TryGetStringFromJsonElement(price);

					if (currentElement.TryGetProperty("eur_foil", out price))
						cardObject.EURFoil = JsonReader.TryGetStringFromJsonElement(price);
				}

				// Gets the card types
				if (card?.TryGetProperty("type_line", out currentElement))
				{
					var types = JsonReader.TryGetStringFromJsonElement(currentElement);
					if (string.IsNullOrEmpty(types))
						cardObject.Types.Add(new Models.Type { Value = "UNKNOWN" });
					else
					{
						if (types.ToLower().Contains("legendary"))
							cardObject.Types.Add(new Models.Type { Value = "LEGENDARY" });
						if (types.ToLower().Contains("planeswalker"))
							cardObject.Types.Add(new Models.Type { Value = "PLANESWALKER" });
						if (types.ToLower().Contains("siege"))
							cardObject.Types.Add(new Models.Type { Value = "SIEGE" });
						if (types.ToLower().Contains("creature"))
							cardObject.Types.Add(new Models.Type { Value = "CREATURE" });
						if (types.ToLower().Contains("instant"))
							cardObject.Types.Add(new Models.Type { Value = "INSTANT" });
						if (types.ToLower().Contains("sorcery"))
							cardObject.Types.Add(new Models.Type { Value = "SORCERY" });
						if (types.ToLower().Contains("enchantment"))
							cardObject.Types.Add(new Models.Type { Value = "ENCHANTMENT" });
						if (types.ToLower().Contains("land"))
							cardObject.Types.Add(new Models.Type { Value = "LAND" });
						if (types.ToLower().Contains("artifact"))
							cardObject.Types.Add(new Models.Type { Value = "ARTIFACT" });
						if (types.ToLower().Contains("token"))
							cardObject.Types.Add(new Models.Type { Value = "TOKEN" });
					}
				}

				// Gets the card colors
				if (card?.TryGetProperty("mana_cost", out currentElement))
				{
					var colors = JsonReader.TryGetStringFromJsonElement(currentElement);
					if (colors.Contains("G"))
						cardObject.Colors.Add(new Color { Value = "GREEN" });
					if (colors.Contains("U"))
						cardObject.Colors.Add(new Color { Value = "BLUE" });
					if (colors.Contains("R"))
						cardObject.Colors.Add(new Color { Value = "RED" });
					if (colors.Contains("W"))
						cardObject.Colors.Add(new Color { Value = "WHITE" });
					if (colors.Contains("B"))
						cardObject.Colors.Add(new Color { Value = "BLACK" });

					if (cardObject.Colors.Count == 0)
					{
						if (card?.TryGetProperty("color_identity", out currentElement))
						{
							foreach (var el in currentElement.EnumerateArray())
							{
								var color = JsonReader.TryGetStringFromJsonElement(el);
								if (color.Contains("G"))
									cardObject.Colors.Add(new Color { Value = "GREEN" });
								if (color.Contains("U"))
									cardObject.Colors.Add(new Color { Value = "BLUE" });
								if (color.Contains("R"))
									cardObject.Colors.Add(new Color { Value = "RED" });
								if (color.Contains("W"))
									cardObject.Colors.Add(new Color { Value = "WHITE" });
								if (color.Contains("B"))
									cardObject.Colors.Add(new Color { Value = "BLACK" });
							}
						}
					}
				}

				// Gets the card keywords
				if (card?.TryGetProperty("keywords", out currentElement))
				{
					foreach (var el in currentElement.EnumerateArray())
					{
						cardObject.Keywords.Add(new Keyword { Value = JsonReader.TryGetStringFromJsonElement(el) });
					}
				}
				return cardObject;
			}
			catch (Exception ex)
			{
				return cardObject;
			}
		}

		#endregion Cards
	}
}
