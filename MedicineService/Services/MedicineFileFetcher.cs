using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class MedicineFileFetcher
{
    private const string BaseUrl = "https://www.titck.gov.tr/dinamikmodul/43";

    // Downloads the latest file to the specified path
    public async Task DownloadLatestFileAsync(string filePath)
    {
        var latestFileUrl = await GetLatestFileLinkAsync();

        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(latestFileUrl);
        response.EnsureSuccessStatusCode();

        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await response.Content.CopyToAsync(fileStream);

        Console.WriteLine($"Downloaded latest file from {latestFileUrl}");
    }

    // Scrapes the webpage and gets the latest file link
    public async Task<string> GetLatestFileLinkAsync()
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(BaseUrl);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(response);

        // Select the first table row containing a link
        var rows = htmlDocument.DocumentNode.SelectNodes("//table/tbody/tr");
        //var rows = htmlDocument.DocumentNode.SelectNodes("//div[@class='table table-striped dataTable no-footer']");

        if (rows == null || rows.Count == 0)
        {
            throw new Exception("No table rows found on the webpage.");
        }

        var latestRow = rows.First();
        var linkNode = latestRow.SelectSingleNode(".//a[@href]");

        if (linkNode == null)
        {
            throw new Exception("No link found in the latest table row.");
        }

        var latestFileLink = linkNode.GetAttributeValue("href", null);
        if (string.IsNullOrEmpty(latestFileLink))
        {
            throw new Exception("Failed to extract the latest file link.");
        }

        // Construct the full URL if the link is relative
        if (!latestFileLink.StartsWith("http"))
        {
            latestFileLink = new Uri(new Uri(BaseUrl), latestFileLink).ToString();
        }

        return latestFileLink;
    }
}