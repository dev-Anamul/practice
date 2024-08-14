using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by   : Tomas
 * Date created : 29.03.2022
 * Modified by  : 
 * Last modified: 
 * Reviewed by  : 
 * Reviewed Date:
 */
namespace Utilities.Constants
{
    /// <summary>
    /// Unique Key for ART patient Locator.
    /// </summary>
    public class UniqueKeyGenerator
    {
        public static string GetFormatedString(string number, int length)
        {
            string format = "";
            for (int i = 0; i < length - number.Length; i++)
            {
                format += "0";
            }
            return format + number;
        }

        public static string GetUniqueNumber(string tokenNumber)
        {
            string nextToken = "SC - " + DateTime.Now.ToString("yyyyMMdd");

            if (tokenNumber == "1" || tokenNumber == null)
            {
                nextToken += GetFormatedString("1", 5);
            }
            else
            {
                string previouNumber = tokenNumber.Substring(tokenNumber.Length - 5);
                int nextNumber = Convert.ToInt32(previouNumber) + 1;
                if (nextNumber >= 100000)
                {
                    nextNumber = 1;
                }
                nextToken += GetFormatedString(nextNumber.ToString(), 5);
            }
            return nextToken;
        }
    }
}
