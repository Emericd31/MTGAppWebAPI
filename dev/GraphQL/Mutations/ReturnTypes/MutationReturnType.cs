/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.Mutations.ReturnTypes
{
    public class MutationReturnType
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public MutationReturnType(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
