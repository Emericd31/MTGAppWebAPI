/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using HotChocolate.Authorization;
using MagicAppAPI.Bll;
using MagicAppAPI.Context;
using MagicAppAPI.ExternalAPIs.ScryFall;
using MagicAppAPI.Models;

namespace MagicAppAPI.GraphQL.Queries
{
	/// <summary>Class that manages the retrieval of set data.</summary>
	[ExtendObjectType("Query")]
	public class SetQuery
	{
		#region Public Methods

		[Authorize]
		/// <summary>Gets all available sets.</summary>
		/// <param name="_">Parameter not used.</param>
		/// <returns>List of available sets.</returns>
		public List<Set> GetSets([Service] MagicAppContext _)
		{
			var setList = new List<Set>();

			using (BllSet bll = new BllSet())
			{
				setList = bll.GetSets(ScryFallRestClient.GetInstance());
			}

			return setList;
		}

		[Authorize]
		/// <summary>Gets a set given a code.</summary>
		/// <param name="setCode">Set code.</param>
		/// <param name="_">Parameter not used.</param>
		/// <returns>Set object.</returns>
		/// <exception cref="Exception">Not found exception.</exception>
		public Set GetSetByCode(string setCode, [Service] MagicAppContext _)
		{
			Set? set = null;

			using (BllSet bll = new BllSet())
			{
				set = bll.GetSetByCode(ScryFallRestClient.GetInstance(), setCode);
			}

			if (set != null)
				return set;
			else
				throw new Exception($"Cannot find set with code {setCode}");
		}

		#endregion Public Methods
	}
}
