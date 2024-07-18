using Hospital_Registration;

class Program
{
    static void Main(string[] args)
    {
        FuncSystem appointmentSystem = new FuncSystem();
        bool system = true;
        while (system)
        {
            User user = appointmentSystem.GetUserDetails();
            appointmentSystem.users.Add(user);
            appointmentSystem.SaveUsers();

            Department department = appointmentSystem.SelectDepartment();
            Doctor doctor = appointmentSystem.SelectDoctor(department);
            string timeSlot = appointmentSystem.SelectTimeSlot(doctor);

            Console.WriteLine($"Thank you, {user.FirstName} {user.LastName}, you have been scheduled for an appointment with Dr. {doctor.FirstName} {doctor.LastName} at {timeSlot}.");
            appointmentSystem.SaveData();
            Console.WriteLine("Continue? (Yes/No)");
            string continueChoice = Console.ReadLine();
            if (continueChoice == "Yes" || continueChoice == "yes")
            {
                Console.Clear();
            }
            else
            {
                system = false;
            }
        }
    }
}
