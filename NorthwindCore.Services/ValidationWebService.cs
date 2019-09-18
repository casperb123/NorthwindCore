using Newtonsoft.Json;
using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindCore.Services
{
    public class ValidationWebService
    {
        private readonly string validatePhoneNumberApi = "http://apilayer.net/api/validate?access_key=723a66f45e11a23acbcf5a0c71257b38";
        private readonly string validateNotesApi = "https://www.purgomalum.com/service/json";
        private readonly string openExchangeRatesApi = "https://openexchangerates.org/api/latest.json?app_id=59999de6333c428ebcd5071ba5883aa1&base=USD";

        internal async Task<string> GetJson(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(url).ConfigureAwait(false);
            }
        }

        public bool ValidatePhoneNumber(Employee employee)
        {
            string phoneNumber = employee.HomePhone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            string countryCode = string.Empty;

            switch (employee.Country)
            {
                case "USA":
                    countryCode = "US";
                    break;
                case "UK":
                    countryCode = "GB";
                    break;
                default:
                    break;
            }

            string url = $"{validatePhoneNumberApi}&number={phoneNumber}&country_code={countryCode}&format=1";
            return JsonConvert.DeserializeObject<ValidationPhoneNumber>(GetJson(url).Result).valid;
        }

        public (bool isValid, List<string> errors) ValidateNotes(string notes)
        {
            string url = $"{validateNotesApi}?text={notes}";
            //return JsonConvert.DeserializeObject<ValidationNotes>(GetJson(url).Result).result;
            ValidationNotes validationNotes = JsonConvert.DeserializeObject<ValidationNotes>(GetJson(url).Result);

            List<string> errors = new List<string>();
            string[] input = notes.Split(' ');
            string[] output = validationNotes.result.Split(' ');
            for (int i = 0; i < input.Length; i++)
            {
                string dif = string.Join("", input[i].ToArray().Where(item => !output[i].Contains(item.ToString())).ToArray());
                if (dif.Length > 0)
                {
                    errors.Add(dif);
                }
            }
            if (errors.Count == 0)
            {
                return (true, errors);
            }
            return (false, errors);
        }

        public ICollection<ExchangeRate> GetRates(string url = null)
        {
            string jsonUrl = openExchangeRatesApi;
            if (url != null)
            {
                jsonUrl = url;
            }

            OpenExchange openExchange = JsonConvert.DeserializeObject<OpenExchange>(GetJson(jsonUrl).Result);
            if (openExchange.error)
            {
                throw new InvalidOperationException(openExchange.message);
            }

            ICollection<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            foreach (KeyValuePair<string, double> rate in openExchange.rates)
            {
                ExchangeRate exchangeRate = new ExchangeRate(rate.Key, rate.Value);
                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates;
        }

        public class ValidationPhoneNumber
        {
            public bool valid { get; set; }
            public string number { get; set; }
            public string local_format { get; set; }
            public string international_format { get; set; }
            public string country_prefix { get; set; }
            public string country_code { get; set; }
            public string country_name { get; set; }
            public string location { get; set; }
            public string carrier { get; set; }
            public string line_type { get; set; }
        }


        public class ValidationNotes
        {
            public string result { get; set; }
        }

        public class OpenExchange
        {
            public bool error { get; set; }
            public string message { get; set; }
            public Dictionary<string, double> rates { get; set; }
        }
    }
}
