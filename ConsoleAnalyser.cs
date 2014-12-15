using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Windows.Forms;
using System.Runtime;
using System.Text;
using System.Collections.Generic;


namespace RegEx
{
    class ConsoleAnalyser
    {
        string ultReg = string.Empty;
        bool showGroup = false;
        bool output = false;
        string lastOutput = string.Empty;
        bool firstMatchOnly = false;
        string inputOriginal = string.Empty;
        string input;
        //bool outputAsCSV = false;

        public void Executa(string inputOriginal)
        {
            this.inputOriginal = inputOriginal;
            input = inputOriginal;

            string reg;
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;

            while ((reg = Console.ReadLine()) != "q")
            {
                if (reg == "h")
                {
                    Console.WriteLine(@"
REG EX TESTER
h             - Help
c             - Copy regex to Clipboard
g             - Toogle group view
s             - SingleLineMode (. is \\n too)
f             - First match only
p             - Paste Regex
w             - Write all output to file 'output.txt'
o             - Set Last output as input
r             - Reload from original input
opt [options] - View or change options, 'opt' by itself displays current options
                Options are separated by comma. Avaiable options are:
                (None, IgnoreCase, Multiline, ExplicitCapture, Compiled, 
                 Singleline, IgnorePatternWhitespace, RightToLeft, 
                 ECMAScript, CultureInvariant)

q           - Quit
");
                    continue;
                }
                else if (reg == "c")
                {
                    Clipboard.SetText(ultReg);
                    Console.WriteLine("Copied to Clipboard");
                    continue;
                }
                else if (reg == "g")
                {
                    showGroup = !showGroup;
                    Console.WriteLine("Group View " + showGroup.ToString());
                    continue;
                }
                else if (reg == "q")
                {
                    return;
                }
                else if (reg == "s")
                {
                    bool singleLine;
                    if (((RegexOptions.Singleline & options) == RegexOptions.Singleline))
                    {
                        singleLine = false;
                        options &= ~RegexOptions.Singleline;
                    }
                    else
                    {
                        singleLine = true;
                        options |= RegexOptions.Singleline;
                    }

                    Console.WriteLine("Single Line Mode: " + singleLine.ToString());
                    continue;
                }
                else if (reg == "opt")
                {
                    Console.WriteLine(options);
                }
                else if (reg.StartsWith ("opt ") && (reg.Length > 4))
                {
                    string strOptions = reg.Substring(4, reg.Length - 4);

                    try
                    {
                        options = (RegexOptions)Enum.Parse(typeof(RegexOptions), strOptions);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                else if (reg == "f")
                {
                    firstMatchOnly = !firstMatchOnly;
                    Console.WriteLine("First match Only: " + firstMatchOnly.ToString());
                    continue;
                }
                else if (reg == "p")
                {
                    reg = Clipboard.GetText();
                    Console.WriteLine(reg);
                }
                else if (reg == "o")
                {
                    input = lastOutput;
                    Console.WriteLine("Input Set");
                    continue;
                }
                else if (reg == "w")
                {
                    output = !output;
                    Console.WriteLine("Output to file: " + output.ToString());
                    continue;
                }
                else if (reg == "r")
                {
                    input = inputOriginal;
                    Console.WriteLine("Reloaded.");
                    continue;
                }



                try
                {
                    Regex regex = new Regex(reg, options);

                    if (!firstMatchOnly)
                    {
                        foreach (Match match in regex.Matches(input))
                        {
                            ShowMatch(regex, match);
                            lastOutput = match.Value;
                        }
                    }
                    else
                    {
                        Match match = regex.Match(input);
                        if (match != null)
                        {
                            ShowMatch(regex, match);
                            lastOutput = match.Value;
                        }

                    }
                    ultReg = reg;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


                Console.WriteLine("---------------------------------------");
            }
        }

        private void ShowMatch(Regex reg, Match match)
        {
            Console.WriteLine(match.Value);
            if (output)
            {
                //string arquivo = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "output.txt";
                string arquivo = "output.txt";
                using (StreamWriter sw = new StreamWriter(arquivo, true, Encoding.Default))
                {
                    sw.WriteLine(match.Value);
                    if (showGroup)
                    {
                        Console.SetOut(sw);
                        ShowGroups(reg, match);
                        Console.SetOut(Console.Out);
                    }
                }
            }
            if (showGroup)
            {
                ShowGroups(reg, match);
            }
        }

        private void ShowGroups(Regex reg, Match match)
        {
            for (int i = 1; i < match.Groups.Count; i++)
            {
                Group group = match.Groups[i];
                string name = reg.GroupNameFromNumber(i);
                Console.WriteLine(name.PadLeft(20) + " : " + group.Value);
            }
        }
    }
}
