using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagStripeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Введите номер магнитной карты (10 символов), указанный на лицевой стороне)");
            string faceNumberString = Console.ReadLine();
            Console.WriteLine("Проведите магнитной картой по считывателю");
            string firstLineString = Console.ReadLine();

            /*DateTime dateTimeNow = DateTime.Now;
            var TimeOut = 1;
            DateTime dateTimeEnd = dateTimeNow.AddSeconds(TimeOut);
            string secondLineString = null;
            //string secondLineString = Console.ReadLine();
            while ((DateTime.Now <= dateTimeEnd) || (secondLineString != null))
            {
                secondLineString = Console.ReadLine();
            }*/

            int i = 0;
            bool stop = false;
            bool error = false;
            string secondLineString = "";

            while (Console.KeyAvailable == false && !stop)
            {
                Thread.Sleep(250); // Loop until input is entered.
                i++;
                if (i > 5)
                {
                    stop = true;
                    error = true;
                }
            }

            if (error)
            {
                Console.WriteLine("Одна из дорожек на карте отсутствует");
                return;
            }
            else
            {
                //cki = Console.ReadKey(true);
                secondLineString = Console.ReadLine();

                //Console.WriteLine($"line: {line}");
                //secondLineString = line;
            }
// запрос данных магнитной карты (проведение магнитной картой по считывателю)
            Console.WriteLine($"Первая линия: {firstLineString}");
            Console.WriteLine($"Вторая линия: {secondLineString}");
            // вывод данных на магнитной карте(string) в неизменном виде

            FirstLine cardObject = FirstLine.ParseFirstLine(firstLineString);

            if (String.Equals(cardObject.StartSymbols, "%B"))
            {
                Console.WriteLine("Первые 2 символа в первой дорожке верны");
            }
            else

            {
                Console.WriteLine("Первые 2 символа в первой дорожке неверны");
            }

            if (String.Equals(cardObject.Number.Substring(0, 6), "677988"))
            {
                if (String.Equals(cardObject.Number.Substring(6, 10), faceNumberString))

                {
                    Console.WriteLine("номер карты в дорожке 1 указан верно");
                }
                else
                {
                    Console.WriteLine("номер карты в дорожке 1 не совпадает с номером на лицевой стороне");
                }
            }
            else
            {
                Console.WriteLine("карта бракованная (ошибка в первых 6-ти цифрах номера карты)");
            }
            if (String.Equals(cardObject.SymbolAfterNumber, "^"))
            {
                Console.WriteLine("Символ после номера карты в 1-й дорожке верный");
            }
            else
            {
                Console.WriteLine("Символ после номера карты в 1-й дорожке неверный");
            }
            if (String.Equals(cardObject.CardHolderName.Substring(0, 2), "DK"))
            {
                Console.WriteLine("Первые 2 символа (DK) в CardHolderName верны");
            }
            else
            {
                Console.WriteLine("Первые 2 символа (DK) в CardHolderName неверны");
            }
            if (String.Equals(cardObject.CardHolderName.Substring(2, 6), cardObject.Number.Substring(10, 6)))
            {
                if (String.Equals(cardObject.CardHolderName.Substring(9, 4), cardObject.Number.Substring(6, 4)))
                {
                    Console.WriteLine("CardHolderName верный");
                }
                else
                {
                    Console.WriteLine("CardHolderName неверный в части FirstName");
                }
            }
            else
            {
                Console.WriteLine("CardHolderName неверный в части SecondName");
            }
            if (String.Equals(cardObject.SymbolAfterCardHolderName, "^"))
            {
                Console.WriteLine("Символ после CardHolderName верный");
            }
            else
            {
                Console.WriteLine("Символ после CardHolderName неверный");
            }
            if (String.Equals(cardObject.ExpDate, "2512"))
            {
                Console.WriteLine("ExpDate верный");
            }
            else
            {
                Console.WriteLine("ExpDate неверный");
            }
            if (String.Equals(cardObject.ControlSum.Substring(0,3),"101"))
            {
                if (String.Equals(cardObject.ControlSum.Substring(3, 1), "1"))
                {
                    Console.WriteLine("В 1-й дорожке ServiceCode верный");
                    Console.WriteLine("В 1-й дорожке контрольная цифра после ServiceCode верная");
                    
                }
                else
                {
                    Console.WriteLine("В 1-й дорожке ServiceCode верный");
                    Console.WriteLine("В 1-й дорожке контрольная цифра после ServiceCode неверная");
                }
            }
            else
            {
                Console.WriteLine("В 1-й дорожке ServiceCode неверный");
            }
            if (String.Equals(cardObject.SymbolAfterControlSum,"?"))
            {
                Console.WriteLine("В 1-й дорожке символ после контрольной суммы верный");
            }
            else
            {
                Console.WriteLine("В 1-й дорожке символ после контрольной суммы неверный");
            }
            Console.ReadKey();
        }

        
    }
}
