using System.Text;

namespace testDB
{
    internal static class Filler
    {
        static DateTime start = new DateTime(1950, 1, 1);
        static DateTime end = new DateTime(2005, 1, 1);
        static int range;
        static Random random = new Random();
        static char[] capitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static char[] wovelsLetters = "aeiouy".ToCharArray();
        static char[] constantsLetters = "bcdfghjklmnpqrstvwxz".ToCharArray();
        static string[] lastNameEnd = new string[] { "ov", "ev","in" };
        static string[] maleNames = new string[] 
        { "Andrey", "Kirill", "Oleg", "Sergey", "Stas","Vladislav","Fedor","Anton",
          "German","Alexandr","Marat", "Petr" };
        static string[] femaleNames = new string[]
            {"Valentina","Inna","Yulia","Regina","Maria","Natalia","Olga","Anastasia",
            "Ksenya","Evgenia","Lolita","Oksana"};

        public static void RandomTableFilling(int count, bool male = false, char? letter = null)
        {
            Sex sex;
            List<Employee> employees = new ();
            range = (end - start).Days;

            for (int i = 0; i < count; i++) 
            {
                if (!male)
                {
                    if (BoolRandom()) sex = Sex.Male;
                    else sex = Sex.Female;
                }

                else sex = Sex.Male;

                string lastName = GenerateRandomLastName(sex,letter);
                string name = GenerateRandomName(sex);
                string surname = GenerateRandomSurname(sex);

                Employee employee = new(lastName, name, surname)
                {
                    BirthDate = GetRandomBirthDate()
                };
                employee.SetSex(sex);
                employees.Add(employee);
            }

            UpdateDB(employees);
        }

        static string GenerateRandomLastName(Sex sex, char? letter = null)
        {
            int lastNameCount = random.Next(2,14);
            StringBuilder sb = new StringBuilder();

            for(int i = 0;i < lastNameCount;i++) 
            {
                if (i == 0)
                {
                    if (letter == null)
                    {
                        sb.Append(capitalLetters[random.Next(capitalLetters.Length - 1)]);
                    }
                    else
                    {
                        sb.Append(letter);
                    }
                }
                else
                {
                    if (BoolRandom())
                    {
                        sb.Append(wovelsLetters[random.Next(wovelsLetters.Length - 1)]);
                    }
                    else
                    {
                        sb.Append(constantsLetters[random.Next(constantsLetters.Length - 1)]);
                    }
                }
            }
            sb.Append(lastNameEnd[random.Next(lastNameEnd.Length - 1)]);
            if (sex == Sex.Female) sb.Append("a");
            return sb.ToString();
        }

        static string GenerateRandomName(Sex sex)
        {
            if (sex == Sex.Male) return maleNames[random.Next(maleNames.Length)];
            else return femaleNames[random.Next(femaleNames.Length)];
        }

        static string GenerateRandomSurname(Sex sex)
        {
            string startSurname = maleNames[random.Next(maleNames.Length)];
            StringBuilder sb = new StringBuilder(startSurname);
            if (wovelsLetters.Contains(startSurname[startSurname.Length - 1]))
            {
                sb.Remove(startSurname.Length - 1 , 1);
                if(sex == Sex.Male) sb.Append("evich");
                else sb.Append("enva");
                return sb.ToString();
            }
            else
            {
                if (sex == Sex.Male) sb.Append("ovich");
                else sb.Append("ovna");
                return sb.ToString();
            }
        }

        static void UpdateDB(List<Employee> employees)
        {
            if(Program.Context != null)
            {
                Program.Context.Employees.AddRange(employees);
                Program.Context.SaveChanges();
            }
        }

        static DateTime GetRandomBirthDate() => start.AddDays(random.Next(range));

        static bool BoolRandom() => random.Next(2) == 1;
    }
}
