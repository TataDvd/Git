﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateGenerator
{
    public class MathEvaluator
    {
        public static void Run()
        {
            Eval("(1+2)");
            Eval("5*4/2");
            Eval("((3+5)-6)");
        }

        public static void Eval(string input)
        {
            var ans = Evaluate(input);
            Console.WriteLine(input + " = " + ans);
        }

        public static double Evaluate(String input)
        {
            String expr = "(" + input + ")";
            Stack<String> ops = new Stack<String>();
            Stack<Double> vals = new Stack<Double>();
            string val = "";
            for (int i = 0; i < expr.Length; i++)
            {
                String s = expr.Substring(i, 1);
                if (IsDigit(s))
                {
                    val += s;
                    continue;
                }
                if (s.Equals("(")) { if (val!="") vals.Push(Double.Parse(val));val = ""; }
                else if (s.Equals("+")) { ops.Push(s);  if (val!="") vals.Push(Double.Parse(val)); val = ""; }
                else if (s.Equals("-")) { ops.Push(s);  if (val!="") vals.Push(Double.Parse(val)); val = ""; }
                else if (s.Equals("*")) { ops.Push(s);  if (val!="") vals.Push(Double.Parse(val)); val = ""; }
                else if (s.Equals("/")) {ops.Push(s);  if (val!="") vals.Push(Double.Parse(val));val="";}
                else if (s.Equals("sqrt")) {ops.Push(s);  if (val!="") vals.Push(Double.Parse(val));val="";}
                else if (s.Equals(")"))
                {
                    if (val!="") vals.Push(Double.Parse(val));val="";
                    int count = ops.Count;
                    while (count > 0)
                    {
                        String op = ops.Pop();
                        double v = vals.Pop();
                        if (op.Equals("+")) v = vals.Pop() + v;
                        else if (op.Equals("-")) v = vals.Pop() - v;
                        else if (op.Equals("*")) v = vals.Pop() * v;
                        else if (op.Equals("/")) v = vals.Pop() / v;
                        else if (op.Equals("sqrt")) v = Math.Sqrt(v);
                        vals.Push(v);

                        count--;
                    }
                }
                
            }
            return vals.Pop();
        }

        private static bool IsDigit(string s)
        {
            int rez;
            if (s == ".") return true;
            if (int.TryParse(s,out rez))
            {
                return true;
            }
            return false;
        }
    }

}
