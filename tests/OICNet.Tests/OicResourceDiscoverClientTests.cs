﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Moq.Language;
using NUnit.Framework;


namespace OICNet.Tests
{
    [TestFixture]
    public class OicResourceDiscoverClientTests
    {
        [Test]
        public void TestDiscoverOnAllInterfaces()
        {
            // Arrange
            var mockInterface1 = new Mock<IOicTransport>();
            mockInterface1
                .Setup(b => b.BroadcastMessageAsync(It.IsAny<OicRequest>()))
                .Returns(Task.CompletedTask);
            var mockInterface2 = new Mock<IOicTransport>();
            mockInterface2
                .Setup(b => b.BroadcastMessageAsync(It.IsAny<OicRequest>()))
                .Returns(Task.CompletedTask);

            var client = new OicResourceDiscoverClient();

            client.AddTransport(mockInterface1.Object);
            client.AddTransport(mockInterface2.Object);

            // Act
            client.Discover();

            // Assert
            Mock.VerifyAll(mockInterface1, mockInterface2);
        }

        [Test]
        public void TestAddNullTransport()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var client = new OicResourceDiscoverClient();

                client.AddTransport(null);
            });
        }

        [Test]
        public void TestDiscoverDevice()
        {
            var mockInterface = new Mock<IOicTransport>();

            bool newDeviceCallbackInvoked = false;
            var service = new OicResourceDiscoverClient();
            OicDevice actualDevice = null;

            service.AddTransport(mockInterface.Object);
            service.NewDevice += (s, e) =>
            {
                newDeviceCallbackInvoked = true;
                actualDevice = e.Device;
            };

            // Act
            mockInterface.Raise(i => i.ReceivedMessage += null, new OicReceivedMessageEventArgs
            {
                Endpoint = null,
                Message = new OicResponse
                {
                    ToUri = new Uri("/oic/res", UriKind.Relative),
                    ContentType = OicMessageContentType.ApplicationCbor,
                    Content = new byte[] { 0x81, 0xA3, 0x62, 0x64, 0x69, 0x78, 0x24, 0x30, 0x36, 0x38, 0x35, 0x42, 0x39, 0x36, 0x30, 0x2D, 0x37, 0x33, 0x36, 0x46, 0x2D, 0x34, 0x36, 0x46, 0x37, 0x2D, 0x42, 0x45, 0x43, 0x30, 0x2D, 0x39, 0x45, 0x36, 0x43, 0x42, 0x44, 0x36, 0x31, 0x41, 0x44, 0x43, 0x31, 0x62, 0x72, 0x74, 0x81, 0x6A, 0x6F, 0x69, 0x63, 0x2E, 0x77, 0x6B, 0x2E, 0x72, 0x65, 0x73, 0x65, 0x6C, 0x69, 0x6E, 0x6B, 0x73, 0x84, 0xA4, 0x64, 0x68, 0x72, 0x65, 0x66, 0x66, 0x2F, 0x6F, 0x69, 0x63, 0x2F, 0x64, 0x62, 0x72, 0x74, 0x82, 0x6B, 0x6F, 0x69, 0x63, 0x2E, 0x64, 0x2E, 0x6C, 0x69, 0x67, 0x68, 0x74, 0x68, 0x6F, 0x69, 0x63, 0x2E, 0x77, 0x6B, 0x2E, 0x64, 0x62, 0x69, 0x66, 0x82, 0x68, 0x6F, 0x69, 0x63, 0x2E, 0x69, 0x66, 0x2E, 0x72, 0x6F, 0x6F, 0x69, 0x63, 0x2E, 0x69, 0x66, 0x2E, 0x62, 0x61, 0x73, 0x65, 0x6C, 0x69, 0x6E, 0x65, 0x61, 0x70, 0xA3, 0x62, 0x62, 0x6D, 0x01, 0x63, 0x73, 0x65, 0x63, 0xF5, 0x64, 0x70, 0x6F, 0x72, 0x74, 0x19, 0x7E, 0x16, 0xA4, 0x64, 0x68, 0x72, 0x65, 0x66, 0x66, 0x2F, 0x6F, 0x69, 0x63, 0x2F, 0x70, 0x62, 0x72, 0x74, 0x81, 0x68, 0x6F, 0x69, 0x63, 0x2E, 0x77, 0x6B, 0x2E, 0x70, 0x62, 0x69, 0x66, 0x82, 0x68, 0x6F, 0x69, 0x63, 0x2E, 0x69, 0x66, 0x2E, 0x72, 0x6F, 0x6F, 0x69, 0x63, 0x2E, 0x69, 0x66, 0x2E, 0x62, 0x61, 0x73, 0x65, 0x6C, 0x69, 0x6E, 0x65, 0x61, 0x70, 0xA3, 0x62, 0x62, 0x6D, 0x01, 0x63, 0x73, 0x65, 0x63, 0xF5, 0x64, 0x70, 0x6F, 0x72, 0x74, 0x19, 0x7E, 0x16, 0xA4, 0x64, 0x68, 0x72, 0x65, 0x66, 0x67, 0x2F, 0x73, 0x77, 0x69, 0x74, 0x63, 0x68, 0x62, 0x72, 0x74, 0x81, 0x73, 0x6F, 0x69, 0x63, 0x2E, 0x72, 0x2E, 0x73, 0x77, 0x69, 0x74, 0x63, 0x68, 0x2E, 0x62, 0x69, 0x6E, 0x61, 0x72, 0x79, 0x62, 0x69, 0x66, 0x82, 0x68, 0x6F, 0x69, 0x63, 0x2E, 0x69, 0x66, 0x2E, 0x61, 0x6F, 0x6F, 0x69, 0x63, 0x2E, 0x69, 0x66, 0x2E, 0x62, 0x61, 0x73, 0x65, 0x6C, 0x69, 0x6E, 0x65, 0x61, 0x70, 0xA3, 0x62, 0x62, 0x6D, 0x02, 0x63, 0x73, 0x65, 0x63, 0xF5, 0x64, 0x70, 0x6F, 0x72, 0x74, 0x19, 0x7E, 0x16, 0xA4, 0x64, 0x68, 0x72, 0x65, 0x66, 0x6B, 0x2F, 0x62, 0x72, 0x69, 0x67, 0x68, 0x74, 0x6E, 0x65, 0x73, 0x73, 0x62, 0x72, 0x74, 0x81, 0x76, 0x6F, 0x69, 0x63, 0x2E, 0x72, 0x2E, 0x6C, 0x69, 0x67, 0x68, 0x74, 0x2E, 0x62, 0x72, 0x69, 0x67, 0x68, 0x74, 0x6E, 0x65, 0x73, 0x73, 0x62, 0x69, 0x66, 0x82, 0x68, 0x6F, 0x69, 0x63, 0x2E, 0x69, 0x66, 0x2E, 0x61, 0x6F, 0x6F, 0x69, 0x63, 0x2E, 0x69, 0x66, 0x2E, 0x62, 0x61, 0x73, 0x65, 0x6C, 0x69, 0x6E, 0x65, 0x61, 0x70, 0xA3, 0x62, 0x62, 0x6D, 0x03, 0x63, 0x73, 0x65, 0x63, 0xF5, 0x64, 0x70, 0x6F, 0x72, 0x74, 0x19, 0x7E, 0x16 }
                }
            });

            // Assert
            Assert.IsTrue(newDeviceCallbackInvoked, $"{typeof(OicResourceDiscoverClient)}.{nameof(service.NewDevice)} was not invoked");

            var expectedDevice = new OicDevice()
            {
                DeviceId = Guid.Parse("0685B960-736F-46F7-BEC0-9E6CBD61ADC1"),
                Resources = {
                    new CoreResources.OicDeviceResource
                    {
                        RelativeUri = "/oid/d",
                        ResourceTypes = {"oic.d.light", "oic.wk.d"},
                        Interfaces = {OicResourceInterface.ReadOnly, OicResourceInterface.Baseline},
                    },
                    new CoreResources.OicPlatformResource
                    {
                        RelativeUri = "/oid/p",
                        ResourceTypes = {"oic.wk.p"},
                        Interfaces = {OicResourceInterface.ReadOnly, OicResourceInterface.Baseline},
                    },
                    new ResourceTypes.SwitchBinary
                    {
                        RelativeUri = "/switch",
                        ResourceTypes = {"oic.r.switch.binary"},
                        Interfaces = {OicResourceInterface.Actuator, OicResourceInterface.Baseline},
                    },
                    new ResourceTypes.LightBrightness
                    {
                        RelativeUri = "/brightness",
                        ResourceTypes = {"oic.r.light.brightness"},
                        Interfaces = {OicResourceInterface.Actuator, OicResourceInterface.Baseline},
                    }
                }
            };
            Assert.AreEqual(expectedDevice.DeviceId, actualDevice.DeviceId);
            Assert.AreEqual(expectedDevice.Resources, actualDevice.Resources);
        }
    }
}
