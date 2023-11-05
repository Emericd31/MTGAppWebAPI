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
	/// <summary>Collection object.</summary>
	public class Collection
	{
		#region Public Properties

		[Key]
		/// <summary>Database's identifier.</summary>
		public int Id { get; set; }

		/// <summary>Name.</summary>
		public string Name { get; set; } = string.Empty;

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

		/// <summary>Identifier of the user owner of the collection.</summary>
		public int UserId { get; set; }

		/// <summary>User object owner of the collection.</summary>
		public User? User { get; set; }

		/// <summary>List of cards present in the collection.</summary>
		public IList<CollectionCards> CollectionCards { get; set; } = new List<CollectionCards>();

		#endregion Public Properties
	}
}
