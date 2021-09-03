using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Enumeration
{
    public class Enumer
    {
        public  static string getGenderName(int? gender)
        {
            switch (gender)
            {
                case (int)_GenderEnum.Female: return "Nữ";
                case (int)_GenderEnum.Male: return "Nam";
                case (int)_GenderEnum.Other: return "Khác";
                default: return "Khác";
            }
        }

        private enum _GenderEnum 
        {
            Female = 0,
            Male = 1,
            Other= 2
        }
    }
}
