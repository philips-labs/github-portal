using System;
using System.Threading.Tasks;
using Octokit;

namespace Crawler
{
    public class Program
    {
        
        static async Task Main(string[] args)
        {
            try
            {
                Crawler crawler = new Crawler(new GitHubClient(new ProductHeaderValue("Github-Portal-Crawler")));
                await crawler.StartCrawler();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
