public  class Appointment
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid PhysicianId { get; }
    public Guid PatientId { get; }
    public DateTime Start { get; }
    public DateTime End { get; }
    public Appointment(Guid physicianId, Guid patientId, DateTime start, DateTime end)
    {
        PhysicianId = physicianId;
        PatientId = patientId;
        Start = start;
        End = end;
    }
    public override string ToString() => $"{Start:yyyy-MM-dd HH:mm}–{End:HH:mm}";
}
