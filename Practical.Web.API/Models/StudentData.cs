namespace Practical.Web.API.Models
{
    public class StudentData
    {
        public static List<Student> Students = new List<Student>
        {
            new Student { Id = 1, Name = "Alice Johnson", Gender = "Female", Department = "HR", City = "New York" },

            new Student { Id = 2, Name = "Bob Smith", Gender = "Male", Department = "IT", City = "Los Angeles" },

            new Student { Id = 3, Name = "Charlie Davis", Gender = "Male", Department = "Finance", City = "Chicago" },

            new Student { Id = 4, Name = "Sara Taylor", Gender = "Female", Department = "HR", City = "Los Angeles" },

            new Student { Id = 5, Name = "James Smith", Gender = "Male", Department = "IT", City = "Chicago" },

        };
    }
}
