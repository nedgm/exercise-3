using System.Collections.Generic;
using System.IO;
using FixMessagesApi.DataLayer.Models;
using FixMessagesApi.Helpers;

namespace FixMessagesApi.DataLayer.Helpers
{
    public static class FixMessagesDbInitializationHelper
    {
        private const string IdTag = "34";
        private const string DescriptionTag = "107";
        private const string SendingTimeTag = "52";

        public static List<FixMessageDataModel> GetPredefinedFixMessages()
        {
            var messageEntries = new List<FixMessageDataModel>();
            var lines = File.ReadAllLines(@".\data\messages");
            foreach (var line in lines)
            {
                var messageEntry = new FixMessageDataModel
                {
                    Data = line
                };

                var keyValuePairs = FixMessageDataParser.TryParseFixMessage(line);
                foreach (var keyValuePair in keyValuePairs)
                {
                    if (keyValuePair.Key == IdTag)
                    {
                        if (int.TryParse(keyValuePair.Value, out var id))
                        {
                            messageEntry.Id = id;
                        }
                    }

                    if (keyValuePair.Key == DescriptionTag)
                    {
                        messageEntry.Description = keyValuePair.Value;
                    }

                    if (keyValuePair.Key == SendingTimeTag)
                    {
                        if (long.TryParse(keyValuePair.Value, out var sendingTimeAsLong))
                        {
                            messageEntry.SendingTime = sendingTimeAsLong;
                        }
                    }
                }

                messageEntries.Add(messageEntry);
            }

            return messageEntries;
        }

        public static List<FieldNameMappingDataModel> GetPredefinedFieldNameMappings()
        {
            var fieldNameMappingEntries = new List<FieldNameMappingDataModel>();
            var lines = File.ReadAllLines(@".\data\field_name_mapping");
            foreach (var line in lines)
            {
                var keyValuePair = FixMessageDataParser.TryParseKeyValueEntry(line);
                var fieldNameMappingEntry = new FieldNameMappingDataModel
                {
                    Key = keyValuePair.Key,
                    Value = keyValuePair.Value
                };

                fieldNameMappingEntries.Add(fieldNameMappingEntry);
            }

            return fieldNameMappingEntries;
        }
    }
}