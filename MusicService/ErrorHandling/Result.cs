using System;
namespace MeNext.MusicService
{
    /// <summary>
    /// Represents a result from an operation where that result might have an error
    /// </summary>
    public class Result<T>
    {
        public T Wrapped { get; private set; }

        public bool HasError { get; private set; }
        public string ErrorMsg { get; private set; }
        public string ErrorDetails { get; private set; }

        private Result(T wrapped)
        {
            this.Wrapped = wrapped;
            this.HasError = false;
            this.ErrorMsg = null;
            this.ErrorDetails = null;
        }

        private Result(string error, string errorDetails)
        {
            this.Wrapped = default(T);
            this.HasError = true;
            this.ErrorMsg = error;
            this.ErrorDetails = errorDetails;
        }

        public static Result<T> Bad(string error, string errorDetails = "")
        {
            return new Result<T>(error, errorDetails);
        }

        public static Result<T> Good(T wrapped)
        {
            return new Result<T>(wrapped);
        }
    }
}
