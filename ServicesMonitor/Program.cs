using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace ServicesMonitor
{
    class ServiNote
    {
        public ServiNote(string serviceName)
        {
            ServiceName = serviceName;
            Instance = string.Empty;
            Controller = new ServiceController(serviceName);
        }
        public ServiNote(string serviceName, string instance):this(serviceName)
        {
            Instance = instance;
        }
        public string ServiceName { get; private set; }
        public string Instance { get; private set; }
        public ServiceController Controller { get; private set; }
    }
    class Program
    {
        static int ReadOption(string[] options)
        {
            int op = -1;
            while(op < 0)
            {
                Console.WriteLine("Write the number of an option");
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{i}. {options[i]}");
                }
                string s = Console.ReadLine();
                Console.Clear();
                if(int.TryParse(s,out op))
                {
                    if (op >= options.Length)
                    {
                        Console.WriteLine("Inalid Option.");
                        op = -1;
                    }
                }
                else
                {
                    op = -1;
                    Console.WriteLine("No valid input.");
                }
            }
            return op;
        }
        static void StartService(ServiNote note)
        {
            try
            {
                note.Controller.Start();
                note.Controller.WaitForStatus(ServiceControllerStatus.Running);
                Console.WriteLine("Service is Running!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine($"{note.Controller.DisplayName}\t{note.Controller.Status.ToString()}");
        }
        static void StopService(ServiNote note)
        {
            try
            {
                note.Controller.Stop();
                note.Controller.WaitForStatus(ServiceControllerStatus.Stopped);
                Console.WriteLine("Service is Stopped!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine($"{note.Controller.DisplayName}\t{note.Controller.Status.ToString()}");
        }
        static void RestartService(ServiNote note)
        {
            StopService(note);
            StartService(note);
        }

        static void Main(string[] args)
        {
            List<ServiNote> services = new List<ServiNote>();
            const string SQLsever = "SQL Server";
            string[] main = {
                "List Services in Monitor",
                "Add a Service to Monitor",
                "Add a Service to Monitor and Start",
                "Remove a Service from Monitor",
                "Remove a Service from Monitor and Stop a Service",
                "Start a Service in Monitor",
                "Restart a Service in Monitor",
                "Stop a Servive in Monitor",
                SQLsever + " Instance Services Monitor",
                SQLsever + " Integration Services 14.0 Monitor",
                SQLsever + " Browser",
                SQLsever + " VSS Writer",
                "Exit"
            };
            string[] sqlmenu =
            {
                "Add Services to Monitor",
                "Add Services to Monitor and Start",
                "Remove Services from Monitor",
                "Remove Services from Monitor and Stop",
                "Back To Main Menu"
            };
            string[] sqlser =
            {
                "",
                "Agent",
                "Analysis Services",
                "Analysis Services CEIP",
                "CEIP service",
                "Launchpad"
            };
            string[] intser =
            {
                "14.0",
                "CEIP service 14.0",
                "Scale Out Master 14.0",
                "Scale Out Worker 14.0",
            };
            string[] sermenu =
            {
                "Start",
                "Restart",
                "Stop",
                "Back To Main Menu"
            };
            int op = -1;
            int o = -1;
            while (op + 1 < main.Length)
            {
                List<string> lists = new List<string>();
                foreach (var item in services)
                {
                    lists.Add($"{item.Controller.DisplayName}\t{item.Controller.Status.ToString()}");
                }
                op = ReadOption(main);
                switch (op)
                {
                    case 0:
                        if (lists.Count == 0) Console.WriteLine("No services in Monitor");
                        else foreach (var item in lists)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case 1:
                        Console.WriteLine("Service Name: ");
                        try
                        {
                            ServiNote note = new ServiNote(Console.ReadLine());
                            services.Add(note);
                            Console.WriteLine($"{note.Controller.DisplayName}\t{note.Controller.Status.ToString()}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Service Name: ");
                        try
                        {
                            ServiNote note = new ServiNote(Console.ReadLine());
                            StartService(note);
                            services.Add(note);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case 3:
                        lists.Add("Back To Main Menu");
                        o = ReadOption(lists.ToArray());
                        if(o + 1 != lists.Count) services.RemoveAt(o);
                        break;
                    case 4:
                        lists.Add("Back To Main Menu");
                        o = ReadOption(lists.ToArray());
                        if (o + 1 != lists.Count)
                        {
                            StopService(services[o]);
                            services.RemoveAt(o);
                        }
                        break;
                    case 5:
                        lists.Add("Back To Main Menu");
                        o = ReadOption(lists.ToArray());
                        if (o + 1 != lists.Count) StartService(services[o]);
                        break;
                    case 6:
                        lists.Add("Back To Main Menu");
                        o = ReadOption(lists.ToArray());
                        if (o + 1 != lists.Count) RestartService(services[o]);
                        break;
                    case 7:
                        lists.Add("Back To Main Menu");
                        o = ReadOption(lists.ToArray());
                        if (o + 1 != lists.Count) StopService(services[o]);
                        break;
                    case 8:
                        Console.WriteLine("Instance Name");
                        string inst = Console.ReadLine();
                        o = ReadOption(sqlmenu);
                        switch (o)
                        {
                            case 0:
                                foreach (var item in sqlser)
                                {
                                    string sn = string.IsNullOrWhiteSpace(item) ? " " : $" {item} ";
                                    sn = $"{SQLsever}{sn}({inst})";
                                    try
                                    {
                                        ServiNote note = new ServiNote(sn,inst);
                                        services.Add(note);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }
                                break;
                            case 1:
                                foreach (var item in sqlser)
                                {
                                    string sn = string.IsNullOrWhiteSpace(item) ? " " : $" {item} ";
                                    sn = $"{SQLsever}{sn}({inst})";
                                    try
                                    {
                                        ServiNote note = new ServiNote(sn,inst);
                                        StartService(note);
                                        services.Add(note);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }
                                break;
                            case 2:
                                try
                                {
                                    List<ServiNote> sublist = services.Where(s => s.Instance == inst).ToList();
                                    foreach (var item in sublist)
                                    {
                                        if (!services.Remove(item))
                                        {
                                            Console.WriteLine($"{item.Controller.DisplayName} not removed from list.");
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            case 3:
                                try
                                {
                                    List<ServiNote> sublist = services.Where(s => s.Instance == inst).ToList();
                                    sublist.Reverse();
                                    foreach (var item in sublist)
                                    {
                                        StopService(item);
                                        if (!services.Remove(item))
                                        {
                                            Console.WriteLine($"{item.Controller.DisplayName} not removed from list.");
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            default:
                                Console.WriteLine("");
                                break;
                        }
                        break;
                    case 9:
                        o = ReadOption(sqlmenu);
                        switch (o)
                        {
                            case 0:
                                foreach (var item in intser)
                                {
                                    string sn = $"{SQLsever} Integration Services {item}";
                                    try
                                    {
                                        ServiNote note = new ServiNote(sn);
                                        services.Add(note);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }
                                break;
                            case 1:
                                foreach (var item in intser)
                                {
                                    string sn = $"{SQLsever} Integration Services {item}";
                                    try
                                    {
                                        ServiNote note = new ServiNote(sn);
                                        StartService(note);
                                        services.Add(note);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }
                                break;
                            case 2:
                                try
                                {
                                    List<ServiNote> sublist = services.Where(s => s.ServiceName.Contains("Integration Services") && s.ServiceName.Contains("14.0")).ToList();
                                    foreach (var item in sublist)
                                    {
                                        if (!services.Remove(item))
                                        {
                                            Console.WriteLine($"{item.Controller.ServiceName} not removed from list.");
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            case 3:
                                try
                                {
                                    List<ServiNote> sublist = services.Where(s => s.ServiceName.Contains("Integration Services") && s.ServiceName.Contains("14.0")).ToList();
                                    foreach (var item in sublist)
                                    {
                                        StopService(item);
                                        if (!services.Remove(item))
                                        {
                                            Console.WriteLine($"{item.ServiceName} not removed from list.");
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            default:
                                Console.WriteLine("");
                                break;
                        }
                        break;
                    case 10:
                        o = ReadOption(sermenu);
                        switch (o)
                        {
                            case 0:
                                try
                                {
                                    ServiNote note = new ServiNote(SQLsever + " Browser");
                                    StartService(note);
                                    services.Add(note);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            case 1:
                                try
                                {
                                    ServiNote note = services.FirstOrDefault(s => s.ServiceName == SQLsever + " Browser");
                                    RestartService(note);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            case 2:
                                try
                                {
                                    ServiNote note = services.FirstOrDefault(s => s.ServiceName == SQLsever + " Browser");
                                    StopService(note);
                                    if (!services.Remove(note))
                                    {
                                        Console.WriteLine($"{note.Controller.ServiceName} not removed from list.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            default:
                                Console.Write("");
                                break;
                        }
                        break;
                    case 11:
                        o = ReadOption(sermenu);
                        switch (o)
                        {
                            case 0:
                                try
                                {
                                    ServiNote note = new ServiNote(SQLsever + " VSS Writer");
                                    StartService(note);
                                    services.Add(note);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            case 1:
                                try
                                {
                                    ServiNote note = services.FirstOrDefault(s => s.ServiceName == SQLsever + " VSS Writer");
                                    RestartService(note);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            case 2:
                                try
                                {
                                    ServiNote note = services.FirstOrDefault(s => s.ServiceName == SQLsever + " VSS Writer");
                                    StopService(note);
                                    if (!services.Remove(note))
                                    {
                                        Console.WriteLine($"{note.Controller.ServiceName} not removed from list.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            default:
                                Console.Write("");
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("End Of the Program");
                        break;
                }
                Console.WriteLine("Any Key To Return");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
