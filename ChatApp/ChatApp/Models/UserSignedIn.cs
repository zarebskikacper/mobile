namespace ChatApp.Models;
public class UserSignedIn
{
    public DateTime CreatedOn => DateTime.Now;
    public string FormattedCreatedOn => CreatedOn.ToString("hh:mm:ss");
    public string Login { get; set; }
}
