using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace SpecFlowAPITestDemo.Support
{
    public class CommonMethods
    {
        public static string GetAppSettingsValue(string key)
        {
            return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json").Build().GetSection(key).Value;
        }

        public RestClient  CreateApiClient() 
        { 
            var client = new RestClient(GetAppSettingsValue("ApiEndpoint"));
            var token = GetAppSettingsValue("token");
            client.AddDefaultHeader("Authorization", token);
            return client;
        }

        public string RunPostApiAndGetResponseAsString(string fileName) 
        {
            var jsonFilePath = $"./TestData/{fileName}";
            // Read the JSON file content as a string
            string payload = File.ReadAllText(jsonFilePath);

            var apiClient = CreateApiClient();
            var request = new RestRequest("",Method.Post);
            // Add the JSON content as the body of the request
            request.AddStringBody(payload, DataFormat.Json);
            var response = apiClient.Execute(request);
            var code = (int) response.StatusCode;
            switch(code) 
            {
                case 200: 
                    { 
                        var stringBody = response.Content;
                        return stringBody;
                    }
                default:
                    throw new Exception($"somethin is wrong, api returns {code}, {response.ErrorMessage}");
            }
        }

        public bool DoesExpectedResponseMatchesActual(string actualResponse, string expectedResponseFileName)
        {
            var jsonFilePath = $"./TestData/{expectedResponseFileName}";
            // Read the JSON file content as a string
            string expectedResponse = File.ReadAllText(jsonFilePath);
            Assert.AreEqual(expectedResponse, actualResponse);
            return true;
        }

        public ResponseObject RunPostApiAndGetResponseAsObject(string fileName)
        {
            var jsonFilePath = $"./TestData/{fileName}";
            // Read the JSON file content as a string
            string payload = File.ReadAllText(jsonFilePath);

            var apiClient = CreateApiClient();
            var request = new RestRequest("", Method.Post);
            // Add the JSON content as the body of the request
            request.AddStringBody(payload, DataFormat.Json);
            var response = apiClient.Execute(request);
            var code = (int)response.StatusCode;
            switch (code)
            {
                case 200:
                    {
                        var stringBody = JsonConvert.DeserializeObject<ResponseObject>(response.Content);
                        return stringBody;
                    }
                default:
                    throw new Exception($"somethin is wrong, api returns {code}, {response.ErrorMessage}");
            }
        }

        public ResponseObject GetExpectedResponseObject(string expectedResponseFileName)
        {
            var jsonFilePath = $"./TestData/{expectedResponseFileName}";
            // Read the JSON file content as a string
            string expectedResponseInString = File.ReadAllText(jsonFilePath);
            var stringBody = JsonConvert.DeserializeObject<ResponseObject>(expectedResponseInString);
            return stringBody;
        }

        public static List<string> CompareObjectsAndGetDifferences<T>(List<T> list1, List<T> list2)
        {
            var differences = new List<string>();

            // Check if lists are of equal length
            if (list1.Count != list2.Count)
            {
                differences.Add("The lists have different sizes.");
                return differences;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                var obj1 = list1[i];
                var obj2 = list2[i];

                // Use reflection to get properties of the objects
                PropertyInfo[] properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    var value1 = property.GetValue(obj1);
                    var value2 = property.GetValue(obj2);

                    // Compare values of each property
                    if (!Equals(value1, value2))
                    {
                        differences.Add($"Difference in item {i + 1} at property '{property.Name}': List1 = '{value1}', List2 = '{value2}'");
                    }
                }
            }

            if (!differences.Any())
            {
                differences.Add("The lists are identical.");
            }

            return differences;
        }
    }
}
