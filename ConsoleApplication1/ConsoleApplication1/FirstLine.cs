using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagStripeTest
{
    class FirstLine
    {
        public string standartStartSymbols;
        public string standartStartNumber;
        public string standartSymbolAfterNumber;
        public string standartStartCardHolderName;
        public string standartSymbolAfterCardHolderName;
        public string standartPartOfControlSum;
        public string standartSymbolAfterControlSum;
        public string standartExpDate;
        public string startSymbols;
        public string number;
        public string symbolAfterNumber;
        public string cardHolderName;
        public string symbolAfterCardHolderName;
        public string expDate;
        public string controlSum;
        public string symbolAfterControlSum;
        //public string CalculateCheckDigit;

        /// <summary>
        /// Разбивка первой строки на составляющие
        /// </summary>
        /// <param name="firstLine"></param>
        /// <returns></returns>
        public static FirstLine ParseFirstLine(string firstLine)
        {
            FirstLine firstLineObject = new FirstLine()
            {
                standartStartSymbols = ("%B"),
                standartStartNumber = ("677988"),
                standartSymbolAfterNumber = ("^"),
                standartStartCardHolderName = ("DK"),
                standartSymbolAfterCardHolderName = ("^"),
                standartExpDate = ("2512"),
                standartPartOfControlSum = ("1011"),
                standartSymbolAfterControlSum = ("?"),
                startSymbols = firstLine.Substring(0, 2),
                number = firstLine.Substring(2, 16),
                symbolAfterNumber = firstLine.Substring(18, 1),
                cardHolderName = firstLine.Substring(19, 13),
                symbolAfterCardHolderName = firstLine.Substring(32, 1),
                expDate = firstLine.Substring(33, 4),
                controlSum = firstLine.Substring(37, 12),
                symbolAfterControlSum = firstLine.Substring(49, 1)
                
            };

            return firstLineObject;
        }
        /// <summary>
        /// Вычисление контрольной цифры (Алгоритм Луна)
        /// </summary>
        /// <param name="checkDigit"></param>
        /// <returns></returns>
        public static string CalculateCheckDigit(string numberOfCard) //numberOfCard - полный PAN карты
        {
            int n = 0;
            List<int> digitsNumberOfCard = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                int number = Int32.Parse(numberOfCard.Substring(n, 1));
                digitsNumberOfCard.Add(number);
                n++;
            }
                
            //numberOfCard.Select(c => c - '0').ToList();
            digitsNumberOfCard.Add(0);
            int checkDigit = (digitsNumberOfCard.Select((d, i) => i % 2 == digitsNumberOfCard.Count % 2 ? ((2 * d) % 10) + d / 5 : d).Sum() % 10);
            int lastDigit = checkDigit == 0 ? checkDigit : 10 - checkDigit;
            //Console.WriteLine($"контрольное число: {lastDigit}");
            //Console.ReadKey();

            return lastDigit.ToString();
        }

        /// <summary>
        /// проверка 1-й дорожки магнитной карты при вводе номера карты
        /// </summary>
        /// <param name="cardObjectFirstLine"></param>
        /// <param name="faceNumberString"></param>
        /// <returns></returns>
        public static List<string> FirstLineCheckWithFaceNumber(FirstLine cardObjectFirstLine, string faceNumberString) //cardObjectFirstLine - объект класса первая дорожка
        {
            List<string> errorsFirstLineList = new List<string>();
            if (!(String.Equals(cardObjectFirstLine.startSymbols, cardObjectFirstLine.standartStartSymbols)))
            {
                errorsFirstLineList.Add("Track 1. Первые 2 символа некорректны");
            }

            if (!(String.Equals(cardObjectFirstLine.number.Substring(0, 6), cardObjectFirstLine.standartStartNumber)))
            {
                errorsFirstLineList.Add("Track 1. Первые 6 цифр PAN карты некорректны");
            }
            
            if (!(String.Equals(cardObjectFirstLine.number.Substring(6, 10), faceNumberString)))
            {
                errorsFirstLineList.Add("Track 1. Номер карты не совпадает с введённым номером (на лицевой стороне)");
            }
            
            if (!(String.Equals(cardObjectFirstLine.number.Substring(15, 1), CalculateCheckDigit(cardObjectFirstLine.number))))
            {
                errorsFirstLineList.Add("Track 1. Контролльное число по алгоритму Луна некорректно");
            }

            if (!(String.Equals(cardObjectFirstLine.symbolAfterNumber, cardObjectFirstLine.standartSymbolAfterNumber)))
            {
                errorsFirstLineList.Add("Track 1. Символ после PAN карты некорректный");
            }

            if (!(String.Equals(cardObjectFirstLine.cardHolderName.Substring(0, 2), cardObjectFirstLine.standartStartCardHolderName)))
            {
                errorsFirstLineList.Add($"Track 1. Первые 2 символа ({cardObjectFirstLine.standartStartCardHolderName}) в CardHolderName некорректны");
            }
            
            if (!(String.Equals(cardObjectFirstLine.cardHolderName.Substring(2, 6), cardObjectFirstLine.number.Substring(10, 6))))
            {
                errorsFirstLineList.Add("Track 1. CardHolderName некорректный в части SecondName");
            }
            
            if (!(String.Equals(cardObjectFirstLine.cardHolderName.Substring(9, 4), cardObjectFirstLine.number.Substring(6, 4))))
            {
                errorsFirstLineList.Add("Track 1. CardHolderName некорректный в части FirstName");
            }
            
            if (!(String.Equals(cardObjectFirstLine.symbolAfterCardHolderName, cardObjectFirstLine.standartSymbolAfterCardHolderName)))
            {
                errorsFirstLineList.Add("Track 1. Символ после CardHolderName некорректный");
            }
            
            if (!(String.Equals(cardObjectFirstLine.expDate, cardObjectFirstLine.standartExpDate)))
            {
                errorsFirstLineList.Add("Track 1. ExpDate некорректный");
            }
            
            if (!(String.Equals(cardObjectFirstLine.controlSum.Substring(0, 3), cardObjectFirstLine.standartPartOfControlSum.Substring(0,3))))
            {
                errorsFirstLineList.Add("Track 1. ServiceCode некорректный");
            }
            
            if (!(String.Equals(cardObjectFirstLine.controlSum.Substring(3, 1), cardObjectFirstLine.standartPartOfControlSum.Substring(3,1))))
            {
                errorsFirstLineList.Add("Track 1. Контрольная цифра после ServiceCode некорректна");
            }
            
            if (!(String.Equals(cardObjectFirstLine.symbolAfterControlSum, cardObjectFirstLine.standartSymbolAfterControlSum)))
            {
                errorsFirstLineList.Add("Track 1. Символ после контрольной суммы некорректный");
            }
            return errorsFirstLineList;
        }
        /// <summary>
        /// проверка 1-й дорожки без ввода номера карты
        /// </summary>
        /// <param name="cardObjectFirstLine"></param>
        /// <returns></returns>
        public static List<string> FirstLineCheckWithoutFaceNumber(FirstLine cardObjectFirstLine) //cardObjectFirstLine - объект класса первая дорожка
        {
            List<string> errorsFirstLineList = new List<string>();
            if (!(String.Equals(cardObjectFirstLine.startSymbols, cardObjectFirstLine.standartStartSymbols)))
            {
                errorsFirstLineList.Add("Track 1. Первых 6 цифр PAN карты некорректны");
            }

            if (!(String.Equals(cardObjectFirstLine.number.Substring(0, 6), cardObjectFirstLine.standartStartNumber)))
            {
                errorsFirstLineList.Add("Track 1. Первые 6 цифр PAN карты некорректны");
            }

            if (!(String.Equals(cardObjectFirstLine.number.Substring(15, 1), CalculateCheckDigit(cardObjectFirstLine.number))))
            {
                errorsFirstLineList.Add("Track 1. Контролльное число по алгоритму Луна некорректно");
            }

            if (!(String.Equals(cardObjectFirstLine.symbolAfterNumber, cardObjectFirstLine.standartSymbolAfterNumber)))
            {
                errorsFirstLineList.Add("Track 1. Символ после номера карты некорректный");
            }

            if (!(String.Equals(cardObjectFirstLine.cardHolderName.Substring(0, 2), cardObjectFirstLine.standartStartCardHolderName)))
            {
                errorsFirstLineList.Add($"Track 1. Первые 2 символа ({cardObjectFirstLine.standartStartCardHolderName}) в CardHolderName некорректны");
            }

            if (!(String.Equals(cardObjectFirstLine.cardHolderName.Substring(2, 6), cardObjectFirstLine.number.Substring(10, 6))))
            {
                errorsFirstLineList.Add("Track 1. CardHolderName некорректный в части SecondName");
            }

            if (!(String.Equals(cardObjectFirstLine.cardHolderName.Substring(9, 4), cardObjectFirstLine.number.Substring(6, 4))))
            {
                errorsFirstLineList.Add("Track 1. CardHolderName некорректный в части FirstName");
            }

            if (!(String.Equals(cardObjectFirstLine.symbolAfterCardHolderName, cardObjectFirstLine.standartSymbolAfterCardHolderName)))
            {
                errorsFirstLineList.Add("Track 1. Символ после CardHolderName некорректный");
            }

            if (!(String.Equals(cardObjectFirstLine.expDate, cardObjectFirstLine.standartExpDate)))
            {
                errorsFirstLineList.Add("Track 1. ExpDate некорректный");
            }

            if (!(String.Equals(cardObjectFirstLine.controlSum.Substring(0, 3), cardObjectFirstLine.standartPartOfControlSum.Substring(0, 3))))
            {
                errorsFirstLineList.Add("Track 1. ServiceCode некорректный");
            }

            if (!(String.Equals(cardObjectFirstLine.controlSum.Substring(3, 1), cardObjectFirstLine.standartPartOfControlSum.Substring(3, 1))))
            {
                errorsFirstLineList.Add("Track 1. Контрольная цифра после ServiceCode некорректна");
            }

            if (!(String.Equals(cardObjectFirstLine.symbolAfterControlSum, cardObjectFirstLine.standartSymbolAfterControlSum)))
            {
                errorsFirstLineList.Add("Track 1. Символ после контрольной суммы некорректный");
            }
            return errorsFirstLineList;
        }

    }
    

}
