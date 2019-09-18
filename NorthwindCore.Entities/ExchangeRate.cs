using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindCore.Entities
{
    public class ExchangeRate
    {
        private double rate;
        private string currency;

        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        public double Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        public ExchangeRate(string currency, double rate)
        {
            Currency = currency;
            Rate = rate;
        }
    }
}
