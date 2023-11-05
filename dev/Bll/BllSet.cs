/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.ExternalAPIs;
using MagicAppAPI.Models;

namespace MagicAppAPI.Bll
{
	/// <summary>Business logic layer for sets objects.</summary>
	public class BllSet : IDisposable
	{
		#region Public Methods

		#region Getters

		/// <summary>Gets all sets.</summary>
		/// <param name="client">Client providing API to get sets.</param>
		/// <returns>List of <see cref="Set"/> objects.</returns>
		public List<Set> GetSets(IRestClient client)
		{
			return client.GetSets();
		}

		/// <summary>Gets set knowing its code.</summary>
		/// <param name="client">Client providing API to get sets.</param>
		/// <returns>A <see cref="Set"/> object (null if not found).</returns>
		public Set? GetSetByCode(IRestClient client, string setCode)
		{
			return client.GetSetByCode(setCode);
		}

		#endregion Getters

		/// <summary>Disposes resources.</summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		#endregion Public Methods
	}
}
