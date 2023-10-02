using System.ComponentModel.DataAnnotations;
using System.Text;

namespace testDB
{
    public class Employee
    {
        [Key]
        public long Id { get; private set; }
        public string LastName { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Sex Sex { get; private set; }
        public DateTime BirthDate { get; set; }


        public Employee(string lastName, string name, string surname)
        {
            Name = name;
            LastName = lastName;
            Surname = surname;
        }

        public void SendToDatabase()
        {
            if (Program.Context != null)
            {
                Program.Context.Employees.Add(this);
                Program.Context.SaveChanges();
            }
        }

        public string GetEmployeeStringWithAge()
        {
            StringBuilder sb = new (LastName + " " + Name + " " + Surname + " ");
            string date = BirthDate.Year.ToString() + "-" 
                + BirthDate.Month.ToString() +"-"+ BirthDate.Day.ToString();
            sb.Append(date + " ");
            sb.Append(Sex.ToString() + " ");
            sb.Append("age: " + CalculateAge().ToString());
            return sb.ToString();
        }

        public bool CompareLastName(char cr) => LastName[0] == cr;

        public void SetSex(Sex sex) => Sex = sex;

        int CalculateAge()
        {
            int age = DateTime.Today.Year - BirthDate.Year;
            if (BirthDate.AddYears(age) > DateTime.Today) age--;

            return age;
        }
    }
}
