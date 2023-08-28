using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
        static async Task Main()
        {
            await Menu();
            Console.ReadLine();
        }

        /// <summary>
        /// Главное меню
        /// </summary>
        /// <returns></returns>
        static async Task Menu()
        {
            ConsoleKeyInfo key;

            do
            {
                Console.Clear();

                Console.WriteLine("MENU\n");
                Console.WriteLine("1: Find customer by Id");
                Console.WriteLine("2: Add customer");
                Console.WriteLine("\nX: Exit");

                Console.WriteLine();
                Console.Write("Press key to continue: ");
                key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        await GetCustomerById();
                        break;

                    case ConsoleKey.D2:
                        await CreateNewCustomer();
                        break;

                    case ConsoleKey.X:
                        break;
                }

            } while (key.Key != ConsoleKey.X);
        }

        /// <summary>
        /// Поиск покупателя по Id
        /// </summary>
        /// <returns></returns>
        static async Task GetCustomerById()
        {
            Console.Clear();

            Console.WriteLine("GET CUSTOMER BY ID\n");
            Console.Write("Enter Id: ");
            var input = Console.ReadLine();

            // при ошибке ввода возврат в меню
            if (!long.TryParse(input, out long id))
                return;

            using var webClient = new HttpClient();

            var url = $"https://localhost:5001/customers/{id}";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await webClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var customer = JsonSerializer.Deserialize<Customer>(content);

            Console.WriteLine($"Firstname: {customer.Firstname}");
            Console.WriteLine($"Lastname: {customer.Lastname}");

            Console.Write("\nPress any key to return... ");
            Console.ReadLine();
        }

        /// <summary>
        /// Создать нового покупателя
        /// </summary>
        /// <returns></returns>
        static async Task CreateNewCustomer()
        {
            Console.Clear();

            // создаем случайного
            var customer = RandomCustomer();

            Console.WriteLine("ADD NEW CUSTOMER\n");
            Console.WriteLine($"Firstname: {customer.Firstname}");
            Console.WriteLine($"Lastname: {customer.Lastname}");

            var json = JsonSerializer.Serialize(customer);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var webClient = new HttpClient();
            var url = "https://localhost:5001/customers";

            using var response = await webClient.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"\nCustomer has been created, Id = {result}");

            Console.Write("\nPress any key to return... ");
            Console.ReadLine();
        }

        /// <summary>
        /// Генерировать случайного покупателя
        /// </summary>
        /// <returns></returns>
        private static CustomerCreateRequest RandomCustomer()
        {
            string[] firstNames = { "John", "Paul", "Ringo", "George", "Max", "Natalia", "Margaret", "Helen", "Julia" };
            string[] lastNames = { "Lennon", "McCartney", "Starr", "Harrison", "Ivanova", "Petrova", "Buzova" };
            
            var random = new Random();

            return new CustomerCreateRequest
            {
                Firstname = firstNames[random.Next(0, firstNames.Length)],
                Lastname = lastNames[random.Next(0, lastNames.Length)]
            };
        }
    }
}