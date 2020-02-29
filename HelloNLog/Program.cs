using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;

namespace HelloNLog
{
    class Program
    {
        static LoggingConfiguration messageConfig;
        static LoggingConfiguration where_did_message_came_from_Config;

        static Program()
        {
            ConsoleTarget messageConsoleTarget = new ConsoleTarget
            {
                Name = "message",
                Layout = "${logger} ${message}"
            };
            messageConfig = new LoggingConfiguration();
            messageConfig.AddRuleForAllLevels(messageConsoleTarget);

            ConsoleTarget whereDidMessageComeFromConsoleTarget = new ConsoleTarget
            {
                Name = "where",
                Layout = "${callsite} ${message}"
            };
            where_did_message_came_from_Config = new LoggingConfiguration();
            where_did_message_came_from_Config.AddRuleForAllLevels(whereDidMessageComeFromConsoleTarget);

            ColoredConsoleTarget coloredConsoleTarget = new ColoredConsoleTarget
            {
                Name = "co",
                Layout = "------ ${message} ${logger}"
            };
            messageConfig = new LoggingConfiguration();
            messageConfig.AddRule(LogLevel.Trace, LogLevel.Fatal, coloredConsoleTarget, "NAME_CHANGE", true);
            messageConfig.AddRuleForAllLevels(coloredConsoleTarget);
        }

        static void Main(string[] args)
        {
            LogManager.Configuration = messageConfig;

            Doctor doctor = new Doctor("Timothy");
            Patient patient = new Patient("Frederick");
            doctor.ChangeName("Tim");
            patient.ChangeName("Fred");
        }

        public class Doctor
        {
            private static ILogger classlogger = LogManager.GetCurrentClassLogger();
            private static  ILogger nameChangeLogger = LogManager.GetLogger("NAME_CHANGE");
            private string _name;

            public Doctor(string name)
            {
                classlogger.Trace("Doctor.ctor() - Enter");
                _name = name;
                classlogger.Trace("Doctor.ctor() - Enter");
            }

            public void ChangeName(string name)
            {
                classlogger.Trace("ChangeName - Enter");
                _name = name;
                nameChangeLogger.Debug("name changed to {newName}", name);
                classlogger.Trace("ChangeName - Exit");
            }
        }

        public class Patient
        {
            private static ILogger classLogger = LogManager.GetCurrentClassLogger();
            private static ILogger nameChangeLogger = LogManager.GetLogger("NAME_CHANGE");
            private string _name;

            public Patient(string name)
            {
                classLogger.Trace("Patient.ctor() - Enter");
                _name = name;
                classLogger.Trace("Patient.ctor() - Exit");
            }

            public void ChangeName(string name)
            {
                classLogger.Trace("ChangeName - Enter");
                nameChangeLogger.Debug("{oldName} changed name to {newName}", _name, name);
                _name = name;
                classLogger.Trace("ChangeName - Exit");
            }
        }
    }
}
