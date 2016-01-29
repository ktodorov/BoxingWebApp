namespace Boxing.Contracts.Requests.Matches
{
    public class CancelMatchRequest : IRequest
    {
        public int Id { get; set; }
    }
}
