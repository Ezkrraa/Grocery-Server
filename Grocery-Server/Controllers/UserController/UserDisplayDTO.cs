namespace Grocery_Server.Controllers.UserController;

public record UserDisplayDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Guid? GroupId { get; set; }

    public UserDisplayDTO(Models.User user)
    {
        Id = user.Id;
        Name = user.UserName;
        Email = user.Email;
        GroupId = user.GroupId;
    }

    public UserDisplayDTO() { }
}
