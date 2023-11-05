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
	/// <summary>Card object.</summary>
	public class Card
	{
		#region Public Properties

		[Key]
		/// <summary>Database's identifier.</summary>
		public int Id { get; set; }

		/// <summary>Unique identifier.</summary>
		public string UID { get; set; } = string.Empty;

		/// <summary>Name.</summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>Text of the card.</summary>
		public string Text { get; set; } = string.Empty;

		/// <summary>Card rarity.</summary>
		public ECardRarity Rarity { get; set; }

		/// <summary>Set code.</summary>
		public string SetCode { get; set; } = string.Empty;

		/// <summary>Artist.</summary>
		public string Artist { get; set; } = string.Empty;

		/// <summary>URL of the image card.</summary>
		public string ImgUrl { get; set; } = string.Empty;

		/// <summary>US dollars card price (no foil).</summary>
		public string USD { get; set; } = string.Empty;

		/// <summary>US dollars card price (foil).</summary>
		public string USDFoil { get; set; } = string.Empty;

		/// <summary>Euros card price (no foil).</summary>
		public string EUR { get; set; } = string.Empty;

		/// <summary>Euros card price (foil).</summary>
		public string EURFoil { get; set; } = string.Empty;

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CardTypes> CardTypes { get; set; } = new List<CardTypes>();

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CardColors> CardColors { get; set; } = new List<CardColors>();

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CardKeywords> CardKeywords { get; set; } = new List<CardKeywords>();

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CollectionCards> CollectionCards { get; set; } = new List<CollectionCards>();

		/// <summary>List of card types.</summary>
		[NotMapped]
		public List<Type> Types { get; set; } = new List<Type>();

		/// <summary>List of card colors.</summary>
		[NotMapped]
		public List<Color> Colors { get; set; } = new List<Color>();

		/// <summary>List of card keywords.</summary>
		[NotMapped]
		public List<Keyword> Keywords { get; set; } = new List<Keyword>();

		#endregion Public Properties
	}
}
