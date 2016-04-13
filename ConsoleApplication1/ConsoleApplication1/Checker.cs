using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagStripeTest
{
    class Check
    {
        /// <summary>
        /// введен ли номер с лицевой части магнитной карты
        /// </summary>
        /// <param name="faceNumber"></param>
        /// <returns></returns>
        public static bool CheckIsFaceNumberEntered(string faceNumber)
        {
            if (String.IsNullOrWhiteSpace(faceNumber))
            {
                return false; //номер с лицевой стороны карты не введён
            }
            else
            {
                return true; //номер с лицевой стороны карты введён 
            }
        } 

        /// <summary>
        /// определение, к какой именно дорожке относятся введённые данные (учитываютя символы до начала PAN и длина)
        /// </summary>
        /// <param name="anyMagEnter"></param>
        /// <returns></returns>
        public static int CheckWhatLineIsIt(string anyMagEnter)

        {
            if ((anyMagEnter.Length >= 2) && (String.Equals(anyMagEnter.Substring(0, 2), "%B")) && (anyMagEnter.Length == 50))
            {
                return 1; // введена первая дорожка
            }
            else
            {
                if ((anyMagEnter.Length >= 2) && (String.Equals(anyMagEnter.Substring(0, 1), ";")) && (anyMagEnter.Length == 35))
                {
                    return 2; // введена вторая дорожка
                }
                else
                {
                    return 3; // введена некорректная дорожка
                }
            }
        }
    }
}
