namespace Opgave_House4IT;

//A class for reading a CSV file
class CSVReader
{
    //Reads
    public List<List<string>> GetAllValues(string filepath)
    {
        //Skips the first line of the CSV file and reads the rest
        List<string> allLinesFromFile = File.ReadLines(filepath).Skip(1).ToList();
        List<List<string>> allItems = [];


        foreach (var line in allLinesFromFile)
        {
            //Turns the line into a list of strings seperated at ;
            List<string> singleLine = [.. line.Split(";")];

            allItems.Add(singleLine);
        }
        return allItems;
    }

    public List<string> GetHeaders(string filepath)
    {
        //Returns the first line of the file, split into a list of strings seperated at ;
        string firstLine = File.ReadLines(filepath).First();
        return [.. firstLine.Split(";")];
    }
}

class CSVWriter
{
    public void SetHeaders(string filepath, List<string> headers)
    {
        //Writes to the top of the file at the specified filepath, a string joined from a list of header values
        File.WriteAllLines(filepath, [string.Join(";", headers)]);
    }

    public void SetValues(string filepath, List<string> values)
    {
        //Appends the specified values to the file
        File.AppendAllLines(filepath, values);
    }

}