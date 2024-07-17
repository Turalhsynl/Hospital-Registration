using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Hospital_Registration;



class Program
{
    static void Main(string[] args)
    {
        AppointmentSystem appointmentSystem = new AppointmentSystem();
        appointmentSystem.Start();
    }
}
