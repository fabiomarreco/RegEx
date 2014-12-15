using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RegEx
{
    class Program
    {

        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                return Executa(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MostraMensagemAjuda();
                return -1;
            }

        }

        private static void MostraMensagemAjuda()
        {
            Console.WriteLine (
@"
   RegEx /c Console /x [XmlFile] /r [Regex] /rf [regexFile] /i [input] 
         /if [inputFile] /g [groupname]

        /c      Inicia o Modo Console
        /r      Regex a ser utilizado, ignora o parametro /rf
        /rf     Arquivo contendo o texto do regex
        /i      Texto a ser usado como iput, ignora o parametro /if
        /if     Arquivo com o texto a ser utilizado
        /g      Se houver nome de grupo mostra o grupo selecionado, 
                caso contrário, mostra todos
        /s      Habilita modo 'SingleLine'
");
        }


        private static int Executa(string[] args)
        {
            if (args.Length == 0)
            {
                MostraMensagemAjuda();
                return -1;
            }

            bool console = false;
            string arquivoXml = string.Empty;

            string regex = string.Empty;
            string regexFile = string.Empty;

            string input = string.Empty;
            string inputFile = string.Empty;
            bool groups = false;

            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled;

            string groupName = string.Empty;

            for (int i = 0; i < args.Length; i++)
            {
                string param = args[i].ToLower();
                if (param == "/x")
                    arquivoXml = args[++i];

                else if (param == "/r")
                    regex = args[++i];

                else if (param == "/rf")
                    regexFile = args[++i];

                else if (param == "/i")
                    input = args[++i];

                else if (param == "/if")
                    inputFile = args[++i];

                else if (param == "/c")
                    console = true;

                else if (param == "/s")
                    options = options | RegexOptions.Singleline;

                else if (param == "/g")
                {
                    if (i < args.Length - 1)
                    {
                        if (!args[i + 1].StartsWith("/"))
                            groupName = args[++i];
                    }
                    else
                        groups = true;
                }


                else if ((param == "/h") || (param == "-h") || (param == "?") || (param == "/?"))
                {
                    MostraMensagemAjuda();
                    return 0;
                }
            }

            if (input.Length == 0)
            {
                using (StreamReader sr = new StreamReader(inputFile, Encoding.Default, true))
                {
                    input = sr.ReadToEnd();
                }
            }

            if (console)
            {
                ConsoleAnalyser ca = new ConsoleAnalyser();
                ca.Executa(input);
                return 0;
            }

            if (regex.Length == 0)
            {
                using (StreamReader sr = new StreamReader(regexFile, Encoding.Default, true))
                {
                    regex = sr.ReadToEnd();
                }
            }



            Regex reg = new Regex(regex, options);
            MatchCollection matches = reg.Matches(input);

            if (arquivoXml.Length > 0)
            {
                XmlExporter exp = new XmlExporter();
                exp.Executa(reg, matches, arquivoXml);
                return matches.Count;
            }

            foreach (Match m in matches)
            {
                if (groupName.Length > 0)
                    Console.WriteLine(m.Groups[groupName]);
                else
                    Console.WriteLine(m.Value);

                if (groups)
                {

                    for (int i = 1; i < m.Groups.Count; i++ )
                    {
                        string name = reg.GroupNameFromNumber(i);
                        if (name.Length == 0) name = i.ToString();
                        Console.WriteLine("{0}:{1}", name.PadLeft (20), m.Groups[i].Value);
                    }
                    Console.WriteLine();
                }
            }

            return matches.Count;
            //Console.ReadKey();
        }
    }
}
