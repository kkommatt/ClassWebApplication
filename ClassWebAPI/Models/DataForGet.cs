namespace ClassWebAPI.Models
{
    public class DataForGet<T>
    {
        public List<T> Models { get; set; }
        public string NextLink { get; set; }

        public DataForGet(List<T> _Models, string _nextLink)
        {
            Models = _Models;
            NextLink = _nextLink;
        }
    }
}
