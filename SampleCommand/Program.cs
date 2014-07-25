﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SampleCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            Log("started");

            string line;
            switch (args[0])
            {
                case "grep":
                    var regex = new Regex(args[1]);
                    while ((line = Console.ReadLine()) != null)
                    {
                        //Log("Read '{0}'", line);
                        if (regex.Match(line).Success)
                        {
                            Console.WriteLine(line);
                            //Log("Wrote '{0}'", line);
                        }
                    }
                    break;
                case "head":
                    var count = int.Parse(args[1]);
                    var i = 0;
                    while ((i++) < count && (line = Console.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                    break;
                case "exit":
                    var code = int.Parse(args[1]);
                    Environment.Exit(code);
                    break;
                default:
                    Console.Error.WriteLine("Unrecognized mode " + args[0]);
                    Environment.Exit(-1);
                    break;
            }

            Log("Exited normally");
        }

        public static void Log(string format, params object[] args)
        {
            var baseText = string.Format("{0:h:m:ss.fff} {1} ({2}): ", DateTime.Now, Process.GetCurrentProcess().Id, string.Join(" ", Environment.GetCommandLineArgs()));
            var text = baseText + string.Format(format, args);

            using (var fs = new FileStream("log.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var writer = new StreamWriter(fs))
            {
                writer.WriteLine(text);
            }
        }
    }
}
