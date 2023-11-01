/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	public class Set
	{
		public string Name { get; set; }

		public string Code { get; set; }

		public string Type { get; set; }

		public string ImgURL { get; set; }

		public string ReleaseDate { get; set; }

		public long CardCount { get; set; }

		public string ParentSetCode { get; set; }

		public bool IsDigital { get; set; }
	}
}
