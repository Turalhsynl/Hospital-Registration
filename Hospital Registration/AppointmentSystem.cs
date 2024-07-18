using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Registration
{
    public class FuncSystem
    {
        public List<Department> departments;
        public const string DepartmentsFile = "appointments.json";
        public List<User> users;
        public const string UsersFile = "users.json"; 

        public FuncSystem()
        {
            if (File.Exists(DepartmentsFile))
            {
                LoadData();
            }
            else
            {
                departments = new List<Department>();
                InitializeDepartments();
            }

            if (File.Exists(UsersFile))
            {
                LoadUsers();
            }
            else
            {
                users = new List<User>();
            }
        }

        public void InitializeDepartments()
        {
            Department pediatrics = new Department("Pediatrics");
            pediatrics.Doctors.Add(new Doctor("Nemet", "Abdullayev", 10));
            pediatrics.Doctors.Add(new Doctor("Veli", "Eliyev", 7));
            pediatrics.Doctors.Add(new Doctor("Allahverdi", "Bedelbeyli", 5));

            Department traumatology = new Department("Traumatology");
            traumatology.Doctors.Add(new Doctor("Nazim", "Huseynov", 12));
            traumatology.Doctors.Add(new Doctor("Natiq", "Hesenov", 8));

            Department stomatology = new Department("Stomatology");
            stomatology.Doctors.Add(new Doctor("Leyla", "Kerimli", 9));
            stomatology.Doctors.Add(new Doctor("Orxan", "Babayev", 6));
            stomatology.Doctors.Add(new Doctor("Nigar", "Ehmedli", 4));
            stomatology.Doctors.Add(new Doctor("Dadas", "Dadasov", 3));

            departments.Add(pediatrics);
            departments.Add(traumatology);
            departments.Add(stomatology);
        }

        public User GetUserDetails()
        {
            try
            {
                Console.Write("Name: ");
                string firstName = Console.ReadLine();
                Console.Write("Surname: ");
                string lastName = Console.ReadLine();

                string email;
                while (true)
                {
                    Console.Write("Email: ");
                    email = Console.ReadLine();

                    try
                    {
                        var addr = new System.Net.Mail.MailAddress(email);
                        string[] allowedDomains = { "gmail.com", "mail.ru", "yandex.ru", "e-mail.com", "icloud.com", "outlook.com", "yahoo.com" };
                        string domain = email.Split('@')[1];
                        bool isValidDomain = false;

                        foreach (string allowedDomain in allowedDomains)
                        {
                            if (domain.Equals(allowedDomain, StringComparison.OrdinalIgnoreCase))
                            {
                                isValidDomain = true;
                                break;
                            }
                        }

                        if (isValidDomain)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong domain or mail. Please try again (ex: abc@gmail.com)");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Wrong mail format.");
                    }
                }

                string phone;
                while (true)
                {
                    Console.Write("Mobile: ");
                    phone = Console.ReadLine();

                    bool isValidPhone = true;
                    foreach (char c in phone)
                    {
                        if (!char.IsDigit(c))
                        {
                            isValidPhone = false;
                            break;
                        }
                    }

                    if (isValidPhone)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Mobile must be a number! Try again.");
                    }
                }

                return new User(firstName, lastName, email, phone);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public Department SelectDepartment()
        {
            try
            {
                Console.WriteLine("Select department:");
                for (int i = 0; i < departments.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}]. [{departments[i].Name}]");
                    Console.WriteLine("---------------------------");
                }

                int choice;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice >= 1 && choice <= departments.Count)
                        {
                            return departments[choice - 1];
                        }
                        else
                        {
                            Console.WriteLine("Wrong choice! Please enter 1, 2, or 3.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong choice! Please enter 1, 2, or 3.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while selecting department: {ex.Message}");
            }

            return null;
        }

        public Doctor SelectDoctor(Department department)
        {
            try
            {
                Console.WriteLine($"Doctors in {department.Name}:");
                for (int i = 0; i < department.Doctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {department.Doctors[i].FirstName} {department.Doctors[i].LastName}, Experience: {department.Doctors[i].Experience} years");
                }

                int choice;
                while (true)
                {
                    Console.Write("Enter your choice: ");
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice >= 1 && choice <= department.Doctors.Count)
                        {
                            return department.Doctors[choice - 1];
                        }
                        else
                        {
                            Console.WriteLine($"Wrong choice! Please enter a number between 1 and {department.Doctors.Count}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong input! Please enter a valid number.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while selecting a doctor: {ex.Message}");
                return null;
            }
        }

        public string SelectTimeSlot(Doctor doctor)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine($"Available time slots for Dr. {doctor.FirstName} {doctor.LastName}:");
                    foreach (var timeSlot in doctor.TimeSlots)
                    {
                        Console.WriteLine($"{timeSlot.Key} - {(timeSlot.Value ? "Reserved" : "Not Reserved")}");
                    }

                    Console.Write("Enter your preferred time slot (ex: 08:00-12:00): ");
                    string selectedTimeSlot = Console.ReadLine();

                    if (doctor.TimeSlots.ContainsKey(selectedTimeSlot) && !doctor.TimeSlots[selectedTimeSlot])
                    {
                        doctor.TimeSlots[selectedTimeSlot] = true;
                        return selectedTimeSlot;
                    }
                    else
                    {
                        Console.WriteLine("The selected time slot is either reserved or invalid. Please select another time slot.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while selecting a time slot: {ex.Message}");
                return null;
            }
        }

        public void SaveData()
        {
            string jsonData = JsonConvert.SerializeObject(departments, Formatting.Indented);
            File.WriteAllText(DepartmentsFile, jsonData);
        }

        public void LoadData()
        {
            string jsonData = File.ReadAllText(DepartmentsFile);
            departments = JsonConvert.DeserializeObject<List<Department>>(jsonData);
        }

        public void SaveUsers()
        {
            string jsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(UsersFile, jsonData);
        }

        public void LoadUsers()
        {
            string jsonData = File.ReadAllText(UsersFile);
            users = JsonConvert.DeserializeObject<List<User>>(jsonData);
        }
    }
}
