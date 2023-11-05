/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MagicAppAPI.Enums;

namespace MagicAppAPI.Models
{
	public class Card
	{
		[Key]
		/// <summary>Database's identifier.</summary>
		public int Id { get; set; }

		/// <summary>Unique identifier.</summary>
		public string UID { get; set; }

		/// <summary>Name.</summary>
		public string Name { get; set; }

		/// <summary>Text of the card.</summary>
		public string Text { get; set; }

		/// <summary>Card rarity.</summary>
		public ECardRarity Rarity { get; set; }

		/// <summary>Set code.</summary>
		public string SetCode { get; set; }

		/// <summary>Artist.</summary>
		public string Artist { get; set; }

		/// <summary>URL of the image card.</summary>
		public string ImgUrl { get; set; }

		/// <summary>US dollars card price (no foil).</summary>
		public string USD { get; set; }

		/// <summary>US dollars card price (foil).</summary>
		public string USDFoil { get; set; }

		/// <summary>Euros card price (no foil).</summary>
		public string EUR { get; set; }

		/// <summary>Euros card price (foil).</summary>
		public string EURFoil { get; set; }

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CardTypes> CardTypes { get; set; }

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CardColors> CardColors { get; set; }

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CardKeywords> CardKeywords { get; set; }

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CollectionCards> CollectionCards { get; set; }

		[NotMapped]
		public List<Type> Types { get; set; } = new List<Type>();

		[NotMapped]
		public List<Color> Colors { get; set; } = new List<Color>();

		[NotMapped]
		public List<Keyword> Keywords { get; set; } = new List<Keyword>();
	}
}
