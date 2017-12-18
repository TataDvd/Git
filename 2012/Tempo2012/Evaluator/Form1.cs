//////////////////////////////////////////////////////////////////////////////
// This source code and all associated files and resources are copyrighted by
// the author(s). This source code and all associated files and resources may
// be used as long as they are used according to the terms and conditions set
// forth in The Code Project Open License (CPOL), which may be viewed at
// http://www.blackbeltcoder.com/Legal/Licenses/CPOL.
//
// Copyright (c) 2010 Jonathan Wood
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SoftCircuits;

namespace TestEval
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnEvaluate_Click(object sender, EventArgs e)
		{
			try
			{
				// Evaluate the current expression
				Eval eval = new Eval();
				eval.ProcessSymbol += ProcessSymbol;
				eval.ProcessFunction += ProcessFunction;
				txtResult.Text = eval.Execute(txtExpression.Text).ToString();
			}
			catch (EvalException ex)
			{
				// Report expression error and move caret to error position
				MessageBox.Show(ex.Message);
				txtExpression.Select(ex.Column, 0);
				txtExpression.Select();
			}
			catch (Exception ex)
			{
				// Unknown error
				MessageBox.Show("Unexpected error : " + ex.Message);
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		// Implement expression symbols
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
                if (e.Parameters.Count ==2)
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
