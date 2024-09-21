using System;
using UserApp.controllers;

namespace UserApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            LoginController loginController = new LoginController();
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Qeydiyyatdan keçmək üçün 1, hesabınıza daxil olmaq üçün 2, proqramdan çıxış etmək üçün 3 seçin:");
                string selectedOption = Console.ReadLine();
                switch (selectedOption)
                {
                    case "1":
                        loginController.SignUp();
                        break;
                    case "2":
                        loginController.SignIn();  
                        break;
                    case "3":
                        keepRunning = false;
                        Console.WriteLine("Proqramdan çıxılır.");
                        break;
                    default:
                        Console.WriteLine("Yanlış seçim etdiniz, yenidən cəhd edin.");
                        break;
                }
            }
        }
    }
}
