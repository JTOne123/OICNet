﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OICNet
{
    public class OicRemoteResourceRepository : IOicRemoteResourceRepository
    {
        public IList<IOicResource> Resourcse { get; }

        public OicDevice Device { get; }

        public OicRemoteResourceRepository(OicDevice device = null)
        {
            Device = device;
        }

        public virtual Task<OicResponse> CreateAsync(IOicResource resource)
        {
            throw new NotSupportedException();
        }

        public virtual Task<OicResponse> CreateOrUpdateAsync(IOicResource resource)
        {
            throw new NotSupportedException();
        }

        public virtual Task<OicResponse> DeleteAsync(IOicResource resource)
        {
            throw new NotSupportedException();
        }

        public virtual Task<OicResponse> RetrieveAsync(IOicResource resource)
        {
            if (Device == null)
                throw new NullReferenceException($"{GetType().FullName}.{nameof(Device)} cannot be null null");

            var endoint = Device.Endpoint;

            var response = endoint.Transport.SendMessageWithResponseAsync(endoint, new OicRequest
            {
                Accepts =
                {
                    OicMessageContentType.ApplicationCbor,
                    OicMessageContentType.ApplicationJson
                },
                Operation = OicRequestOperation.Get,
                ToUri = new Uri(resource.RelativeUri, UriKind.Relative)
            }).Result;

            using (var results = Device.Configuration.Serialiser.Deserialise(response.Content, response.ContentType)
                .GetEnumerator())
            {
                results.MoveNext();
                var result = results.Current;

                resource.UpdateFields(result);

                // We should not have more than one result in a response to a Retreive.
                if (results.MoveNext())
                    throw new InvalidOperationException($"Received multiple objects during {nameof(RetrieveAsync)}");
            }

            return Task.FromResult(response);
        }
    }
}
