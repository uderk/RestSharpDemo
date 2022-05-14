using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RestSharpDemo
{

   public  class RestSharpClass
    {
        static void Main()
        {
            TestAsync();

            Console.ReadKey();
        }
        public static async void TestAsync()
        {
            
            var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/users/uderk/repos");
            client.Authenticator = new HttpBasicAuthenticator("uderk", "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");

            var request2 = new RestRequest("repos/uderk/postman/issues");
            var response2 = await client.ExecuteAsync(request2);
            var response = await client.ExecuteAsync(request);
            var repos = JsonSerializer.Deserialize<List<Repo>>(response.get_Content());
            string url = "/repos/uderk/postman/issues";

            var postRequest = new RestRequest(url);
            postRequest.AddBody(new { title = "New issue from RestSharp project" });
            var postResponse = await client.ExecuteAsync(postRequest, Method.Post);
            
            Console.WriteLine("Status code" + postResponse.get_StatusCode());
            Console.WriteLine("Body content"+ postResponse.get_Content());
            

            var issues = JsonSerializer.Deserialize<List<Issue>>(response2.get_Content());

            foreach (var issue in issues)
            {
                Console.WriteLine("*****************");
                Console.WriteLine("Issue number : "+issue.number);
                Console.WriteLine("Issue ID: " + issue.id);
                Console.WriteLine("*******************");
            }

            foreach (var repo in repos)
            {
                Console.WriteLine("Fullname "+repo.full_name);
            }

        }
    }

    internal class Issue
    {
        public int number { get; set; }
        public int id { get; set; }
    }

    public class Repo
    {
        public string full_name { get; set; }
    }
}



