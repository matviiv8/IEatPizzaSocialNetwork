using IEatPizzaSocialNetwork.WebUI.Paging.Model;

namespace IEatPizzaSocialNetwork.WebUI.Models
{
    public class ShowAllFeedsViewModel
    {
        public IEnumerable<FeedViewModel> AllFeeds { get; set; }

        public PageViewModel PagingInfo { get; set; }

    }
}
