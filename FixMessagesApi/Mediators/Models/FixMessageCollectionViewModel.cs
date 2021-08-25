using System.Collections.Generic;

namespace FixMessagesApi.Mediators.Models
{
    public class FixMessageCollectionViewModel
    {
        public List<FixMessageViewModel> Messages { get; set; }
        public int TotalCount { get; set; }
    }
}