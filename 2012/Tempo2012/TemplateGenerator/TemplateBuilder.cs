using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tempo2012.EntityFramework;
using SoftCircuits;

namespace TemplateGenerator
{
    public class TemplateBuilder
    {
        private Dictionary<string, string> ContoVars = new Dictionary<string, string>();
        private StringBuilder SectionTemplate=new StringBuilder();
        public string ResultTemplate { get; set;}
        //public void PopulateVariables()
        //{
        //    Variables.Add("@A1", "10");
        //    Variables.Add("@A2", "20");
        //    Variables.Add("@A3", "35");
        //    Variables.Add("@A4", "53");
        //}

        public void PopulateSysVariables()
        {
            var context = new Tempo2012.EntityFramework.TempoDataBaseContext();
            ContoVars = context.GetOborotnaVedTemplate(new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, 1, 1), new DateTime(ConfigTempoSinglenton.GetInstance().WorkDate.Year, 12, 31));
            ContoVars.Add("@@FirmaName", Entrence.CurrentFirma.Name);
            ContoVars.Add("@@FirmaBulstat", Entrence.CurrentFirma.Bulstad);
            ContoVars.Add("@@FirmaAddress", Entrence.CurrentFirma.Address);
            ContoVars.Add("@@Operator", Entrence.UserName);
        }

        public void LoadTemplate(string fileName)
        {
            Eval eval = new Eval();
            eval.ProcessSymbol += ProcessSymbol;
            eval.ProcessFunction += ProcessFunction;
            PopulateSysVariables();
            string[] lines = System.IO.File.ReadAllLines(fileName);
            int secId = 1;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) && secId != 3) continue;
                if (line=="Section Vars")
                {
                    secId=1;
                    continue;
                }
                if (line == "Section Calculations")
                {
                    secId = 2;
                    continue;
                }
                if (line == "Section Template")
                {
                    secId = 3;
                    continue;
                }
                if (secId == 1)
                {
                    var items = line.Split('=');
                    if (ContoVars.ContainsKey(items[0]))
                    {
                        ContoVars[items[0]] = items[1];
                    }
                    else
                    {
                        ContoVars.Add(items[0], items[1]);
                    }
                }
                if (secId == 2)
                {
                    var items = line.Split('=');
                    string rez = items[1];
                    rez = ReplaceValues(rez);
                    rez = eval.Execute(rez).ToString();
                    if (ContoVars.ContainsKey(items[0]))
                    {
                        ContoVars[items[0]] = rez;
                    }
                    else
                    { 
                        ContoVars.Add(items[0], rez); 
                    }
                }
                if (secId == 3)
                {
                    SectionTemplate.AppendLine(ReplaceValues(line));
                }
            }
            ResultTemplate = SectionTemplate.ToString();
        }

        public string ReplaceValues(string input)
        {
            var rez = input;
            foreach (var item in ContoVars)
            {
                rez=rez.Replace(item.Key, item.Value);
            }
            return rez;
        }

        protected void ProcessSymbol(object sender, SymbolEventArgs e)
        {
            if (String.Compare(e.Name, "pi", true) == 0)
            {
                e.Result = Math.PI;
            }
            // Unknown symbol name
            else e.Status = SymbolStatus.UndefinedSymbol;
        }

        // Implement expression functions
        protected void ProcessFunction(object sender, FunctionEventArgs e)
        {
            if (String.Compare(e.Name, "abs", true) == 0)
            {
                if (e.Parameters.Count == 1)
                    e.Result = Math.Abs(e.Parameters[0]);
                else
                    e.Status = FunctionStatus.WrongParameterCount;
            }
            else if (String.Compare(e.Name, "pow", true) == 0)
            {
                if (e.Parameters.Count == 2)
                    e.Result = Math.Pow(e.Parameters[0], e.Parameters[1]);
                else
                    e.Status = FunctionStatus.WrongParameterCount;
            }
            else if (String.Compare(e.Name, "round", true) == 0)
            {
                if (e.Parameters.Count == 1)
                    e.Result = Math.Round(e.Parameters[0]);
                else
                    e.Status = FunctionStatus.WrongParameterCount;
            }
            else if (String.Compare(e.Name, "sqrt", true) == 0)
            {
                if (e.Parameters.Count == 1)
                    e.Result = Math.Sqrt(e.Parameters[0]);
                else
                    e.Status = FunctionStatus.WrongParameterCount;
            }
            else if (String.Compare(e.Name, "ifbig", true) == 0)
            {
                if (e.Parameters.Count == 4)
                {
                    if (e.Parameters[0] > e.Parameters[1])
                    {
                        e.Result = e.Parameters[2];
                    }
                    else
                    {
                        e.Result = e.Parameters[3];
                    }

                }
                else
                    e.Status = FunctionStatus.WrongParameterCount;
            }
            else if (String.Compare(e.Name, "getmax", true) == 0)
            {
                if (e.Parameters.Count == 2)
                {
                    if (e.Parameters[0] >= e.Parameters[1])
                    {
                        e.Result = e.Parameters[0];
                    }
                    else
                    {
                        e.Result = e.Parameters[1];
                    }

                }
                else
                    e.Status = FunctionStatus.WrongParameterCount;
            }
            else if (String.Compare(e.Name, "getmin", true) == 0)
            {
                if (e.Parameters.Count == 2)
                {
                    if (e.Parameters[0] <= e.Parameters[1])
                    {
                        e.Result = e.Parameters[0];
                    }
                    else
                    {
                        e.Result = e.Parameters[1];
                    }

                }
                else
                    e.Status = FunctionStatus.WrongParameterCount;
            }
            // Unknown function name
            else e.Status = FunctionStatus.UndefinedFunction;
        }
    }
}
