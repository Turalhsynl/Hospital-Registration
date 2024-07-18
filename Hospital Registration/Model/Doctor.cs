namespace Hospital_Registration
{
    public class Doctor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Experience { get; set; }
        public Dictionary<string, bool> TimeSlots { get; set; }

        public Doctor(string firstName, string lastName, int experience)
        {
            FirstName = firstName;
            LastName = lastName;
            Experience = experience;
            TimeSlots = new Dictionary<string, bool>
        {
            {"09:00-11:00", false},
            {"12:00-14:00", false},
            {"15:00-17:00", false}
        };
        }
    }
}
