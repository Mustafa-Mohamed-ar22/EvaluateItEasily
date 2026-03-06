
namespace EvaluateItEasily.Core.Results
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; } = default!;
        public Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);

        public static Result<TData> Success<TData>(TData data) => new(data, true, Error.None);
        public static Result<TData> Failure<TData>(Error error) => new(default, false, error);
    }
    public class Result<TData> : Result
    {
        private readonly TData? _data;
        public Result(TData? data, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _data = data;
        }
        public TData Data => IsSuccess ?
            _data! :
            throw new InvalidOperationException("Failure Results Can't have Data");
    }
}
