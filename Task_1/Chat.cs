using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Task_1
{
    internal class Chat
    {
              
        public static void Server()
        {
            UdpClient ucl = new UdpClient(12345);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 0);
            
            Console.WriteLine("Сервер ожидает сообщение от клиента.");

            while (true)
            {
                try
                {
                    byte[] buffer = ucl.Receive(ref localEP);
                    string str = Encoding.UTF8.GetString(buffer);

                    Message? msg = Message.FromJson(str);
                    if (msg != null)
                    {
                        Console.WriteLine($"{msg.ToString()}\n"); // Вывод в консоль сообщения от клиента

                        Message servResp = new Message("Server", "Сообщение получено"); // Формируем ответ клиенту, о доставке сообщения
                        string strServResp = servResp.ToJson(); // Конвертируем наше сообщение в JSON
                        byte[] byteServResp = Encoding.UTF8.GetBytes(strServResp); // Кодируем JSON в массив байтов
                        ucl.Send(byteServResp, localEP); // Отправляем пакет клиенту

                    }
                    else { Console.WriteLine("Некорректное сообщение"); }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка {e.Message}");
                }
            }
        }
        public static void Client(string nikName)
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            UdpClient ucl = new UdpClient();
          
            while (true)
            {
                Console.WriteLine("\nВведите сообщение:");
                string text = Console.ReadLine();
                
                if (String.IsNullOrEmpty(text))
                {
                    break;
                }    
                    
                Message newMessage = new Message (nikName,text);
                string js = newMessage.ToJson();
                byte[] bytes = Encoding.UTF8.GetBytes(js);
                ucl.Send(bytes, localEP);

                byte[] buffer = ucl.Receive(ref localEP); // Создаем буфер принимающий события от сервера
                string str = Encoding.UTF8.GetString(buffer); // Переводим массив байтов в строку 
                Message? msg = Message.FromJson(str); // Преобразуем строку из JSON в объект Message
                Console.WriteLine($"\n{msg.ToString()}"); // Вывод в консоль сообщения от сервера

            }
        }
    }
}
