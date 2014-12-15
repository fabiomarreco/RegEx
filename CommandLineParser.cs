using System;
using System.Collections.Generic;
using System.Text;

namespace RegEx
{
    class CommandLineParser
    {
        public CommandLineParser()
        {
        }


        public void Load(params string[] cmdLine)
        {
            
        }




        private IList<Parameter> parametros = new List<Parameter>();
        public IList<Parameter> Parametros
        {
            get { return parametros; }
            set { parametros = value; }
        }
        public void AdicionaParametro(Parameter parametro)
        {
            parametros.Add (parametro);
        }
        public void AdicionaParametro (string nome, string keyword, int paramCount)
        {
            parametros.Add (new Parameter (nome, keyword, paramCount));
        }

        public void AdicionaParametro(string nome, int paramCount)
        {
            parametros.Add(new Parameter(nome, string.Empty, paramCount));
        }

    }


    class Parameter
    {
        public Parameter()
        {

        }
        public Parameter(string name, string keyword, int subParamCount)
        {
            this.name = name;
            this.subParameterCount = subParamCount;
            this.keyword = keyword;
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int subParameterCount;
        public int SubParameterCount
        {
            get { return subParameterCount; }
            set { subParameterCount = value; }
        }
        private IList<string> subParameters;
        public IList<string> SubParameters
        {
            get { return subParameters; }
            set { subParameters = value; }
        }

        private string keyword;

        public string KeyWord
        {
            get { return keyword; }
            set { keyword = value; }
        }
	
    }
}
