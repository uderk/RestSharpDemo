using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHubApiTests
{
    public class GitHubApiTests
    {
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient("https://api.github.com");
            string url = "/repos/uderk/postman/issues";
            this.request = new RestRequest(url);
            this.client.Authenticator = new HttpBasicAuthenticator("uderk", "g******************lUi");
        }

        [Test]
        public async Task Test_Get_Issues()
        {
            var response = await client.ExecuteAsync(request);
            Assert.IsNotNull(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async  Task Test_GitHubAllIssues()
        {
            var response = await this.client.ExecuteAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var issues = JsonSerializer.Deserialize<System.Collections.Generic.List<Issue>>(response.Content);

            foreach (var issue in issues)
            {
                Assert.Greater(issue.id, 0);
                Assert.Greater(issue.number, 0);
                Assert.IsNotNull(issue.number);
                Assert.IsNotEmpty(issue.title);
            }
        }

     

        [Test]
        public async Task Test_Create_GitHubIssueAssync()
        {
            string title = "New Issue from RestSharp Test Nunit Project";
            string body = "Some body here";
            var issue = await CreateIssue(title, body);
            Assert.Greater(issue.id, 0);
            Assert.Greater(issue.number, 0);
            Assert.IsNotEmpty(issue.title);
        }
        private async Task<Issue> CreateIssue(string title, string body)
        {
            var request = new RestRequest("repos/uderk/postman/issues");
            request.AddBody(new { body, title });
            var response = await this.client.ExecuteAsync(request, Method.Post);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;
        }
    }
}
