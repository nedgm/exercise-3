namespace FixMessagesApi.DataLayer.Models
{
    public class FixMessageDataModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public long SendingTime { get; set; }
        public string Data { get; set; }
    }
}
