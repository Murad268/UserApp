using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.db;
using UserApp.helpers;
using UserApp.models;
using UserApp.enums;

namespace UserApp.controllers
{
    internal class LoginController
    {
        Users usersDb;
        LoginHelper loginHelper;

        public LoginController()
        {
            usersDb = new Users();
            loginHelper = new LoginHelper();
        }

        public void SignUp()
        {
            string name;
            do
            {
                Console.WriteLine("Adınızı daxil edin:");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Ad boş ola bilməz. Zəhmət olmasa yenidən daxil edin.");
                }
            } while (string.IsNullOrWhiteSpace(name));

            string surname;
            do
            {
                Console.WriteLine("Soyadınızı daxil edin:");
                surname = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(surname))
                {
                    Console.WriteLine("Soyad boş ola bilməz. Zəhmət olmasa yenidən daxil edin.");
                }
            } while (string.IsNullOrWhiteSpace(surname));

            int age;
            do
            {
                Console.WriteLine("Yaşınızı daxil edin:");
                string ageInput = Console.ReadLine();

                if (!int.TryParse(ageInput, out age) || age <= 0)
                {
                    Console.WriteLine("Zəhmət olmasa düzgün yaş daxil edin.");
                }
            } while (age <= 0);

            string email;
            do
            {
                Console.WriteLine("Email ünvanınızı daxil edin:");
                email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Email boş ola bilməz.");
                }
                else if (usersDb.users.Any(u => u.Email == email))
                {
                    Console.WriteLine("Bu email artıq mövcuddur, başqa bir email daxil edin.");
                    email = null;
                }
            } while (string.IsNullOrWhiteSpace(email));

            string password;
            do
            {
                Console.WriteLine("Şifrənizi daxil edin (ən azı 8 simvol):");
                password = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                {
                    Console.WriteLine("Şifrəniz ən azı 8 simvol olmalıdır.");
                }
            } while (string.IsNullOrWhiteSpace(password) || password.Length < 8);

            string hashedPassword = PasswordHelper.HashPassword(password);

            User newUser = new User()
            {
                Id = usersDb.users.Max(u => u.Id) + 1,
                Name = name,
                Surname = surname,
                Age = age,
                Email = email,
                Password = hashedPassword,
                UserRole = UserRole.User
            };

            usersDb.users.Add(newUser);

            Console.WriteLine("Qeydiyyat uğurla tamamlandı! Yeni istifadəçi əlavə edildi:");
            Console.WriteLine($"ID: {newUser.Id}, Ad: {newUser.Name}, Email: {newUser.Email}");
        }

        public void SignIn()
        {
            bool loggedIn = false;

            while (!loggedIn)
            {
                Console.WriteLine("Email daxil edin:");
                string email = Console.ReadLine();

                Console.WriteLine("Şifrəni daxil edin:");
                string password = Console.ReadLine();

                var user = usersDb.users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    bool passwordMatches = PasswordHelper.VerifyPassword(user.Password, password);
                    if (passwordMatches)
                    {
                        Console.WriteLine("Sistemə uğurla daxil oldunuz!");
                        loggedIn = true;

                        if (user.UserRole == UserRole.Admin)
                        {
                            AdminPanel(user);
                        }
                        else
                        {
                            UserPanel(user);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Şifrə yanlışdır. Yenidən cəhd edin.");
                    }
                }
                else
                {
                    Console.WriteLine("Email tapılmadı. Yenidən cəhd edin.");
                }
            }
        }

        public void UserPanel(User currentUser)
        {
            bool continueLoop = true;

            while (continueLoop)
            {
                Console.WriteLine("1 - Məlumatları yenilə");
                Console.WriteLine("2 - Hesabdan çıxış");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UpdateUserInfo(currentUser, true);
                        break;
                    case "2":
                        continueLoop = false;
                        Console.WriteLine("Hesabdan çıxış edildi.");
                        break;
                    default:
                        Console.WriteLine("Yanlış seçim etdiniz, yenidən cəhd edin.");
                        break;
                }
            }
        }

        public void AdminPanel(User currentUser)
        {
            bool continueLoop = true;

            while (continueLoop)
            {
                Console.WriteLine("Admin panelinə xoş gəldiniz!");
                Console.WriteLine("1 - Bütün istifadəçiləri gör");
                Console.WriteLine("2 - İstifadəçi yenilə");
                Console.WriteLine("3 - İstifadəçi sil");
                Console.WriteLine("4 - İstifadəçini admin elan et");
                Console.WriteLine("5 - Hesabdan çıxış");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllUsers();
                        break;
                    case "2":
                        UpdateUserById(currentUser);
                        break;
                    case "3":
                        DeleteUserById(currentUser);
                        break;
                    case "4":
                        PromoteUserToAdmin();
                        break;
                    case "5":
                        continueLoop = false;
                        Console.WriteLine("Hesabdan çıxış edildi.");
                        break;
                    default:
                        Console.WriteLine("Yanlış seçim etdiniz, yenidən cəhd edin.");
                        break;
                }
            }
        }

        public void PromoteUserToAdmin()
        {
            int userId = GetValidUserId("Admin elan etmək istədiyiniz istifadəçinin ID-ni daxil edin:");
            var userToPromote = usersDb.users.FirstOrDefault(u => u.Id == userId);

            if (userToPromote != null)
            {
                if (userToPromote.UserRole == UserRole.Admin)
                {
                    Console.WriteLine("Bu istifadəçi artıq admindir.");
                }
                else
                {
                    userToPromote.UserRole = UserRole.Admin;
                    Console.WriteLine($"İstifadəçi {userToPromote.Name} admin elan edildi.");
                }
            }
            else
            {
                Console.WriteLine("İstifadəçi tapılmadı.");
            }
        }

        public void ShowAllUsers()
        {
            Console.WriteLine("İstifadəçilər siyahısı:");
            foreach (var user in usersDb.users)
            {
                Console.WriteLine($"ID: {user.Id}, Ad: {user.Name}, Soyad: {user.Surname}, Email: {user.Email}, Rol: {user.UserRole}");
            }
        }

        public void UpdateUserById(User currentUser)
        {
            int userId = GetValidUserId("Yeniləmək istədiyiniz istifadəçinin ID-ni daxil edin:");
            var userToUpdate = usersDb.users.FirstOrDefault(u => u.Id == userId);

            if (userToUpdate != null)
            {
                bool isCurrentUser = (currentUser.Id == userToUpdate.Id);
                UpdateUserInfo(userToUpdate, isCurrentUser);
            }
            else
            {
                Console.WriteLine("İstifadəçi tapılmadı.");
            }
        }

        public void UpdateUserInfo(User userToUpdate, bool isCurrentUser)
        {
            bool requiresLogout = false;

            Console.WriteLine($"Ad: {userToUpdate.Name}. Yeniləmək üçün yeni ad daxil edin (əgər dəyişməyəcəksinizsə, Enter basın):");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                userToUpdate.Name = newName;
            }

            Console.WriteLine($"Soyad: {userToUpdate.Surname}. Yeniləmək üçün yeni soyad daxil edin (əgər dəyişməyəcəksinizsə, Enter basın):");
            string newSurname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newSurname))
            {
                userToUpdate.Surname = newSurname;
            }

        StartAge:
            Console.WriteLine($"Yaş: {userToUpdate.Age}. Yeniləmək üçün yeni yaş daxil edin (əgər dəyişməyəcəksinizsə, Enter basın):");
            string newAgeInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAgeInput))
            {
                int newAge;
                if (int.TryParse(newAgeInput, out newAge) && newAge > 0)
                {
                    userToUpdate.Age = newAge;
                }
                else
                {
                    Console.WriteLine("Düzgün yaş daxil edilmədi.");
                    goto StartAge;
                }
            }

            Console.WriteLine($"Email: {userToUpdate.Email}. Yeniləmək üçün yeni email daxil edin (əgər dəyişməyəcəksinizsə, Enter basın):");
            string newEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEmail) && !usersDb.users.Any(u => u.Email == newEmail))
            {
                userToUpdate.Email = newEmail;
                Console.WriteLine("Email yeniləndi.");
                requiresLogout = true;
            }
            else if (!string.IsNullOrWhiteSpace(newEmail) && usersDb.users.Any(u => u.Email == newEmail))
            {
                Console.WriteLine("Bu email artıq mövcuddur.");
            }

        StartPass:
            Console.WriteLine("Yeni şifrə daxil edin - min: 8 simvol (əgər dəyişməyəcəksinizsə, Enter basın):");
            string newPassword = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword.Length >= 8)
                {
                    userToUpdate.Password = PasswordHelper.HashPassword(newPassword);
                    Console.WriteLine("Şifrəniz yeniləndi.");
                    requiresLogout = true;
                }
                else
                {
                    Console.WriteLine("Şifrəniz ən azı 8 simvol olmalıdır.");
                    goto StartPass;
                }
            }

           

            Console.WriteLine("Məlumatlar uğurla yeniləndi.");
        }

        public void DeleteUserById(User currentUser)
        {
            int userId = GetValidUserId("Silinmək istədiyiniz istifadəçinin ID-ni daxil edin:");
            var userToDelete = usersDb.users.FirstOrDefault(u => u.Id == userId);

            if (userToDelete != null)
            {
                if (userToDelete.UserRole == UserRole.Admin)
                {
                    Console.WriteLine("Adminləri silmək mümkün deyil.");
                }
                else
                {
                    usersDb.users.Remove(userToDelete);
                    Console.WriteLine("İstifadəçi uğurla silindi.");
                }
            }
            else
            {
                Console.WriteLine("İstifadəçi tapılmadı.");
            }
        }

        private int GetValidUserId(string message)
        {
            int userId;
            do
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();

                if (!int.TryParse(input, out userId) || userId <= 0)
                {
                    Console.WriteLine("Düzgün ID daxil edin.");
                    userId = -1;
                }
            } while (userId <= 0);

            return userId;
        }
    }
}
