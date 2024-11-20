using Opgave_House4IT;

List<string> filesToRead = ["./resources/prisliste1.csv", "./resources/prisliste2.csv"];
string newFile = "./ny_prisliste.csv";
List<string> newHeaders = ["Varenummer", "Navn", "Kostpris i danske kroner", "Beregnet salgspris i danske kroner"];

GenerateCSVFile(filesToRead, newFile, newHeaders);

//Gets all the items from all the files specified, and saves them all to a new file
void GenerateCSVFile(List<string> paths, string newFilePath, List<string> newHeaders) {
    List<Product> products = [];
    foreach (var path in paths)
    {
        products.AddRange(ProductsFromCSVFile(path));
    }
    SaveToCSVFile(newFilePath, newHeaders, products);
}

//Saves the products provided to a new file at the specified filepath with the specified header values
void SaveToCSVFile(string newFilePath, List<string> newHeaders, List<Product> products) {
    CSVWriter csvWriter = new CSVWriter();
    List<string> csvData = [];

    csvWriter.SetHeaders(newFilePath, newHeaders);
    foreach (var product in products)
    {
        //Adds the stringified product to csvData
        csvData.Add(product.ToCSVPrice());
    }

    csvWriter.SetValues(newFilePath, csvData);
}

//Gets all lines from the file and returns them as a Product
List<Product> ProductsFromCSVFile(string path) {
    CSVReader csvReader = new CSVReader();
    //Makes a CSVStructure based on the headers of the file
    ICSVStructure csvStructure = CSVStructureFactory.CreateStructure(csvReader.GetHeaders(path));

    return ParseProducts(csvStructure, csvReader.GetAllValues(path));
}

//Turns every unformatted product into a Product
List<Product> ParseProducts(ICSVStructure cSVStructure, List<List<string>> values)
{
    List<Product> products = [];
    foreach (var item in values)
    {
        try
        {
            //Tries to add the Product to the list
            products.Add(cSVStructure.MapToProduct(item));
        }
        catch (Exception e)
        { 
            //Writes the exception if any to the Console
            Console.WriteLine(e);
        }
    }
    return products;
}