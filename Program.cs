using Opgave_House4IT;

GenerateCSVFile();

List<Product> ParseProducts(ICSVStructure cSVStructure, List<List<string>> products)
{
    List<Product> realProducts = [];
    foreach (var item in products)
    {
        try
        {
            realProducts.Add(cSVStructure.MapToProduct(item));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    return realProducts;
}

void GenerateCSVFile()
{
    CSVReader cSVReader = new CSVReader();
    CSVWriter cSVWriter = new CSVWriter();

    CSVStructure1 headersFromFirstFile = new CSVStructure1(cSVReader.GetHeaders("./prisliste1.csv"));
    CSVStructure2 headerFromSecondFile = new CSVStructure2(cSVReader.GetHeaders("./prisliste2.csv"));

    //CSVStructureFactory.CreateStructure(headers);

    List<List<string>> products = cSVReader.GetAllValues("./prisliste1.csv");
    List<List<string>> products2 = cSVReader.GetAllValues("./prisliste2.csv");

    List<Product> parsedProducts = ParseProducts(headersFromFirstFile, products);
    List<Product> realProducts2 = ParseProducts(headerFromSecondFile, products2);

    List<Product> allProducts = [.. parsedProducts, .. realProducts2];

    List<string> csvData = [];

    cSVWriter.SetHeaders("./ny_prisliste.csv", ["Varenummer", "Navn", "Kostpris i danske kroner", "Beregnet salgspris i danske kroner"]);
    foreach (var product in allProducts)
    {
        csvData.Add(product.ToCSVPrice());
    }

    cSVWriter.SetValues("./ny_prisliste.csv", csvData);
}


interface ICSVStructure
{
    public Product MapToProduct(List<string> values);
}

public readonly struct HeaderStruct
{
    public HeaderStruct(string value, int index)
    {
        Value = value;
        Index = index;
    }

    public string Value { get; }
    public int Index { get; }
}

class CSVStructureFactory
{
    public static ICSVStructure CreateStructure(List<string> headers)
    {
        if (headers.Count == 7)
        {
            return new CSVStructure1(headers);
        }
        else
        {
            return new CSVStructure2(headers);
        }
    }
}
class CSVStructure1 : ICSVStructure
{
    public HeaderStruct Item { get; }
    public HeaderStruct Unit { get; }
    public HeaderStruct ArticleDescription { get; }
    public HeaderStruct KostprisEUR { get; }
    public HeaderStruct PriceUnit { get; }
    public HeaderStruct PriceGroup { get; }
    public HeaderStruct DateOfIssuance { get; }

    public CSVStructure1(List<string> headers)
    {
        Item = new HeaderStruct(headers[0], 0);
        ArticleDescription = new HeaderStruct(headers[1], 1);
        Unit = new HeaderStruct(headers[2], 2);
        KostprisEUR = new HeaderStruct(headers[3], 3);
        PriceUnit = new HeaderStruct(headers[4], 4);
        PriceGroup = new HeaderStruct(headers[5], 5);
        DateOfIssuance = new HeaderStruct(headers[6], 6);
    }
    public Product MapToProduct(List<string> values)
    {
        Parser parser = new Parser();

        long item = parser.Parse<long>(values[Item.Index]);
        string description = values[ArticleDescription.Index];
        string unit = values[Unit.Index];
        double kostpris = parser.ParsePrice(values[KostprisEUR.Index]);
        int priceUnit = parser.Parse<int>(values[PriceUnit.Index]);
        int priceGroup = parser.Parse<int>(values[PriceGroup.Index]);
        string dateOfIssuance = values[DateOfIssuance.Index];

        return new Product(item, description, unit, kostpris, priceUnit, priceGroup, dateOfIssuance);
    }
}

class CSVStructure2 : ICSVStructure
{
    public HeaderStruct Item { get; }
    public HeaderStruct Unit { get; }
    public HeaderStruct ArticleDescription { get; }
    public HeaderStruct KostprisEUR { get; }
    public HeaderStruct PriceUnit { get; }
    public HeaderStruct PriceGroup { get; }

    public CSVStructure2(List<string> headers)
    {
        Item = new HeaderStruct(headers[1], 1);
        ArticleDescription = new HeaderStruct(headers[0], 0);
        Unit = new HeaderStruct(headers[2], 2);
        KostprisEUR = new HeaderStruct(headers[5], 5);
        PriceUnit = new HeaderStruct(headers[4], 4);
        PriceGroup = new HeaderStruct(headers[3], 3);
    }
    public Product MapToProduct(List<string> values)
    {
        Parser parser = new Parser();

        long item = parser.Parse<long>(values[Item.Index]);
        string description = values[ArticleDescription.Index];
        string unit = values[Unit.Index];
        double kostpris = parser.ParsePrice(values[KostprisEUR.Index]);
        int priceUnit = parser.Parse<int>(values[PriceUnit.Index]);
        int priceGroup = parser.Parse<int>(values[PriceGroup.Index]);

        return new Product(item, description, unit, kostpris, priceUnit, priceGroup);
    }
}