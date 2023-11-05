/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.ComponentModel.DataAnnotations;

namespace MagicAppAPI.Models
{
	public class Collection
	{
		[Key]
		/// <summary>Database's identifier.</summary>
		public int Id { get; set; }

		/// <summary>Name.</summary>
		public string Name { get; set; }

		/// <summary>Description.</summary>
		public string Description { get; set; } = "";

		/// <summary>Total number of cards (by print).</summary>
		public int NbCards { get; set; }

		/// <summary>Price of the collection in euro.</summary>
		public float EURPrice { get; set; }

		/// <summary>Number of cards without their euros prices.</summary>
		public int EURCardNotValued { get; set; }

		/// <summary>Price of the collection in dollars US.</summary>
		public float USDPrice { get; set; }

		/// <summary>Number of cards without their dollars US prices.</summary>
		public int USDCardNotValued { get; set; }

		public int UserId { get; set; }

		public User User { get; set; }

		public IList<CollectionCards> CollectionCards { get; set; }
	}
}
