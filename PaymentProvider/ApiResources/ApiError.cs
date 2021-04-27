using System.Diagnostics;

namespace PaymentProvider.ApiResources
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public class ApiError
    {
        public string Type { get; set; }

        public string Message { get; set; }

        private string DebuggerDisplay => $"{Type}: {Message}";

        public override string ToString() => DebuggerDisplay;
    }
}
