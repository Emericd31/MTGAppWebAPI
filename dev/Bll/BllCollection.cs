/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Context;
using MagicAppAPI.Dal;
using MagicAppAPI.Models;
using MagicAppAPI.Models.Builders;

namespace MagicAppAPI.Bll
{
	/// <summary>Business logic layer for collection objects.</summary>
	public class BllCollection : IDisposable
	{
		#region Private Properties

		/// <summary>Data access layer for collection.</summary>
		private DalCollection _dalCollection;

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="appContext">Database context.</param>
		public BllCollection(MagicAppContext appContext)
		{
			_dalCollection = new DalCollection(appContext);
		}

		#endregion Constructor

		#region Public Methods

		#region Getters

		/// <summary>Gets the list of collections for the specific user.</summary>
		/// <param name="userId">User identifier.</param>
		/// <returns>The list of <see cref="Collection"/>.</returns>
		public IQueryable<Collection> GetUserCollections(int userId)
		{
			return _dalCollection.GetUserCollections(userId);
		}

		/// <summary>Gets the specific collection for user.</summary>
		/// <param name="userId">User identifier.</param>
		/// <returns>The <see cref="Collection"/> (null if  not found).</returns>
		public Collection? GetUserCollectionById(int userId, int collectionId)
		{
			return _dalCollection.GetUserCollectionById(userId, collectionId);
		}

		#endregion Getters

		#region Adding

		/// <summary>Adds a new collection in database and saves the context.</summary>
		/// <param name="user">User associated with the collection.</param>
		/// <param name="name">Collection name.</param>
		/// <param name="description">Collection description.</param>
		public void AddCollectionToUser(User user, string name, string description)
		{
			var newCollection = new CollectionBuilder(user).AddName(name).AddDescription(description).Build();
			_dalCollection.AddCollection(newCollection);
		}

		#endregion Adding

		#region Setters

		/// <summary>Modifies the collection information.</summary>
		/// <param name="collection">Collection.</param>
		/// <param name="name">Name.</param>
		/// <param name="description">Descritpion.</param>
		public void ModifyCollection(Collection collection, string name, string description)
		{
			_dalCollection.ModifyCollection(collection, name, description);
		}

		#endregion Setters

		#region Removing

		/// <summary>Deletes a specific collection.</summary>
		/// <param name="collection">Collection.</param>
		public void DeleteCollection(Collection collection)
		{
			_dalCollection.DeleteCollection(collection);
		}

		#endregion Removing

		/// <summary>Dispose methods.</summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		#endregion Public Methods
	}
}
