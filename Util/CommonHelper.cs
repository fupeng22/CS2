using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class CommonHelper
    {
        public static string ParseSwbNeedCheck(string swbNeedCheck)
        {
            string strRet = "";
            switch (swbNeedCheck)
            {
                case "0":
                    strRet = "放行";
                    break;
                case "1":
                    strRet = "等待预检";
                    break;
                case "2":
                    strRet = "查验放行";
                    break;
                case "3":
                    strRet = "查验扣留";
                    break;
                case "4":
                    strRet = "查验待处理";
                    break;
                case "99":
                    strRet = "查验退货";
                    break;
                default:
                    strRet = "未知";
                    break;
            }
            return strRet;
        }

        public static double PerctangleToDecimal(string perc)
        {
            return double.Parse(perc.Replace("%", "")) / 100;
        }
    }
}
