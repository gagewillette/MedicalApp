class PatientSoftware
{
    static Clinic clinic = new Clinic();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1) Create Patient");
            Console.WriteLine("2) List Patients");
            Console.WriteLine("3) Create Physician");
            Console.WriteLine("4) List Physicians");
            Console.WriteLine("5) Schedule Appointment");
            Console.WriteLine("6) List Appointments");
            Console.WriteLine("7) List Appointments By Physician");
            Console.WriteLine("0) Exit");
            Console.Write("Select: ");
            var choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1": CreatePatient(); break;
                    case "2": ListPatients(); break;
                    case "3": CreatePhysician(); break;
                    case "4": ListPhysicians(); break;
                    case "5": ScheduleAppointment(); break;
                    case "6": ListAppointments(); break;
                    case "7": ListAppointmentsByPhysician(); break;
                    case "0": return;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // create new patient based on input
    static void CreatePatient()
    {
        var first = Prompt("First name");
        var last = Prompt("Last name");
        var address = Prompt("Address");
        var birth = PromptDate("Birthdate (yyyy-mm-dd)");
        var gender = PromptGender();
        var p = clinic.CreatePatient(first, last, address, birth, gender);
        Console.WriteLine($"Created patient {p.Id} {p}");
    }

    // helper function for  print menu
    static void ListPatients()
    {
        if (!clinic.Patients.Any()) { Console.WriteLine("No patients"); return; }
        foreach (var (p, i) in clinic.Patients.Select((x, i) => (x, i)))
            Console.WriteLine($"{i + 1}) {p.Id} {p} | {p.Gender} | {p.BirthDate:yyyy-MM-dd}");
    }

    // helper func to create new physician
    // see physician class
    static void CreatePhysician()
    {
        var first = Prompt("First name");
        var last = Prompt("Last name");
        var license = Prompt("License number");
        var grad = PromptDate("Graduation date (yyyy-mm-dd)");
        var specs = Prompt("Specializations (comma-separated)").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var ph = clinic.CreatePhysician(first, last, license, grad, specs);
        Console.WriteLine($"Created physician {ph.Id} {ph}");
    }


    // itr thru physician list in clinic class and print
    static void ListPhysicians()
    {
        if (!clinic.Physicians.Any()) { Console.WriteLine("No physicians"); return; }
        foreach (var (d, i) in clinic.Physicians.Select((x, i) => (x, i)))
            Console.WriteLine($"{i + 1}) {d.Id} {d} | Lic {d.LicenseNumber} | Grad {d.GraduationDate:yyyy-MM-dd} | [{string.Join(", ", d.Specializations)}]");
    }

    // prompts user for physician, patient and dates, then checks for valid apointment scheduling
    // if all valid, adds to appointmetn list
    static void ScheduleAppointment()
    {
        var physician = SelectPhysician();
        var patient = SelectPatient();
        var date = PromptDate("Appointment date (yyyy-mm-dd)");
        var time = PromptTime("Start time (HH:mm, 24h)");
        var durationMin = PromptInt("Duration minutes");
        var start = date.Date.Add(time);
        var appt = clinic.ScheduleAppointment(physician.Id, patient.Id, start, TimeSpan.FromMinutes(durationMin));
        Console.WriteLine($"Scheduled {appt} with {physician} for {patient}");
    }

    // print appointments
    static void ListAppointments()
    {
        if (!clinic.Appointments.Any()) { Console.WriteLine("No appointments"); return; }
        foreach (var (a, i) in clinic.Appointments.OrderBy(a => a.Start).Select((x, i) => (x, i)))
        {
            var d = clinic.Physicians.FirstOrDefault(p => p.Id == a.PhysicianId);
            var p = clinic.Patients.FirstOrDefault(pt => pt.Id == a.PatientId);
            Console.WriteLine($"{i + 1}) {a.Id} | {a.Start:yyyy-MM-dd HH:mm}–{a.End:HH:mm} | Dr {d} | {p}");
        }
    }

    // print appointments by prompted physician
    static void ListAppointmentsByPhysician()
    {
        var physician = SelectPhysician();
        var appts = clinic.Appointments.Where(a => a.PhysicianId == physician.Id).OrderBy(a => a.Start).ToList();
        if (!appts.Any()) { Console.WriteLine("No appointments"); return; }
        foreach (var (a, i) in appts.Select((x, i) => (x, i)))
        {
            var p = clinic.Patients.First(pt => pt.Id == a.PatientId);
            Console.WriteLine($"{i + 1}) {a.Id} | {a.Start:yyyy-MM-dd HH:mm}–{a.End:HH:mm} | {p}");
        }
    }

    // select patient from prmopt
    static Patient SelectPatient()
    {
        if (!clinic.Patients.Any()) throw new Exception("No patients");
        ListPatients();
        var idx = PromptInt("Select patient #") - 1;
        if (idx < 0 || idx >= clinic.Patients.Count) throw new Exception("Invalid selection");
        return clinic.Patients[idx];
    }

    // select physician from prompt
    static Physician SelectPhysician()
    {
        if (!clinic.Physicians.Any()) throw new Exception("No physicians");
        ListPhysicians();
        var idx = PromptInt("Select physician #") - 1;
        if (idx < 0 || idx >= clinic.Physicians.Count) throw new Exception("Invalid selection");
        return clinic.Physicians[idx];
    }


    //
    // Helper functions for printing and prompting values to and from console
    //

    static string Prompt(string label)
    {
        Console.Write($"{label}: ");
        return Console.ReadLine() ?? "";
    }

    static DateTime PromptDate(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            if (DateTime.TryParse(Console.ReadLine(), out var dt)) return dt.Date;
            Console.WriteLine("Invalid date");
        }
    }

    static TimeSpan PromptTime(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            if (TimeSpan.TryParse(Console.ReadLine(), out var ts)) return ts;
            Console.WriteLine("Invalid time");
        }
    }

    static int PromptInt(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            if (int.TryParse(Console.ReadLine(), out var v)) return v;
            Console.WriteLine("Invalid number");
        }
    }

    static Gender PromptGender()
    {
        while (true)
        {
            Console.Write("Gender (1=Male, 2=Female)");
            var s = Console.ReadLine();
            if (s == "1") return Gender.Male;
            if (s == "2") return Gender.Female;
            Console.WriteLine("Invalid");
        }
    }
}
