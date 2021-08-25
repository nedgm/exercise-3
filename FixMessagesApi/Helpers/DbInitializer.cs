using FixMessagesApi.DataLayer.Managers;

namespace FixMessagesApi.Helpers
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            FixMessageDataManager.Initialize();
        }
    }
}