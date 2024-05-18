namespace WebApi.Services.Interfaces
{
    public interface IChangePasswordService
    {
        public void ChangePassword(string new_password, int user_id);
    }
}
