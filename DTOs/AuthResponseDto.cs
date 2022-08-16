namespace WawAPI.DTOs;

public class AuthResponseDto
{
    public string Message { get; set; } = default!;
    public bool IsSuccess { get; set; } = default!;
    public IEnumerable<string> Errors { get; set; } = default!;
}
