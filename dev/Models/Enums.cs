/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Enums
{
	/// <summary>Possible values of card rarity.</summary>
	public enum ECardRarity
	{
		UNKNWOWN,
		COMMON,
		UNCOMMON,
		RARE,
		SPECIAL,
		MYTHIC,
		BONUS
	}

	/// <summary>Possible values of request result.</summary>
	public enum ERequestResult
	{
		USER_NOT_FOUND,
		CARD_NOT_FOUND,
		CARD_NOT_FOUND_IN_COLLECTION,
		COLLECTION_NOT_FOUND,

		INVALID_PARAMETERS,

		CARD_ADDED,
		CARD_REMOVED,
		SUCCESS
	}
}
