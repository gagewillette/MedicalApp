
public class Patient 
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Address { get; private set; }
    public DateTime BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public Patient(string firstName, string lastName, string address, DateTime birthDate, Gender gender)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        BirthDate = birthDate.Date;
        Gender = gender;
    }
    public override string ToString() => $"{FirstName} {LastName}";

}