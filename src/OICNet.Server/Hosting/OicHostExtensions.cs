﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
#if NETSTANDARD1_5
using System.Reflection;
using System.Runtime.Loader;
#endif

namespace OICNet.Server.Hosting
{
    public static class OicHostExtensions
    {
        /// <summary>
        /// Runs a web application and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="host">The <see cref="OicHost"/> to run.</param>
        public static void Run(this OicHost host)
        {
            var done = new ManualResetEventSlim(false);
            using (var cts = new CancellationTokenSource())
            {
                Action shutdown = () =>
                {
                    if (!cts.IsCancellationRequested)
                    {
                        Console.WriteLine("Application is shutting down...");
                        cts.Cancel();
                    }

                    done.Wait();
                };

#if NETSTANDARD1_5
                var assemblyLoadContext = AssemblyLoadContext.GetLoadContext(typeof(WebHostExtensions).GetTypeInfo().Assembly);
                assemblyLoadContext.Unloading += context => shutdown();
#endif
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    shutdown();
                    // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                    eventArgs.Cancel = true;
                };

                host.Run(cts.Token, "Application started. Press Ctrl+C to shut down.");
                done.Set();
            }
        }

        /// <summary>
        /// Runs a web application and block the calling thread until token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="host">The <see cref="OicHost"/> to run.</param>
        /// <param name="token">The token to trigger shutdown.</param>
        public static void Run(this OicHost host, CancellationToken token)
        {
            host.Run(token, shutdownMessage: null);
        }

        private static void Run(this OicHost host, CancellationToken token, string shutdownMessage)
        {
            using (host)
            {
                host.Start();

                var hostingEnvironment = host.Services.GetService<IHostingEnvironment>();
                var applicationLifetime = host.Services.GetService<IApplicationLifetime>();

                Console.WriteLine($"Hosting environment: {hostingEnvironment.EnvironmentName}");

                //var serverAddresses = host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
                //if (serverAddresses != null)
                //{
                //    foreach (var address in serverAddresses)
                //    {
                //        Console.WriteLine($"Now listening on: {address}");
                //    }
                //}

                if (!string.IsNullOrEmpty(shutdownMessage))
                {
                    Console.WriteLine(shutdownMessage);
                }

                token.Register(state =>
                {
                    ((IApplicationLifetime)state).StopApplication();
                },
                applicationLifetime);

                applicationLifetime.ApplicationStopping.WaitHandle.WaitOne();
            }
        }
    }
}