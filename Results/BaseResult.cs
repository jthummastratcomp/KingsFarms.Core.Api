namespace HotTowel.Core.Api.Results
{
    public class BaseResult
    {
        protected BaseResult(int id, string message)
        {
            Id = id;
            Message = message;
        }

        protected BaseResult()
        {
        }

        protected BaseResult(object data)
        {
            Data = data;
        }

        protected BaseResult(object data, string message)
        {
            Data = data;
            Message = message;
        }

        public int Id { get; }
        public string Message { get; }
        public object Data { get; set; }
    }
}