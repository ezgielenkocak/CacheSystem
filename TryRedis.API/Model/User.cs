namespace TryRedis.API.Model
{
    public class User
    {
        public User()
        {

        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public User(string name, string surname)
        {
            Name = name;
            SurName = surname;
            Id = Guid.NewGuid();
        }

    }
}
