using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Registration
{
    public class AppointmentSystem
    {
        private List<Department> departments;
        private const string DepartmentsFilePath = "appointments.json";
        private List<User> users;
        private const string UsersFilePath = "users.json"; 

        public AppointmentSystem()
        {
            if (File.Exists(DepartmentsFilePath))
            {
                LoadData();
            }
            else
            {
                departments = new List<Department>();
                InitializeDepartments();
            }

            if (File.Exists(UsersFilePath))
            {
                LoadUsers();
            }
            else
            {
                users = new List<User>();
            }
        }

        private void InitializeDepartments()
        {
            Department pediatrics = new Department("Pediatriya");
            pediatrics.Doctors.Add(new Doctor("Ali", "Aliyev", 10));
            pediatrics.Doctors.Add(new Doctor("Veli", "Veliyev", 7));
            pediatrics.Doctors.Add(new Doctor("Aynur", "Aynurova", 5));

            Department traumatology = new Department("Travmatologiya");
            traumatology.Doctors.Add(new Doctor("Kamil", "Kamillov", 12));
            traumatology.Doctors.Add(new Doctor("Zaur", "Zaurov", 8));

            Department stomatology = new Department("Stamotologiya");
            stomatology.Doctors.Add(new Doctor("Leyla", "Leylazade", 9));
            stomatology.Doctors.Add(new Doctor("Aysel", "Ayselova", 6));
            stomatology.Doctors.Add(new Doctor("Nigar", "Nigarova", 4));
            stomatology.Doctors.Add(new Doctor("Murad", "Muradov", 3));

            departments.Add(pediatrics);
            departments.Add(traumatology);
            departments.Add(stomatology);
        }

        public void Start()
        {
            while (true)
            {
                User user = GetUserDetails();
                users.Add(user);
                SaveUsers();

                Department department = SelectDepartment();
                Doctor doctor = SelectDoctor(department);
                string timeSlot = SelectTimeSlot(doctor);

                Console.WriteLine($"Tesekkurler {user.FirstName} {user.LastName}, siz {timeSlot} ucun {doctor.FirstName} {doctor.LastName} hekimin qebuluna yazildiniz.");
                SaveData();
            }
        }

        private User GetUserDetails()
        {
            Console.Write("Ad: ");
            string firstName = Console.ReadLine();
            Console.Write("Soyad: ");
            string lastName = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Telefon: ");
            string phone = Console.ReadLine();

            return new User(firstName, lastName, email, phone);
        }

        private Department SelectDepartment()
        {
            Console.WriteLine("Sobe seçin:");
            for (int i = 0; i < departments.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {departments[i].Name}");
            }

            int choice = int.Parse(Console.ReadLine());
            return departments[choice - 1];
        }

        private Doctor SelectDoctor(Department department)
        {
            Console.WriteLine($"{department.Name} sobesinde olan hekimler:");
            for (int i = 0; i < department.Doctors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {department.Doctors[i].FirstName} {department.Doctors[i].LastName}, Tecrube: {department.Doctors[i].Experience} il");
            }

            int choice = int.Parse(Console.ReadLine());
            return department.Doctors[choice - 1];
        }

        private string SelectTimeSlot(Doctor doctor)
        {
            while (true)
            {
                Console.WriteLine($"{doctor.FirstName} {doctor.LastName} hekiminin movcud vaxtlari:");
                foreach (var timeSlot in doctor.TimeSlots)
                {
                    Console.WriteLine($"{timeSlot.Key} - {(timeSlot.Value ? "Rezerv olunub" : "Rezerv olunmayib")}");
                }

                string selectedTimeSlot = Console.ReadLine();

                if (doctor.TimeSlots.ContainsKey(selectedTimeSlot) && !doctor.TimeSlots[selectedTimeSlot])
                {
                    doctor.TimeSlots[selectedTimeSlot] = true;
                    return selectedTimeSlot;
                }
                else
                {
                    Console.WriteLine("Secdiyiniz vaxt artiq rezerv olunub, zehmet olmasa basqa bir vaxt secin.");
                }
            }
        }

        private void SaveData()
        {
            string jsonData = JsonConvert.SerializeObject(departments, Formatting.Indented);
            File.WriteAllText(DepartmentsFilePath, jsonData);
        }

        private void LoadData()
        {
            string jsonData = File.ReadAllText(DepartmentsFilePath);
            departments = JsonConvert.DeserializeObject<List<Department>>(jsonData);
        }

        private void SaveUsers()
        {
            string jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(UsersFilePath, jsonData);
        }

        private void LoadUsers()
        {
            string jsonData = File.ReadAllText(UsersFilePath);
            users = JsonConvert.DeserializeObject<List<User>>(jsonData);
        }
    }
}
