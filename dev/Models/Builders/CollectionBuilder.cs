/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models.Builders
{
	/// <summary>Class that uses builder pattern to build collection object.</summary>
	public class CollectionBuilder
	{
		#region Private Properties

		/// <summary>Collection object.</summary>
		private readonly Collection _collection = new Collection();

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor that initializes necesary values.</summary>
		public CollectionBuilder(User user)
		{
			_collection.Name = string.Empty;
			_collection.Description = string.Empty;
			_collection.NbCards = 0;
			_collection.EURCardNotValued = 0;
			_collection.USDCardNotValued = 0;
			_collection.EURPrice = 0;
			_collection.USDPrice = 0;
			_collection.CollectionCards = new List<CollectionCards>();
			_collection.UserId = user.Id;
			_collection.User = user;
		}

		#endregion Constructor

		#region Builders

		/// <summary>Adds name to the collection.</summary>
		/// <param name="name">Name.</param>
		/// <returns>The <see cref="CollectionBuilder"/> object.</returns>
		public CollectionBuilder AddName(string name)
		{
			_collection.Name = name;
			return this;
		}

		/// <summary>Adds description to the collection.</summary>
		/// <param name="name">Description.</param>
		/// <returns>The <see cref="CollectionBuilder"/> object.</returns>
		public CollectionBuilder AddDescription(string description)
		{
			_collection.Description = description;
			return this;
		}

		/// <summary>Adds number of cards in the collection.</summary>
		/// <param name="nbCards">Description.</param>
		/// <returns>The <see cref="CollectionBuilder"/> object.</returns>
		public CollectionBuilder AddNbCards(int nbCards)
		{
			_collection.NbCards = nbCards;
			return this;
		}

		/// <summary>Adds EUR cards not value to the collection.</summary>
		/// <param name="eurCardNotValued">Number of EUR cards with no price value.</param>
		/// <returns>The <see cref="CollectionBuilder"/> object.</returns>
		public CollectionBuilder AddEURCardNotValued(int eurCardNotValued)
		{
			_collection.EURCardNotValued = eurCardNotValued;
			return this;
		}

		/// <summary>Adds USD cards not value to the collection.</summary>
		/// <param name="usdCardNotValued">Number of USD cards with no price value.</param>
		/// <returns>The <see cref="CollectionBuilder"/> object.</returns>
		public CollectionBuilder AddUSDCardNotValued(int usdCardNotValued)
		{
			_collection.USDCardNotValued = usdCardNotValued;
			return this;
		}

		/// <summary>Adds EUR price to the collection.</summary>
		/// <param name="eurPrice">EUR price.</param>
		/// <returns>The <see cref="CollectionBuilder"/> object.</returns>
		public CollectionBuilder AddEURPrice(float eurPrice)
		{
			_collection.EURPrice = eurPrice;
			return this;
		}

		/// <summary>Adds USD price to the collection.</summary>
		/// <param name="usdPrice">USD price.</param>
		/// <returns>The <see cref="CollectionBuilder"/> object.</returns>
		public CollectionBuilder AddUSDPrice(float usdPrice)
		{
			_collection.USDPrice = usdPrice;
			return this;
		}

		#endregion Builders

		#region Build Method

		/// <summary>Build method.</summary>
		/// <returns>The built <see cref="Collection"/> object.</returns>
		public Collection Build()
		{
			return _collection;
		}

		#endregion Build Method
	}
}
