namespace GetSit.Data.Security
{
    public interface IAbstractUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
