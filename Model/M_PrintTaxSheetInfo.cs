using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class M_PrintTaxSheetInfo
    {
        public Int32 cId
        {
            get;
            set;
        }

        public DateTime PrintDate
        {
            get;
            set;
        }

        public Int32 OrderNumber
        {
            get;
            set;
        }

        public string Operator
        {
            get;
            set;
        }

        public string mMemo
        {
            get;
            set;
        }
    }
}
