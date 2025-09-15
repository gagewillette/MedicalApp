public class Physician
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string LicenseNumber { get; private set; }
    public DateTime GraduationDate { get; private set; }
    public List<string> Specializations { get; private set; }
    public Physician(string firstName, string lastName, string licenseNumber, DateTime graduationDate, IEnumerable<string> specializations)
    {
        FirstName = firstName;
        LastName = lastName;
        LicenseNumber = licenseNumber;
        GraduationDate = graduationDate.Date;
        Specializations = specializations?.Distinct(StringComparer.OrdinalIgnoreCase).ToList() ?? new List<string>();
    }
    public override string ToString() => $"{FirstName} {LastName}";
}
