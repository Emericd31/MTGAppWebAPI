﻿/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.ComponentModel.DataAnnotations;

namespace MagicAppAPI.Models
{
	/// <summary>Keyword object.</summary>
	public class Keyword
	{
		#region Public Properties

		[Key]
		/// <summary>Database's identifier.</summary>
		public int Id { get; set; }

		/// <summary>Keyword value.</summary>
		public string Value { get; set; } = string.Empty;

		/// <summary>Property used for the intermediate table.</summary>
		public IList<CardKeywords> CardKeywords { get; set; } = new List<CardKeywords>();

		#endregion Public Properties

		#region Constructor

		/// <summary>Default constructor.</summary>
		public Keyword() { }

		#endregion Constructor
	}
}
