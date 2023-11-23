using System.ComponentModel.DataAnnotations;

namespace Domain;

public class UserEntity
{
    [Key]
    public Guid Id { get; private set; }
    
    private string _password;

    private string _login;

    public string Password
    {
        get
        {
            return _password;
        }
        set
        {
            _password = value;
        }
    }
    
    public string Login
    {
        get
        {
            return _login;
        }

        set
        {
            _login = value;
        }
    }

    public UserEntity(string password, string login)
    {
        _password = password;
        _login = login;
        Id = Guid.NewGuid();
    }
}