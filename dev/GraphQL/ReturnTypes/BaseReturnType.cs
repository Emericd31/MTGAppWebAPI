/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.ReturnTypes
{
    /// <summary>Base result for query.</summary>
    public class BaseReturnType
    {
        #region Public Properties

        /// <summary>Status code.</summary>
        public int StatusCode { get; set; }

        /// <summary>Message.</summary>
        public string Message { get; set; }

        #endregion Public Properties

        #region Constructor

        /// <summary>Default constructor.</summary>
        public BaseReturnType()
        {
            StatusCode = 500;
            Message = "Internal Server Error";
        }

        /// <summary>Constructor.</summary>
        /// <param name="statusCode">Status code.</param>
        /// <param name="message">Meessage.</param>
        public BaseReturnType(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        #endregion Constructor
    }
}
