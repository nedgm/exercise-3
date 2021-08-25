using System;
using System.Collections.Generic;

namespace FixMessagesApi.Helpers
{
    public static class FixMessageDataParser
    {
        private const char KeyValuePairEntrySeparator = '\u0001';
        private const char KeyValuePairSeparator = '=';

        public static List<KeyValuePair<string, string>> TryParseFixMessage(string rawFixMessage)
        {
            var result = new List<KeyValuePair<string, string>>();
            var rawKeyValuePairs = rawFixMessage.Split(KeyValuePairEntrySeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var rawKeyValuePair in rawKeyValuePairs)
            {
                var keyValuePair = TryParseKeyValueEntry(rawKeyValuePair);
                if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
                {
                    result.Add(keyValuePair);
                }
            }

            return result;
        }

        public static KeyValuePair<string, string> TryParseKeyValueEntry(string rawKeyValuePair)
        {
            var delimiterIndex = rawKeyValuePair.IndexOf(KeyValuePairSeparator);
            if (delimiterIndex == -1)
            {
                return new KeyValuePair<string, string>(null, null);
            }
            var key = rawKeyValuePair.Substring(0, delimiterIndex);
            var value = rawKeyValuePair.Substring(delimiterIndex + 1);

            return new KeyValuePair<string, string>(key, value);
        }
    }
}