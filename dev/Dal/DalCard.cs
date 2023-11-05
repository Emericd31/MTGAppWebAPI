/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Context;
using MagicAppAPI.Models;

namespace MagicAppAPI.Dal
{
	public class DalCard
	{
		private MagicAppContext _appContext;

		public DalCard(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		public Card? GetByUID(string uid)
		{
			return _appContext.Cards.FirstOrDefault(c => c.UID == uid);
		}
	}
}
