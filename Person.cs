public enum Gender
{
    Male = 1,
    Female = 2
}

class Person
{
    public Person(string firstName, string lastName, int age, Gender gender)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Gender = gender;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public Gender Gender;
}