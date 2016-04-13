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
            int k = 0;
            while (k == 0)
            {
                // запрос данных магнитной карты (проведение магнитной картой по считывателю)
                int lengthFaceNumberString = -999; // для того, чтобы зашел в цикл
                string faceNumberString = " ";
                int faceNumberNumber = 0;
                while (lengthFaceNumberString != 0 && lengthFaceNumberString != 10 && int.TryParse(faceNumberString, out faceNumberNumber))
                {
                    Console.WriteLine("Введите номер магнитной карты (10 символов), указанный на лицевой стороне или просто нажмите ENTER");
                    faceNumberString = Console.ReadLine();
                    lengthFaceNumberString = faceNumberString.Length;
                }
                Console.WriteLine("Проведите магнитной картой по считывателю");

                string firstMagEnter = "";
                string secondMagEnter = "";
                string firstLine = "";
                string secondLine = "";

                // 3 попытки ввода в случае прочтения только одной дорожки с таймаутом для 2-й дорожки
                //bool stop2 = false;
                //bool error2 = false;
                //int i0 = 0;
                //while (i0 <=2)
                //{
                //i0++;
                bool stop = false;
                bool error = false;
                int i1 = 0;
                while (i1 <= 2)
                {
                    firstMagEnter = Console.ReadLine();
                    //firstMagEnter = firstMagEnter2;
                    int i2 = 0;
                    while (Console.KeyAvailable == false && !stop)
                    {
                        Thread.Sleep(200); // Loop until input is entered.
                        i2++;
                        if (i2 >= 5)
                        {
                            stop = true;
                            //stop2 = stop;
                            error = true;
                            //error2 = error;
                        }
                    }

                    if (error)
                    {
                        //Console.ForegroundColor = ConsoleColor.DarkRed;
                        string outputMessage;
                        outputMessage = "Одна из дорожек на карте отсутствует (не прочиталась)";
                        //Console.ResetColor();
                        i1++;
                        if (i1 <= 2)
                        {
                            outputMessage += ",повторите попытку ввода карты";
                        }
                        Console.WriteLine(outputMessage);
                        bool twoLines = false;
                        //Console.ReadKey();
                        //return;
                    }
                    else
                    {
                        bool twoLines = true;
                        secondMagEnter = Console.ReadLine();
                        //secondMagEnter = secondMagEnter2;
                        //i0 = 4;
                        i1 = 3;
                        //stop = true;

                    }
                    error = false;
                    stop = false;
                }
                //}


                // вывод данных на магнитной карте(string) в неизменном виде
                //Console.WriteLine($"Первая дорожка: {firstMagEnter}");
                //Console.WriteLine($"Вторая дорожка: {secondMagEnter}");

                if ((Check.CheckIsFaceNumberEntered(faceNumberString) == true))
                {
                    //проверка в случае введения номера с лицевой стороны и правильной первой дорожке. 
                    if ((Check.CheckWhatLineIsIt(firstMagEnter)) == 1)
                    {
                        firstLine = firstMagEnter;
                        FirstLine cardObjectFirstLine = FirstLine.ParseFirstLine(firstLine);
                        var errorsFirstLine = FirstLine.FirstLineCheckWithFaceNumber(cardObjectFirstLine, faceNumberString);
                        foreach (var errorFirstLine in errorsFirstLine)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(errorFirstLine);
                            Console.ResetColor();
                        }
                        int sizeOfListFirstLineErrors = errorsFirstLine.Count;
                        if (sizeOfListFirstLineErrors <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Track 1 корректный");
                            Console.ResetColor();

                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 2)
                        {
                            secondLine = secondMagEnter;
                            SecondLine cardObjectSecondLine = SecondLine.ParseSecondLine(secondLine);
                            var errorsSecondLine = SecondLine.SecondLineCheckWithFirstLine(cardObjectSecondLine, cardObjectFirstLine);

                            foreach (var errorSecondLine in errorsSecondLine)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(errorSecondLine);
                                Console.ResetColor();
                            }

                            int sizeOfListSecondLineErrors = errorsSecondLine.Count;
                            if (sizeOfListSecondLineErrors == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Track 2 корректный");
                                Console.ResetColor();
                            }
                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Вместо Track 2 прописан Track 1\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                            Console.ResetColor();
                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Track 2 прописан некорректно либо не прописан\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                            Console.ResetColor();
                        }
                        //Console.WriteLine("2-я дорожка на карте отсутствует");


                    }
                    //проверка в случае введения номера с лицевой стороны и первый ввод = дорожка 2.
                    if ((Check.CheckWhatLineIsIt(firstMagEnter)) == 2)
                    {
                        secondLine = firstMagEnter;
                        SecondLine cardObjectSecondLine = SecondLine.ParseSecondLine(secondLine);
                        var errorsSecondLine = SecondLine.SecondLineCheckWithoutFirstLineWithFaceNumber(cardObjectSecondLine, faceNumberString);
                        foreach (var errorSecondLine in errorsSecondLine)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Track 2 находится на Дорожке 1");
                            Console.WriteLine(errorSecondLine);
                            Console.ResetColor();
                        }
                        int sizeOfListSecondLineErrors = errorsSecondLine.Count;
                        if (sizeOfListSecondLineErrors <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Track 2 находится на Дорожке 1");
                            Console.ResetColor();
                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Track 2 находится и в Дорожке 2:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                            Console.ResetColor();
                        }
                        else
                        {
                            if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Track 1 находится в Дорожке 2:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                Console.ResetColor();
                            }
                            if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Дорожка 2 некорректна:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                Console.ResetColor();
                            }
                            //Console.WriteLine("2-я дорожка на карте отсутствует");
                        }

                    }
                    //проверка в случае введения номера с лицевой стороны и первый ввод = некорректный (ни первая, ни вторая дорожки).
                    if ((Check.CheckWhatLineIsIt(firstMagEnter) == 3))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Дорожка 1 некорректна");
                        Console.ResetColor();
                        if ((Check.CheckWhatLineIsIt(secondMagEnter) == 1))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Track 1 находится на Дорожке 2:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                            Console.ResetColor();
                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter) == 2))
                        {
                            secondLine = secondMagEnter;
                            SecondLine cardObjectSecondLine = SecondLine.ParseSecondLine(secondLine);
                            var errorsSecondLine = SecondLine.SecondLineCheckWithoutFirstLineWithFaceNumber(cardObjectSecondLine, faceNumberString);

                            foreach (var errorSecondLine in errorsSecondLine)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(errorSecondLine);
                                Console.WriteLine($"Первая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                Console.ResetColor();
                            }

                            int sizeOfListSecondLineErrors = errorsSecondLine.Count;
                            if (sizeOfListSecondLineErrors == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Track 2 корректный:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                Console.ResetColor();
                            }
                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter) == 3))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Track 2 некорректный:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                            Console.ResetColor();
                        }
                    }
                }
                else
                {
                    //если номер карты не введён
                    //проверка в случае правильного вида первой дорожки без ввода номера карты с лицевой стороны.
                    if ((Check.CheckWhatLineIsIt(firstMagEnter)) == 1)
                    {
                        firstLine = firstMagEnter;
                        FirstLine cardObjectFirstLine = FirstLine.ParseFirstLine(firstLine);
                        var errorsFirstLine = FirstLine.FirstLineCheckWithoutFaceNumber(cardObjectFirstLine);
                        foreach (var errorFirstLine in errorsFirstLine)
                        {
                            Console.WriteLine(errorFirstLine);
                        }
                        int sizeOfListFirstLineErrors = errorsFirstLine.Count;
                        if (sizeOfListFirstLineErrors <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Track 1 корректный");
                            Console.ResetColor();
                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 2)
                        {
                            secondLine = secondMagEnter;
                            SecondLine cardObjectSecondLine = SecondLine.ParseSecondLine(secondLine);
                            var errorsSecondLine = SecondLine.SecondLineCheckWithFirstLine(cardObjectSecondLine, cardObjectFirstLine);

                            foreach (var errorSecondLine in errorsSecondLine)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(errorSecondLine);
                                Console.ResetColor();
                            }

                            int sizeOfListSecondLineErrors = errorsSecondLine.Count;
                            if (sizeOfListSecondLineErrors == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Track 2 корректный");
                                Console.ResetColor();
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Номер карты: {cardObjectFirstLine.number.Substring(6)}");
                            Console.ResetColor();
                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Track 1 находится на Дорожке 2\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                            Console.WriteLine($"Номер карты: {cardObjectFirstLine.number.Substring(6)}");
                            Console.ResetColor();
                        }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Дорожка 2 либо некорректна, либо не прочиталась, либо не присутствует\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                            Console.WriteLine($"Номер карты: {cardObjectFirstLine.number.Substring(6)}");
                            Console.ResetColor();
                        }
                    }
                        //проверка в случае, когда первый ввод = дорожка 2 и не введён номер с лицевой стороны.
                    if ((Check.CheckWhatLineIsIt(firstMagEnter)) == 2)
                    {
                       secondLine = firstMagEnter;
                       SecondLine cardObjectSecondLine = SecondLine.ParseSecondLine(secondLine);
                       var errorsSecondLine = SecondLine.SecondLineCheckWithoutFirstLineWithoutFaceNumber(cardObjectSecondLine);

                       Console.ForegroundColor = ConsoleColor.Red;
                       Console.WriteLine("Track 2 находится на Дорожке 1");
                       Console.ResetColor();

                            foreach (var errorSecondLine in errorsSecondLine)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(errorSecondLine);
                                Console.ResetColor();
                            }
                            int sizeOfListSecondLineErrors = errorsSecondLine.Count;
                            if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Track 2 находится и на Дорожке 2:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                Console.ResetColor();
                            }
                            if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Track 1 находится на Дорожке 2:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                Console.ResetColor();
                            }
                            if ((Check.CheckWhatLineIsIt(secondMagEnter)) == 3)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Дорожка 2 некорректна:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                Console.ResetColor();
                            }

                      }

                        //проверка в случае, когда первый ввод = некорректный (ни первая, ни вторая дорожки) без ввода номера карты с лицевой стороны.
                        if ((Check.CheckWhatLineIsIt(firstMagEnter) == 3))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Дорожка 1 некорректна");
                            Console.ResetColor();

                            if ((Check.CheckWhatLineIsIt(secondMagEnter) == 1))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Track 1 находится на Дорожке 2:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                Console.ResetColor();
                            }
                            if ((Check.CheckWhatLineIsIt(secondMagEnter) == 2))
                            {
                                secondLine = secondMagEnter;
                                SecondLine cardObjectSecondLine = SecondLine.ParseSecondLine(secondLine);
                                var errorsSecondLine = SecondLine.SecondLineCheckWithoutFirstLineWithoutFaceNumber(cardObjectSecondLine);

                                foreach (var errorSecondLine in errorsSecondLine)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(errorSecondLine);
                                    Console.ResetColor();
                                }

                                int sizeOfListSecondLineErrors = errorsSecondLine.Count;
                                if (sizeOfListSecondLineErrors == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Track 2 корректный:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                    Console.ResetColor();
                                }
                                if ((Check.CheckWhatLineIsIt(secondMagEnter) == 3))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Дорожка 2 некорректна:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                                    Console.ResetColor();
                                }
                                //Console.WriteLine("2-я дорожка на карте отсутствует");
                            }
                        if ((Check.CheckWhatLineIsIt(secondMagEnter) == 3))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Дорожка 2 некорректна:\r\nПервая дорожка: {firstMagEnter}\r\nВторая дорожка: {secondMagEnter}");
                            Console.ResetColor();
                        }
                    }
                    }
                Console.WriteLine("\r\n");
            }
            
            Console.ReadKey();
        }
            //FirstLine cardObjectFirstLine = FirstLine.ParseFirstLine(firstLine);
            //Console.WriteLine(cardObject.Number);
            //Console.ReadKey();
            //FirstLine firstLine = new FirstLine();
            //string lastDigit = FirstLine.CalculateCheckDigit(cardObjectFirstLine.number.Substring(0,15));
     }     
}
