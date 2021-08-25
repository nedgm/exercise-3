using System;
using System.Collections.Generic;
using System.Linq;
using FixMessagesApi.DataLayer.Exceptions;
using FixMessagesApi.DataLayer.Managers;
using FixMessagesApi.DataLayer.Models;
using FixMessagesApi.Helpers;
using FixMessagesApi.Mediators.Exceptions;
using FixMessagesApi.Mediators.Models;

namespace FixMessagesApi.Mediators
{
    public class FixMessageMediator
    {
        private readonly FixMessageDataManager _fixMessageDataManager;
        private Dictionary<string, string> _fieldNameMappings;

        private Dictionary<string, string> FieldNameMappings
        {
            get
            {
                if (_fieldNameMappings == null)
                {
                    _fieldNameMappings = _fixMessageDataManager.GetFieldNameMappings();
                }

                return _fieldNameMappings;
            }
        }

        public FixMessageMediator(FixMessageDataManager fixMessageDataManager)
        {
            _fixMessageDataManager = fixMessageDataManager;
        }

        public FixMessageViewModel GetFixMessageById(int id)
        {
            try
            {
                var fixMessageDataModel = _fixMessageDataManager.GetFixMessageById(id);
                var result = Map(fixMessageDataModel);
                return result;
            }
            catch (EntityNotFoundDataException)
            {
                throw new EntityNotFoundException($"FIX message with id {id} not found");
            }
        }

        public FixMessageCollectionViewModel GetFixMessagesByDescription(string description, int? offset, int? count)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("GetFixMessagesByDescription. Description is empty, white space or null");
            }

            var fixMessageDataModels = _fixMessageDataManager.GetFixMessagesByDescription(description, offset, count);
            var result = new FixMessageCollectionViewModel
            {
                Messages = fixMessageDataModels.Item1.Select(Map).ToList(),
                TotalCount = fixMessageDataModels.Item2
            }; return result;
        }

        public FixMessageCollectionViewModel GetFixMessagesBySendingTime(string fromDateTime, string toDateTime, int? offset, int? count)
        {
            if (string.IsNullOrWhiteSpace(fromDateTime))
            {
                throw new ArgumentException("GetFixMessagesBySendingTime. From date time is not specified");
            }
            if (string.IsNullOrWhiteSpace(toDateTime))
            {
                throw new ArgumentException("GetFixMessagesBySendingTime. To date time is not specified");
            }

            var fixMessageDataModels = _fixMessageDataManager.GetFixMessagesBySendingTime(fromDateTime, toDateTime, offset, count);
            var result = new FixMessageCollectionViewModel
            {
                Messages = fixMessageDataModels.Item1.Select(Map).ToList(),
                TotalCount = fixMessageDataModels.Item2
            };

            return result;
        }

        private FixMessageViewModel Map(FixMessageDataModel dataModel)
        {
            var result = new FixMessageViewModel
            {
                Properties = new List<FixMessageKeyValuePairViewModel>()
            };

            var keyValuePairs = FixMessageDataParser.TryParseFixMessage(dataModel.Data);
            foreach (var keyValuePair in keyValuePairs)
            {
                result.Properties.Add(new FixMessageKeyValuePairViewModel
                {
                    Key = TryMapPropertyKey(keyValuePair.Key),
                    Value = keyValuePair.Value
                });
            }

            return result;
        }

        private string TryMapPropertyKey(string key)
        {
            if (FieldNameMappings.ContainsKey(key))
            {
                return FieldNameMappings[key];
            }

            return key;
        }
    }
}