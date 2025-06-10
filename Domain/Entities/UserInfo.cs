namespace Domain.Entities
{
    public class UserInfo
    {
        private string? _email;

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Family { get; set; }

        public string Email 
        {
            get { return _email; }

            set 
            {
                if (value.Contains("@"))
                    _email = value;
                else
                    throw new ArgumentException("Invalid email format!");
            } 
        }

        public List<WorkHistory>? WorkHistories { get; set; }
        public List<Certificate>? Certificates { get; set; }
    }
}
