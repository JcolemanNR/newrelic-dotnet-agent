﻿// Copyright 2020 New Relic, Inc. All rights reserved.
// SPDX-License-Identifier: Apache-2.0


using MultiFunctionApplicationHelpers;
using NewRelic.Agent.IntegrationTestHelpers;
using NewRelic.Testing.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace NewRelic.Agent.UnboundedIntegrationTests.NServiceBus
{
    public abstract class NsbAsyncCmdHandlerTestsBase<TFixture> : NewRelicIntegrationTest<TFixture>
        where TFixture : ConsoleDynamicMethodFixture
    {
        private readonly ConsoleDynamicMethodFixture _fixture;

        protected NsbAsyncCmdHandlerTestsBase(TFixture fixture, ITestOutputHelper output) : base(fixture)
        {
            _fixture = fixture;
            _fixture.TestLogger = output;
            _fixture.SetTimeout(TimeSpan.FromMinutes(3));

            _fixture.AddCommand("NServiceBusDriver StartNServiceBusWithAsyncCommandHandler");
            _fixture.AddCommand("NServiceBusDriver SendCommand");
            _fixture.AddCommand("RootCommands DelaySeconds 5");
            _fixture.AddCommand("NServiceBusDriver StopNServiceBus");

            _fixture.Actions
            (
                setupConfiguration: () =>
                {
                    var configPath = fixture.DestinationNewRelicConfigFilePath;
                    var configModifier = new NewRelicConfigModifier(configPath);
                    configModifier.ForceTransactionTraces();
                }
            );

            _fixture.Initialize();
        }

        [Fact]
        public void Test()
        {
            var expectedMetrics = new List<Assertions.ExpectedMetric>
            {
                new Assertions.ExpectedMetric { metricName = @"MessageBroker/NServiceBus/Queue/Consume/Named/NsbTests.Command"},
                new Assertions.ExpectedMetric { metricName = @"MessageBroker/NServiceBus/Queue/Consume/Named/NsbTests.Command",
                                                metricScope = @"OtherTransaction/Message/NServiceBus/Queue/Named/NsbTests.Command"},
                new Assertions.ExpectedMetric { metricName = @"OtherTransaction/Message/NServiceBus/Queue/Named/NsbTests.Command"}
            };

            var expectedTransactionTraceSegments = new List<string>
            {
                @"MessageBroker/NServiceBus/Queue/Consume/Named/NsbTests.Command"
            };

            var metrics = _fixture.AgentLog.GetMetrics().ToList();

            var transactionSample = _fixture.AgentLog.TryGetTransactionSample("OtherTransaction/Message/NServiceBus/Queue/Named/NsbTests.Command");
            var transactionEvent = _fixture.AgentLog.TryGetTransactionEvent("OtherTransaction/Message/NServiceBus/Queue/Named/NsbTests.Command");

            NrAssert.Multiple(
                () => Assert.NotNull(transactionSample),
                () => Assert.NotNull(transactionEvent)
            );

            NrAssert.Multiple
            (
                () => Assertions.MetricsExist(expectedMetrics, metrics),
                () => Assertions.TransactionTraceSegmentsExist(expectedTransactionTraceSegments, transactionSample)
            );

            Assert.DoesNotContain("Transaction was garbage collected without ever ending", _fixture.AgentLog.GetFullLogAsString());
        }
    }

    [NetFrameworkTest]
    public class NsbAsyncCmdHandlerTestsFW471 : NsbAsyncCmdHandlerTestsBase<ConsoleDynamicMethodFixtureFW471>
    {
        public NsbAsyncCmdHandlerTestsFW471(ConsoleDynamicMethodFixtureFW471 fixture, ITestOutputHelper output)
            : base(fixture, output)
        {
        }
    }

    [NetFrameworkTest]
    public class NsbAsyncCmdHandlerTestsFW48 : NsbAsyncCmdHandlerTestsBase<ConsoleDynamicMethodFixtureFW48>
    {
        public NsbAsyncCmdHandlerTestsFW48(ConsoleDynamicMethodFixtureFW48 fixture, ITestOutputHelper output)
            : base(fixture, output)
        {
        }
    }

    [NetCoreTest]
    public class NsbAsyncCmdHandlerTestsCore31 : NsbAsyncCmdHandlerTestsBase<ConsoleDynamicMethodFixtureCore31>
    {
        public NsbAsyncCmdHandlerTestsCore31(ConsoleDynamicMethodFixtureCore31 fixture, ITestOutputHelper output)
            : base(fixture, output)
        {
        }
    }

    [NetCoreTest]
    public class NsbAsyncCmdHandlerTestsCore50 : NsbAsyncCmdHandlerTestsBase<ConsoleDynamicMethodFixtureCore50>
    {
        public NsbAsyncCmdHandlerTestsCore50(ConsoleDynamicMethodFixtureCore50 fixture, ITestOutputHelper output)
            : base(fixture, output)
        {
        }
    }

    [NetCoreTest]
    public class NsbAsyncCmdHandlerTestsCore60 : NsbAsyncCmdHandlerTestsBase<ConsoleDynamicMethodFixtureCore60>
    {
        public NsbAsyncCmdHandlerTestsCore60(ConsoleDynamicMethodFixtureCore60 fixture, ITestOutputHelper output)
            : base(fixture, output)
        {
        }
    }

    [NetCoreTest]
    public class NsbAsyncCmdHandlerTestsCoreLatest : NsbAsyncCmdHandlerTestsBase<ConsoleDynamicMethodFixtureCoreLatest>
    {
        public NsbAsyncCmdHandlerTestsCoreLatest(ConsoleDynamicMethodFixtureCoreLatest fixture, ITestOutputHelper output)
            : base(fixture, output)
        {
        }
    }
}
