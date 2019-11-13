﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace NewRelic.SystemInterfaces
{
	public class Environment : IEnvironment
	{
		public string GetEnvironmentVariable(string variable)
		{
			return System.Environment.GetEnvironmentVariable(variable);
		}

		public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget environmentVariableTarget)
		{
			return System.Environment.GetEnvironmentVariable(variable, environmentVariableTarget);
		}

		public Dictionary<string, string> GetEnvironmentVariablesWithPrefix(string prefix)
		{
			var environmentVariables = System.Environment.GetEnvironmentVariables();

			Dictionary<string, string> result = null;

			foreach (DictionaryEntry entry in environmentVariables)
			{
				var key = entry.Key.ToString();
				if (key.StartsWith(prefix))
				{
					if(result == null)
					{
						result = new Dictionary<string, string>();
					}

					result.Add(key.ToString(), entry.Value.ToString());
				}
			}
			return result;
		}
	}
}