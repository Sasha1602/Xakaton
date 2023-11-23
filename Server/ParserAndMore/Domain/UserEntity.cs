namespace Domain;

public class UserEntity
{
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
    
    public Guid Id { get; private set; }
}