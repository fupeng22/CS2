using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class M_TaxRateSetting
    {
        public Int32 trsID
        {
            get;
            set;
        }

        public string TaxNo
        {
            get;
            set;
        }

        public string ParentTaxNo
        {
            get;
            set;
        }

        public string CargoName
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        public string FullValue
        {
            get;
            set;
        }

        public double TaxRate
        {
            get;
            set;
        }

        public int isLeaf
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
