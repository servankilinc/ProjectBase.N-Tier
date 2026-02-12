using System.Diagnostics.CodeAnalysis;

namespace Core.Utils.ResultPattern
{
    public interface IResult
    {

        [MemberNotNullWhen(false, nameof(Error))]
        bool IsSuccess { get; }
        Error? Error { get; }
        string Message { get; set; }
    }
}