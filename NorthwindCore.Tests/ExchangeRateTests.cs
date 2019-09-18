using NorthwindCore.Entities;
using NorthwindCore.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NorthwindCore.Tests
{
    public class ExchangeRateTests
    {
        [Theory]
        [InlineData("https://openexchangerates.org/api/latest.json?app=59999de6333c428ebcd5071ba5883aa1&base=USD")]
        [InlineData("https://openexchangerates.org/api/latest.json?app_id=59999de6333c428ebcd5071ba5883aa1&base=USD")]
        [InlineData("https://openexchangerates.org/api/latest.json?app_id=59999de633&base=USD")]
        public void ExceptionTest(string url)
        {
            ValidationWebService validationWebService = new ValidationWebService();
            Assert.Throws<AggregateException>(() => validationWebService.GetRates(url));
        }

        [Theory]
        [InlineData("USD", 13.5)]
        [InlineData("DKK", 350.67)]
        public void ExchangeRateTest(string currency, double rate)
        {
            ExchangeRate exchangeRate = new ExchangeRate(currency, rate);
            Assert.True(exchangeRate.Currency == currency && exchangeRate.Rate == rate);
        }
    }
}
