namespace Boxing.Contracts.Requests.Count
{
    public class GetCountRequest : IRequest<int>
    {
        public string Model { get; set; }
    }
}
