﻿// Copyright 2020 New Relic, Inc. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using NewRelic.Agent.Core.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NewRelic.Agent.Core.WireModels
{
    [JsonConverter(typeof(LogEventWireModelCollectionJsonConverter))]
    public class LogEventWireModelCollection
    {
        public string EntityName { get; }

        public string EntityType { get; }

        public string EntityGuid { get; }

        public string Hostname { get; }

        public string PluginType { get; }

        public IList<LogEventWireModel> LoggingEvents { get; }

        public LogEventWireModelCollection(string entityName, string entityType, string entityGuid, string hostname, string pluginType, IList<LogEventWireModel> loggingEvents)
        {
            EntityName = entityName;
            EntityType = entityType;
            EntityGuid = entityGuid;
            Hostname = hostname;
            PluginType = pluginType;
            LoggingEvents = loggingEvents;
        }
    }
}