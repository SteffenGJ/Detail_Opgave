namespace Opgave_House4IT;

//Interface for making consistency across structures
interface ICSVStructure
{
    //Parses the values and returns a product
    public Product MapToProduct(List<string> values);
}

//A factory for determining which CSVStructure to use based on the headers from CSV file
class CSVStructureFactory
{
    public static ICSVStructure CreateStructure(List<string> headers)
    {
        if (headers.Count == 7)
        {
            return new CSVStructure1();
        }
        else
        {
            return new CSVStructure2();
        }
    }
}

//A CSVStructure for dealing with the prisliste1.csv file
class CSVStructure1 : ICSVStructure
{
    public int ItemIndex { get; }
    public int UnitIndex { get; }
    public int ArticleDescriptionIndex { get; }
    public int KostprisEURIndex { get; }
    public int PriceUnitIndex { get; }
    public int PriceGroupIndex { get; }
    public int DateOfIssuanceIndex { get; }

    public CSVStructure1()
    {
        ItemIndex = 0;
        ArticleDescriptionIndex = 1;
        UnitIndex = 2;
        KostprisEURIndex = 3;
        PriceUnitIndex = 4;
        PriceGroupIndex = 5;
        DateOfIssuanceIndex = 6;
    }
    public Product MapToProduct(List<string> values)
    {
        Parser parser = new Parser();

        long item = parser.Parse<long>(values[ItemIndex]);
        string description = values[ArticleDescriptionIndex];
        string unit = values[UnitIndex];
        double kostpris = parser.ParsePrice(values[KostprisEURIndex]);
        int priceUnit = parser.Parse<int>(values[PriceUnitIndex]);
        int priceGroup = parser.Parse<int>(values[PriceGroupIndex]);
        string dateOfIssuance = values[DateOfIssuanceIndex];

        return new Product(item, description, unit, kostpris, priceUnit, priceGroup, dateOfIssuance);
    }
}

//A CSVStructure for dealing with the prisliste2.csv file
class CSVStructure2 : ICSVStructure
{
    public int ItemIndex { get; }
    public int UnitIndex { get; }
    public int ArticleDescriptionIndex { get; }
    public int KostprisEURIndex { get; }
    public int PriceUnitIndex { get; }
    public int PriceGroupIndex { get; }

    public CSVStructure2()
    {
        ItemIndex = 1;
        ArticleDescriptionIndex = 0;
        UnitIndex = 2;
        KostprisEURIndex = 5;
        PriceUnitIndex = 4;
        PriceGroupIndex = 3;
    }
    public Product MapToProduct(List<string> values)
    {
        Parser parser = new Parser();

        long item = parser.Parse<long>(values[ItemIndex]);
        string description = values[ArticleDescriptionIndex];
        string unit = values[UnitIndex];
        double kostpris = parser.ParsePrice(values[KostprisEURIndex]);
        int priceUnit = parser.Parse<int>(values[PriceUnitIndex]);
        int priceGroup = parser.Parse<int>(values[PriceGroupIndex]);

        return new Product(item, description, unit, kostpris, priceUnit, priceGroup);
    }
}