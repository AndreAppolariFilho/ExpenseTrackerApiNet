public class ReturnUserDTO
{
    public ReturnUserDTO(User user)
    {
        this.Username = user.Username;
    }
    public string Username { get; set; } = string.Empty;
}