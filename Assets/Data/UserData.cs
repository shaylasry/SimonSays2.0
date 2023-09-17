public class UserData
{
    // Singleton instance
    private static UserData instance;

    public static UserData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UserData();
            }
            return instance;
        }
    }

    public string userName;

    private UserData() { }

    public void UpdateUserData(string newUserName)
    {
        userName = newUserName;
    }
}