using System.Collections.Generic;
using System.Linq;
using FixMessagesApi.DataLayer.Exceptions;
using FixMessagesApi.DataLayer.Helpers;
using FixMessagesApi.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace FixMessagesApi.DataLayer.Managers
{
    public class FixMessageDataManager
    {
        private readonly string _connectionString;

        public FixMessageDataManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void Initialize()
        {
            var connectionString = Startup.StaticConfiguration["FixMessageDbConnectionString"];
            using (var dbContext = new FixMessageDbContext(connectionString))
            {
                if (!dbContext.Database.CanConnect())
                {
                    dbContext.Database.EnsureCreated();
                    var predefinedFixMessages = FixMessagesDbInitializationHelper.GetPredefinedFixMessages();
                    var predefinedFieldNameMappings = FixMessagesDbInitializationHelper.GetPredefinedFieldNameMappings();

                    dbContext.Messages.AddRange(predefinedFixMessages);
                    dbContext.FieldNameMappings.AddRange(predefinedFieldNameMappings);
                    dbContext.SaveChanges();
                }
            }
        }

        public FixMessageDataModel GetFixMessageById(int id)
        {
            using (var dbContext = new FixMessageDbContext(_connectionString))
            {
                var result = dbContext.Messages.FirstOrDefault(m => m.Id == id);
                if (result == null)
                {
                    throw new EntityNotFoundDataException();
                }
                return result;
            }
        }

        public (IReadOnlyCollection<FixMessageDataModel>, int) GetFixMessagesByDescription(string description, int? offset, int? count = 100)
        {
            using (var dbContext = new FixMessageDbContext(_connectionString))
            {
                var totalCount = dbContext.Messages.Count(m => m.Description.ToUpper().Contains(description.ToUpper()));
                var messages = dbContext.Messages
                    .Skip(offset ?? 0)
                    .Where(m => m.Description.ToUpper().Contains(description.ToUpper()))
                    .Take(count.GetValueOrDefault(100))
                    .ToList();

                return (messages, totalCount);
            }
        }

        public (IReadOnlyCollection<FixMessageDataModel>, int) GetFixMessagesBySendingTime(string fromDateTime, string toDateTime, int? offset, int? count = 100)
        {
            long.TryParse(fromDateTime, out var fromDateTimeAsLong);
            long.TryParse(toDateTime, out var toDateTimeAsLong);

            using (var dbContext = new FixMessageDbContext(_connectionString))
            {
                var totalCount = dbContext.Messages.Count(m => fromDateTimeAsLong <= m.SendingTime && m.SendingTime <= toDateTimeAsLong);
                var messages = dbContext.Messages
                    .Skip(offset ?? 0)
                    .Where(m => fromDateTimeAsLong <= m.SendingTime && m.SendingTime <= toDateTimeAsLong)
                    .Take(count.GetValueOrDefault(100))
                    .ToList();

                return (messages, totalCount);
            }
        }

        public Dictionary<string, string> GetFieldNameMappings()
        {
            using (var dbContext = new FixMessageDbContext(_connectionString))
            {
                var result = new Dictionary<string, string>();
                dbContext.FieldNameMappings.Select(_ => _).ForEachAsync(_ => result.Add(_.Key, _.Value));
                return result;
            }
        }
    }
}