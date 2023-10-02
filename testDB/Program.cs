using System.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace testDB
{
    internal class Program
    {
        static AppMode appMode;
        static int inputCount = 5;
        static int randomCount = 999900;
        public static string? ConnectionString { get; private set; }
        public static MyDbContext? Context { get;private set; }

        static void Main(string[] args)
        {
            if (args.Length == 0) Console.WriteLine("Please set options");
            else TryToSetAppMode(args[0]);
            if(ConnectionString == null) GenerateContext();
            AppBehavior();
        }

        static void TryToSetAppMode(string message)
        {
            try
            {
                int mode = int.Parse(message);

                if (mode < 6) appMode = (AppMode)mode;
                else Console.WriteLine("Set the correct mode");
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void AppBehavior()
        {
            switch (appMode)
            {
                case AppMode.Create:
                    GenerateContext();
                    break;
                case AppMode.Update:
                    SetEmployee();
                    break;
                case AppMode.GetAll:
                    GetAllEmployees();
                    break;
                case AppMode.Autocomplete:
                    RandomFill();
                    break;
                case AppMode.Result:
                    SelectEmployees();
                    break;
            }
        }

        static void GenerateContext()
        {
            if (ConnectionString == null)
            {
                ConnectionString = 
                    ConfigurationManager.ConnectionStrings["Employees"].ConnectionString;
                Context = new();
            }
        }

        static void SetEmployee()
        {
            string? userInput = Console.ReadLine();
            if (userInput != null)
            {
                List<char> charsToRemove = new List<char>() { '@', '_', ',', '.','"' };
                string cleanInput = userInput.Filter(charsToRemove);
                string noSpace = cleanInput.Trim();
                string[] data = noSpace.Split(" ");
                if (data.Length >= inputCount)
                {
                    Employee employee = new(data[0], data[1], data[2]);
                    employee.BirthDate = DateTime.ParseExact(data[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (Enum.TryParse(data[4], out Sex sex))
                    {
                        employee.SetSex(sex);
                        employee.SendToDatabase();
                    }
                    else SendExeption();
                }
                else
                {
                    SendExeption();
                }
            }
        }

        static void GetAllEmployees()
        {
            if (Context != null)
            {
                var employees = GetSortEmployees(Context.Employees);

                if (employees != null)
                {
                    foreach (var employee in employees)
                    {
                        Console.WriteLine(employee.GetEmployeeStringWithAge());
                    }
                }
            }
        }

        static IOrderedEnumerable<Employee> GetSortEmployees(IEnumerable<Employee> employees)
        {
            return employees.OrderBy(employee => employee.LastName)
                                    .OrderBy(employee => employee.Name)
                                    .OrderBy(employee => employee.Surname);
        }

        static void RandomFill()
        {
            Filler.RandomTableFilling(randomCount);
            Filler.RandomTableFilling(100, true, 'F');
        }

        static void SelectEmployees()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if(Context != null)
            {
                var result = Context.Employees
                    .Where(x => x.Sex == Sex.Male).ToList();
                var test = result.Where(x => x.CompareLastName('F') == true);
                var sortResult = GetSortEmployees(test);

                if (sortResult != null)
                {
                    foreach (var employee in sortResult)
                    {
                        Console.WriteLine(employee.GetEmployeeStringWithAge());
                    }
                }
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);            
        }

        static void SendExeption() => Console.WriteLine("Set the correct format");
    }

    public enum AppMode 
    {   
        Create = 1,
        Update = 2,
        GetAll = 3,
        Autocomplete = 4, 
        Result = 5
    }

    public enum Sex
    {
        Male,
        Female
    }
}