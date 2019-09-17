﻿using Newtonsoft.Json;
using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindCore.Services
{
    public class ValidationWebService
    {
        private readonly string validatePhoneNumberApi = "http://apilayer.net/api/validate?access_key=723a66f45e11a23acbcf5a0c71257b38";
        private readonly string validateNotesApi = "https://www.purgomalum.com/service/json";

        internal async Task<string> GetJson(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(url).ConfigureAwait(false);
            }
        }

        public bool ValidatePhoneNumber(Employee employee)
        {
            int phoneNumber = int.Parse(employee.HomePhone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
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

        public string ValidateNotes(string notes)
        {
            string url = $"{validateNotesApi}?text={notes}";
            return JsonConvert.DeserializeObject<ValidationNotes>(GetJson(url).Result).result;
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
    }
}