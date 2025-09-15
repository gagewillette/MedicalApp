
public class Clinic
{
    public List<Patient> Patients { get; } = new List<Patient>(); // static list all patients registered to a clinic
    public List<Physician> Physicians { get; } = new List<Physician>(); // list of all physician registered to a clinic
    public List<Appointment> Appointments { get; } = new List<Appointment>(); // list of all past and active appointments


    // create new patient based on pat class member data
    public Patient CreatePatient(string firstName, string lastName, string address, DateTime birthDate, Gender gender)
    {
        var p = new Patient(firstName, lastName, address, birthDate, gender);
        Patients.Add(p);
        return p;
    }

    // create ny physician based on phy class member data
    public Physician CreatePhysician(string firstName, string lastName, string licenseNumber, DateTime graduationDate, IEnumerable<string> specializations)
    {
        var ph = new Physician(firstName, lastName, licenseNumber, graduationDate, specializations);
        Physicians.Add(ph);
        return ph;
    }

    // schedule new appointments
    public Appointment ScheduleAppointment(Guid physicianId, Guid patientId, DateTime startLocal, TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero) throw new Exception("Invalid duration");
        var start = startLocal;
        var end = startLocal.Add(duration);
        if (!IsBusinessDay(start) || !IsBusinessDay(end)) throw new Exception("Appointments must be Monday–Friday");
        if (!IsWithinBusinessHours(start, end)) throw new Exception("Appointments must be between 08:00 and 17:00 local time");
        if (!Physicians.Any(p => p.Id == physicianId)) throw new Exception("Physician not found");
        if (!Patients.Any(p => p.Id == patientId)) throw new Exception("Patient not found");
        if (IsDoubleBooked(physicianId, start, end)) throw new Exception("Physician is already booked for that time range");
        var appt = new Appointment(physicianId, patientId, start, end);
        Appointments.Add(appt);
        return appt;
    }

    // 
    //  business logic utility funcitons
    // 
    
    static bool IsBusinessDay(DateTime dt) 
    {
        return dt.DayOfWeek >= DayOfWeek.Monday && dt.DayOfWeek <= DayOfWeek.Friday;
    }

    static bool IsWithinBusinessHours(DateTime start, DateTime end)
    {
        var open = new DateTime(start.Year, start.Month, start.Day, 8, 0, 0);
        var close = new DateTime(start.Year, start.Month, start.Day, 17, 0, 0);
        var sameDay = start.Date == end.Date;
        return sameDay && start >= open && end <= close;
    }

    bool IsDoubleBooked(Guid physicianId, DateTime start, DateTime end)
    {
        return Appointments.Any(a => a.PhysicianId == physicianId && start < a.End && end > a.Start);
    }
}