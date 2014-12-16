using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class M_SubWayBillDetail
    {
        #region

        public int swbdID
        {
            get;
            set;
        }

        public int swbd_swbID
        {
            get;
            set;
        }

        private string _swbDescription_CHN;

        public string SwbDescription_CHN
        {
            get { return _swbDescription_CHN; }
            set { _swbDescription_CHN = value; }
        }
        private string _swbDescription_ENG;

        public string SwbDescription_ENG
        {
            get { return _swbDescription_ENG; }
            set { _swbDescription_ENG = value; }
        }
        private int _swbNumber;

        public int SwbNumber
        {
            get { return _swbNumber; }
            set { _swbNumber = value; }
        }
        private double _swbWeight;

        public double SwbWeight
        {
            get { return _swbWeight; }
            set { _swbWeight = value; }
        }
        private double _swbActualWeight;

        public double SwbActualWeight
        {
            get { return _swbActualWeight; }
            set { _swbActualWeight = value; }
        }
        private DateTime _swbSortingTime;

        public DateTime SwbSortingTime
        {
            get { return _swbSortingTime; }
            set { _swbSortingTime = value; }
        }
        private int _swbNeedCheck;

        public int SwbNeedCheck
        {
            get { return _swbNeedCheck; }
            set { _swbNeedCheck = value; }
        }
        private string _swbImgeLocalPath;

        public string SwbImgeLocalPath
        {
            get { return _swbImgeLocalPath; }
            set { _swbImgeLocalPath = value; }
        }
        private int _swbDelFlag;

        public int SwbDelFlag
        {
            get { return _swbDelFlag; }
            set { _swbDelFlag = value; }
        }

        private double _swbValue;

        public double SwbValue
        {
            get { return _swbValue; }
            set { _swbValue = value; }
        }
        private string _swbMonetary;

        public string SwbMonetary
        {
            get { return _swbMonetary; }
            set { _swbMonetary = value; }
        }
        private string _swbRecipients;

        public string SwbRecipients
        {
            get { return _swbRecipients; }
            set { _swbRecipients = value; }
        }

        private string _swbCustomsCategory;

        public string SwbCustomsCategory
        {
            get { return _swbCustomsCategory; }
            set { _swbCustomsCategory = value; }
        }

        public string swbValueDetail
        {
            get;
            set;
        }

        public DateTime DetainDate
        {
            get;
            set;
        }

        public DateTime ReleaseDate
        {
            get;
            set;
        }

        public DateTime InHandleDate
        {
            get;
            set;
        }

        public int swbActualNumber
        {
            get;
            set;
        }

        public string TaxNo
        {
            get;
            set;
        }

        public double TaxRate
        {
            get;
            set;
        }

        public double ActualTaxRate
        {
            get;
            set;
        }

        public string CategoryNo
        {
            get;
            set;
        }

        public int mismatchCargoName
        {
            get;
            set;
        }

        public int belowFullPrice
        {
            get;
            set;
        }

        public int above1000
        {
            get;
            set;
        }

        public int CheckResult
        {
            get;
            set;
        }

        public int HandleSuggestion
        {
            get;
            set;
        }

        public string CheckResultDescription
        {
            get;
            set;
        }

        public string HandleSuggestionDescription
        {
            get;
            set;
        }

        public string CheckResultOperator
        {
            get;
            set;
        }

        public int IsConfirmCheck
        {
            get;
            set;
        }

        public string ConfirmCheckOperator
        {
            get;
            set;
        }

        public double TaxValue
        {
            get;
            set;
        }

        #endregion
    }
}
