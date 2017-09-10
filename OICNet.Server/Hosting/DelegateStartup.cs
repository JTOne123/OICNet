﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using OICNet.Server.Builder;

namespace OICNet.Server.Hosting
{
    public class DelegateStartup : StartupBase<IServiceCollection>
    {
        private Action<IApplicationBuilder> _configureApp;

        public DelegateStartup(IServiceProviderFactory<IServiceCollection> factory, Action<IApplicationBuilder> configureApp) : base(factory)
        {
            _configureApp = configureApp;
        }

        public override void Configure(IApplicationBuilder app) => _configureApp(app);
    }
}