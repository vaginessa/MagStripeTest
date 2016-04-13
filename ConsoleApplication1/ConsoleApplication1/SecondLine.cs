using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagStripeTest
{
    class SecondLine
    {
        public string standartStartSymbol;
        public string standartStartNumber;
        public string standartSymbolAfterNumber;
        public string standartSymbolAfterControlSum;
        public string standartExpDate;
        public string standartPartOfControlSum;
        public string startSymbol;
        public string number;
        public string symbolAfterNumber;
        public string expDate;
        public string controlSum;
        public string symbolAfterControlSum;

        public static SecondLine ParseSecondLine(string secondLine)
        {
            SecondLine secondLineObject = new SecondLine()
            {
                standartStartSymbol = (";"),
                standartStartNumber = ("677988"),
                standartSymbolAfterNumber = ("="),
                standartSymbolAfterControlSum = ("?"),
                standartExpDate = ("2512"),
                standartPartOfControlSum = ("1011"),
                startSymbol = secondLine.Substring(0, 1),
                number = secondLine.Substring(1, 16),
                symbolAfterNumber = secondLine.Substring(17, 1),
                expDate = secondLine.Substring(18, 4),
                controlSum = secondLine.Substring(22, 12),
                symbolAfterControlSum = secondLine.Substring(34, 1),

            };
            return secondLineObject;
        }
        /// <summary>
        /// проверка 2-й дорожки с 1-й дорожкой (без учёта ввода номера карты)
        /// </summary>
        /// <returns></returns>
        public static List<string> SecondLineCheckWithFirstLine(SecondLine cardObjectSecondLine, FirstLine cardObjectFirstLine)
        {
            List<string> errorsSecondLineList = new List<string>();
            if (!(String.Equals(cardObjectSecondLine.startSymbol, cardObjectSecondLine.standartStartSymbol)))
            {
                errorsSecondLineList.Add("Track 2. Первый символ некорректный");
            }
            
            if (!(String.Equals(cardObjectSecondLine.number, cardObjectFirstLine.number)))
            {
                errorsSecondLineList.Add("Track 2. PAN карты не совпадает с PAN карты в Track 1");
            }
           
            if (!(String.Equals(cardObjectSecondLine.symbolAfterNumber, cardObjectSecondLine.standartSymbolAfterNumber)))
            {
                errorsSecondLineList.Add("Track 2. Символ после PAN карты некорректный");
            }
            
            if (!(String.Equals(cardObjectSecondLine.expDate, cardObjectSecondLine.standartExpDate)))
            {
                errorsSecondLineList.Add("Track 2. ExpDate некорректный");
            }
           
            if (!(String.Equals(cardObjectSecondLine.controlSum, cardObjectFirstLine.controlSum)))
            {
                errorsSecondLineList.Add("Track 2. Контрольная сумма не совпадает с контрольной суммой в Track 1");
            }
            
            if (!(String.Equals(cardObjectSecondLine.symbolAfterControlSum, cardObjectFirstLine.symbolAfterControlSum)))
            {
                errorsSecondLineList.Add("Track 2. Символ после контрольной суммы некорректный");
            }
            return errorsSecondLineList;
        }
        /// <summary>
        /// проверка 2-й дорожки без 1-й дорожки с вводом номера карты
        /// </summary>
        /// <param name="cardObjectSecondLine"></param>
        /// <param name="cardObjectFirstLine"></param>
        /// <returns></returns>
        public static List<string> SecondLineCheckWithoutFirstLineWithFaceNumber(SecondLine cardObjectSecondLine, string faceNumber)
        {
            List<string> errorsSecondLineList = new List<string>();
            if (!(String.Equals(cardObjectSecondLine.startSymbol, cardObjectSecondLine.standartStartSymbol)))
            {
                errorsSecondLineList.Add("Track 2. Первый символ некорректный");
            }

            if (!(String.Equals(cardObjectSecondLine.number,cardObjectSecondLine.standartStartNumber+faceNumber)))
            {
                errorsSecondLineList.Add("Track 2. PAN карты не совпадает с введённым номером на лицевой стороне карты");
            }

            if (!(String.Equals(cardObjectSecondLine.symbolAfterNumber, cardObjectSecondLine.standartSymbolAfterNumber)))
            {
                errorsSecondLineList.Add("Track 2. Символ после PAN карты некорректный");
            }

            if (!(String.Equals(cardObjectSecondLine.expDate, cardObjectSecondLine.standartExpDate)))
            {
                errorsSecondLineList.Add("Track 2. ExpDate некорректный");
            }

            if (!(String.Equals(cardObjectSecondLine.controlSum.Substring(0,4), cardObjectSecondLine.standartPartOfControlSum)))
            {
                errorsSecondLineList.Add("Track 2. Начало контрольной суммы некорректно");
            }

            if (!(String.Equals(cardObjectSecondLine.symbolAfterControlSum, cardObjectSecondLine.standartSymbolAfterControlSum)))
            {
                errorsSecondLineList.Add("Track 2. Символ после контрольной суммы некорректный");
            }
            return errorsSecondLineList;
        }
        /// <summary>
        /// проверка 2-й дорожки без 1-й дорожки и без номера на лицевой стороне карты
        /// </summary>
        /// <param name="cardObjectSecondLine"></param>
        /// <param name="faceNumber"></param>
        /// <returns></returns>
        public static List<string> SecondLineCheckWithoutFirstLineWithoutFaceNumber(SecondLine cardObjectSecondLine)
        {
            List<string> errorsSecondLineList = new List<string>();
            if (!(String.Equals(cardObjectSecondLine.startSymbol, cardObjectSecondLine.standartStartSymbol)))
            {
                errorsSecondLineList.Add("Track 2. Первый символ некорректный");
            }

            if (!(String.Equals(cardObjectSecondLine.number.Substring(0,6), cardObjectSecondLine.standartStartNumber)))
            {
                errorsSecondLineList.Add("Track 2. Первые 6 цифр PAN карты некорректны");
            }

            if (!(String.Equals(cardObjectSecondLine.symbolAfterNumber, cardObjectSecondLine.standartSymbolAfterNumber)))
            {
                errorsSecondLineList.Add("Track 2. Символ после PAN карты некорректный");
            }

            if (!(String.Equals(cardObjectSecondLine.expDate, cardObjectSecondLine.standartExpDate)))
            {
                errorsSecondLineList.Add("Track 2. ExpDate некорректный");
            }

            if (!(String.Equals(cardObjectSecondLine.controlSum.Substring(0, 4), cardObjectSecondLine.standartPartOfControlSum)))
            {
                errorsSecondLineList.Add("Track 2. Начало контрольной суммы некорректное");
            }

            if (!(String.Equals(cardObjectSecondLine.symbolAfterControlSum, cardObjectSecondLine.standartSymbolAfterControlSum)))
            {
                errorsSecondLineList.Add("Track 2. Символ после контрольной суммы некорректный");
            }
            return errorsSecondLineList;
        }
    }
}



    /*public static string ReadLineTimeOut (string inputSecondLine)
    {
        //ConsoleKeyInfo cki = new ConsoleKeyInfo();
        int i = 0;
        bool stop = false;
        bool error = false;
        //do
        //{
        Console.WriteLine("\nPress a key to display; press the 'x' key to quit.");

        // Your code could perform some useful task in the following loop. However, 
        // for the sake of this example we'll merely pause for a quarter second.

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
        }
        else
        {
            //cki = Console.ReadKey(true);
            string line = Console.ReadLine();

            Console.WriteLine($"line: {line}");
        }

        //} while (cki.Key != ConsoleKey.X);
    }
}
}
*/