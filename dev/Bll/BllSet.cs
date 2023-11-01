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
	public class BllSet : IDisposable
	{
		public List<Set> GetSets(IRestClient client)
		{
			return client.GetSets();
		}

		public Set GetSetByCode(IRestClient client, string setCode)
		{
			return client.GetSetByCode(setCode);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
