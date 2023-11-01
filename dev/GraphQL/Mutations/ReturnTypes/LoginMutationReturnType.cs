﻿/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.Mutations.ReturnTypes
{
    public class LoginMutationReturnType : MutationReturnTypeWithId
    {
        public string Token { get; set; }
        public LoginMutationReturnType(int statusCode, string message, int id, string token) : base(statusCode, message, id)
        {
            Token = token;
        }
    }
}
