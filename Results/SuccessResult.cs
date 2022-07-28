namespace HotTowel.Core.Api.Results
{
    public class SuccessResult : BaseResult, IResult
    {
        public SuccessResult() : this(0, "Saved")
        {
        }

        public SuccessResult(int id) : this(id, string.Empty)
        {
        }

        public SuccessResult(string message) : this(0, message)
        {
        }

        public SuccessResult(int id, string message) : base(id, message)
        {
        }

        public SuccessResult(object data) : base(data)
        {
        }

        public SuccessResult(object data, string message) : base(data, message)
        {
        }

        public ResultType ResultType => ResultType.Success;
    }
}