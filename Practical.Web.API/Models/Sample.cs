namespace Practical.Web.API.Models
{

    //This Model will use to demonstrate Limited to Built-in Sources
    public class CustomObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
    }

    //This Model will use to demonstrate Lacks Flexibility
    public class ComplexBodyModel
    {
        public string Data { get; set; }
    }

    //This Model will use to demonstrate Lacks Flexibility
    public class MergedModel
    {
        public string Header { get; set; }
        public string Query { get; set; }
        public string BodyData { get; set; }
    }

    //This Model will use to demonstrate No Support for Special Data Types
    public class CustomTupleModel
    {
        public int Item1 { get; set; }
        public string Item2 { get; set; }
    }

    //This Model will use to demonstrate Performance Issues for Large Data
    public class LargeDataModel
    {
        public List<string> LargeDataList { get; set; }
    }
}
