﻿// -----------------------------------------------------------------------
// <copyright file="ServerFactory.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// Copyright 2011-2012 Katana contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNet.Abstractions;
using Microsoft.AspNet.ConfigurationModel;
using Microsoft.AspNet.Hosting.Server;
using Microsoft.AspNet.Logging;

namespace Microsoft.AspNet.Server.WebListener
{
    using AppFunc = Func<object, Task>;

    /// <summary>
    /// Implements the setup process for this server.
    /// </summary>
    public class ServerFactory : IServerFactory
    {
        private ILoggerFactory _loggerFactory;

        public ServerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Creates a configurable instance of the server.
        /// </summary>
        /// <param name="properties"></param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by caller")]
        public IServerInformation Initialize(IConfiguration config)
        {
            OwinWebListener listener = new OwinWebListener();
            ParseAddresses(config, listener);
            return new WebListenerWrapper(listener, _loggerFactory);
        }

        /// <summary>
        /// </summary>
        /// <param name="app">The per-request application entry point.</param>
        /// <param name="server">The value returned </param>
        /// <returns>The server.  Invoke Dispose to shut down.</returns>
        public IDisposable Start(IServerInformation server, AppFunc app)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            WebListenerWrapper wrapper = server as WebListenerWrapper;
            if (wrapper == null)
            {
                throw new ArgumentException("server");
            }

            // TODO: var capabilities = new Dictionary<string, object>();

            wrapper.Start(app);
            return wrapper;
        }

        private void ParseAddresses(IConfiguration config, OwinWebListener listener)
        {
            // TODO: Key format?
            string urls;
            if (config != null && config.TryGet("server.urls", out urls) && !string.IsNullOrEmpty(urls))
            {
                foreach (var value in urls.Split(';'))
                {
                    listener.UriPrefixes.Add(Prefix.Create(value));
                }
            }
            // TODO: look for just a port option?
        }
    }
}