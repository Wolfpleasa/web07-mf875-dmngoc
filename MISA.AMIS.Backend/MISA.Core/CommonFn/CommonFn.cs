using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.Core.CommonFunction
{
    public class CommonFn
    {
        /// <summary>
        /// Hàm chuyển đổi ngày tháng(nếu có) về dd/MM/yyyy
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns>Date</returns>
        /// Created By: Ngọc 02/09/2021
        public static DateTime? ConvertToYMD(string dateString)
        {   
            var str = dateString.Replace('-', '/').Split("/");
            switch (str.Length)
            {
                case 1:
                    return DateTime.Parse($"{str[0]}/01/01");
                case 2:
                    return DateTime.Parse($"{str[1]}/{str[0]}/01");
                case 3:
                    return DateTime.Parse($"{str[2]}/{str[1]}/{str[0]}");
                default:
                    return null;
            }
        }
    }
}
