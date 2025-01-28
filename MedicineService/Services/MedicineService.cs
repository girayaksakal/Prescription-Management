using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class MedicineService
{
    private readonly MedicineFileFetcher _fileFetcher;
    private readonly string _filePath;
    private List<string> _medicineNames;

    public MedicineService(MedicineFileFetcher fileFetcher)
    {
        _fileFetcher = fileFetcher;
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "medicine_list.xlsx");
        _medicineNames = new List<string>();
    }

    // Refreshes the medicine list by downloading and parsing the latest file
    public async Task RefreshMedicineDataAsync()
    {
        await _fileFetcher.DownloadLatestFileAsync(_filePath);
        _medicineNames = ParseExcelFile();
    }

    // Parses the Excel file and extracts medicine names
    public List<string> ParseExcelFile()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException("Medicine Excel file not found.");
        }

        var medicineNames = new List<string>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage(new FileInfo(_filePath));
        var worksheet = package.Workbook.Worksheets[0]; // Assume the first worksheet contains the data

        for (int row = 2; row <= worksheet.Dimension.Rows; row++) // Skip header row
        {
            var medicineName = worksheet.Cells[row, 1].Text; // Adjust column index as necessary
            if (!string.IsNullOrEmpty(medicineName))
            {
                medicineNames.Add(medicineName);
            }
        }

        return medicineNames;
    }

    // Searches for medicines matching a term
    public List<string> SearchMedicines(string term)
    {
        return _medicineNames.FindAll(name => name.StartsWith(term, System.StringComparison.OrdinalIgnoreCase));
    }
}