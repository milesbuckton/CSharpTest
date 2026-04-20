namespace Server.Models
{
    /// <summary>
    /// Represents the result of the server doing something.
    /// </summary>
    public class OperationResult
    {
        public static readonly OperationResult Success = new() { IsSuccessful = true };

        public bool IsSuccessful { get; set; }

        public bool HasError => !IsSuccessful;

        public string ErrorMsg { get; set; } = string.Empty;

        public static OperationResult Error(string errorMsg) => new()
        {
            IsSuccessful = false,
            ErrorMsg = errorMsg
        };
    }
}
