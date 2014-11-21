﻿/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Specialized;
using System.Threading.Tasks;
using Thinktecture.IdentityServer.Core;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Validation;
using Thinktecture.IdentityServer.Tests.Connect.Setup;

namespace Thinktecture.IdentityServer.Tests.Connect.Validation.AuthorizeRequest
{
    
    public class Authorize_ClientValidation_Token
    {
        IdentityServerOptions _options = TestIdentityServerOptions.Create();

        [Xunit.Fact]
        [Xunit.Trait("Category", "AuthorizeRequest Client Validation - Token")]
        public async Task Mixed_Token_Request_Without_OpenId_Scope()
        {
            var parameters = new NameValueCollection();
            parameters.Add(Constants.AuthorizeRequest.ClientId, "implicitclient");
            parameters.Add(Constants.AuthorizeRequest.Scope, "resource profile");
            parameters.Add(Constants.AuthorizeRequest.RedirectUri, "oob://implicit/cb");
            parameters.Add(Constants.AuthorizeRequest.ResponseType, Constants.ResponseTypes.Token);

            var validator = Factory.CreateAuthorizeRequestValidator();
            var protocolResult = validator.ValidateProtocol(parameters);
            Xunit.Assert.Equal(false, protocolResult.IsError);

            var clientResult = await validator.ValidateClientAsync();
            Xunit.Assert.True(clientResult.IsError);
            Xunit.Assert.Equal(ErrorTypes.Client, clientResult.ErrorType);
            Xunit.Assert.Equal(Constants.AuthorizeErrors.InvalidScope, clientResult.Error);
        }
    }
}
