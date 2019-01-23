using RecoilAssist.DatabaseModule;
using RecoilAssist.Patterns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RecoilAssist.UI_Console
{
    /// <summary>
    /// Kontroler konsoli
    /// </summary>
    public class ConsoleUI
    {
        public const string HelpText = "Commands:\n" + HelpText_Pattern + "\n\n" + HelpText_Turn + "\n\n" + "\tsave" + "\n\n" + "\texit";
        public const string HelpText_Pattern = "\tpattern add {A:double} {B:double} {C:double}\n\t\targuments\n\t\t-type [q|l|r|e]  type of pattern (quadric | linear | rational | exponential)\n\t\t-time {start_time:double} {duration:double}  time for pattern\n\t\t-d [v/h]  direction of pattern (vertical/horizontal)";
        public const string HelpText_Turn = "\tturn [off|on]";

        private RecoilAssistController recoilAssist;

        /// <summary>
        /// true - jeœli program ma dokoñczyæ pêtlê i zakoñczyæ program, false - jeœli program ma dalej reagowaæ na komendy
        /// </summary>
        private bool exit = false;

        public IDatabaseProvider DatabaseProvider { get; set; }

        /// <summary>
        /// Strumieñ wejœciowy
        /// </summary>
        public TextReader In { get; set; }

        /// <summary>
        /// Strumieñ wyjœciowy
        /// </summary>
        public TextWriter Out { get; set; }

        /// <summary>
        /// true - jeœli program nas³uchuje zdarzenia i koryguje ruchy myszki, false - w.p.p.
        /// </summary>
        public bool IsActive => recoilAssist.IsActive;

        /// <summary>
        /// Uruchom konsolê
        /// </summary>
        public void Run()
        {
            // Inicjalizacja
            recoilAssist = new RecoilAssistController();
            recoilAssist.AttachedPatterns.AddRange(DatabaseProvider.Patterns);
            recoilAssist.Listener.OnMousePressed += Listener_OnMousePressed;
            recoilAssist.SetActive(true);

            // Czekaj na komendy
            while (!exit)
            {
                InnerLoop();
            }
        }

        /// <summary>
        /// Zamknij konsolê
        /// </summary>
        private void Exit()
        {
            exit = true;
            DatabaseProvider.Dispose();
            recoilAssist.Dispose();
        }

        /// <summary>
        /// G³ówna pêtla
        /// </summary>
        private void InnerLoop()
        {
            string command = In.ReadLine();
            ProcessCommand(command);
        }

        /// <summary>
        /// Obs³u¿ komendê
        /// </summary>
        public void ProcessCommand(string command)
        {
            var args = SplitCommand(command);

            if (args.Count == 0)
                return;

            switch (args[0])
            {
                case "q":
                    Pattern(new List<string>(new string[] { "pattern", "add", "-type", "l", "-time", "0", "5", "-10", "0" }));
                    break;
                case "exit":
                    Exit();
                    break;
                case "pattern":
                    Pattern(args);
                    break;
                case "turn":
                    Turn(args);
                    break;
                case "save":
                    Save(args);
                    break;
                default:
                    Out.WriteLine(HelpText);
                    break;
            }
        }

        /// <summary>
        /// "Potnij" komendê na oddzielne argumenty
        /// </summary>
        /// <param name="command">linia komendy (pojedyñczy string)</param>
        /// <returns>lista argumentów</returns>
        public IList<string> SplitCommand(string command)
        {
            var args = new List<string>();

            if (string.IsNullOrEmpty(command))
                return args;

            var builder = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < command.Length; i++)
            {
                if ((!inQuotes && command[i] == ' ') || (inQuotes && command[i] == '"'))
                {
                    args.Add(builder.ToString());
                    builder.Clear();
                    inQuotes = false;
                }
                else if (!inQuotes && builder.Length == 0 && command[i] == '"')
                {
                    inQuotes = true;
                }
                else
                {
                    builder.Append(command[i]);
                }
            }

            if (builder.Length > 0)
                args.Add(builder.ToString());

            return args;
        }

        /// <summary>
        /// Komenda PATTERN
        /// </summary>
        /// <param name="args"></param>
        private void Pattern(IList<string> args)
        {
            if (args.Count > 1 && args[1] == "add")
            {
                FunctionType? type = null;
                MovementDirection direction = MovementDirection.Vertical;
                double[] values = new double[3];
                int vIndex = 0;
                double? timeStart = null;
                double? duration = null;

                for (int i = 2; i < args.Count; i++)
                {
                    if (args[i] == "-type")
                    {
                        if (args[++i] == "q")
                            type = FunctionType.Quadratic;
                        else if (args[i] == "e")
                            type = FunctionType.Exponential;
                        else if (args[i] == "r")
                            type = FunctionType.Rational;
                        else if (args[i] == "l")
                            type = FunctionType.Logarithmic;
                    }
                    else if (args[i] == "-d")
                    {
                        if (args[++i] == "v")
                            direction = MovementDirection.Vertical;
                        else if (args[i] == "h")
                            direction = MovementDirection.Horizontal;
                    }

                    else if (args[i] == "-time")
                    {
                        double value;
                        if (double.TryParse(args[++i], out value))
                        {
                            timeStart = value;
                        }

                        if (double.TryParse(args[++i], out value))
                        {
                            duration = value;
                        }
                    }
                    else
                    {
                        double value;
                        if (double.TryParse(args[i], out value) && vIndex < 3)
                        {
                            values[vIndex++] = value;
                        }
                    }
                }

                if (type.HasValue && timeStart.HasValue && duration.HasValue)
                {
                    var start = TimeSpan.FromSeconds(timeStart.Value);
                    var stop = start.Add(TimeSpan.FromSeconds(duration.Value));
                    recoilAssist.AttachedPatterns.Add(MovementPattern.Create(type.Value, values, direction, start, stop));
                    Out.WriteLine("Pattern added!");
                }
                else if (!type.HasValue)
                {
                    Out.WriteLine("Type is missing!\n");
                    Out.WriteLine(HelpText_Pattern);
                }
                else
                {
                    Out.WriteLine("Time is missing!\n");
                    Out.WriteLine(HelpText_Pattern);
                }
            }
            else if (args.Count > 1 && args[1] == "clear")
            {
                recoilAssist.AttachedPatterns.Clear();
                Out.WriteLine("Patterns cleared!");
            }
            else if (args.Count == 1)
            {
                for (int i = 0; i < recoilAssist.AttachedPatterns.Count; i++)
                {
                    Out.WriteLine($"{i}: {recoilAssist.AttachedPatterns[i].ToString()}");
                }
            }
            else
            {
                Out.WriteLine(HelpText_Pattern);
            }
        }

        /// <summary>
        /// Komenda TURN
        /// </summary>
        /// <param name="args"></param>
        private void Turn(IList<string> args)
        {
            if (args.Count > 1 && args[1] == "off")
            {
                recoilAssist.SetActive(false);
                Out.WriteLine("Turned ON. Program listen on mouse events ...");
            }
            else if (args.Count > 1 && args[1] == "on")
            {
                recoilAssist.SetActive(true);
                Out.WriteLine("Turned OFF. Program suspended correcting mouse movement.");
            }
            else
            {
                Out.WriteLine(HelpText_Turn);
            }
        }

        /// <summary>
        /// Komenda SAVE
        /// </summary>
        private void Save(IList<string> args)
        {
            DatabaseProvider.Patterns.Clear();
            foreach (var item in recoilAssist.AttachedPatterns)
                DatabaseProvider.Patterns.Add(item);
            DatabaseProvider.CommitChanges();
            Out.WriteLine("Data saved!");
        }

        private void Listener_OnMousePressed(object sender, Controllers.Vector e)
        {
            Out.WriteLine("Mouse pressed, MouseWorker Fired!");
        }

    }
}
