using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
	public enum Operations
	{
		Add, Substract, Multiplay, Divide
	}
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		double? Operand1; // первый операнд
		double? Operand2; // второй операнд
		double curOperand; // число с дисплея
		double memory;
		Operations? oper = null;
		string display;
		string result; // результат операции
		public MainWindow()
		{
			InitializeComponent();
		}


		private void Memory_Click(object sender, RoutedEventArgs e)
		{
			Memory((sender as Button).Tag.ToString());
		}

		private void btnOperand_Click(object sender, RoutedEventArgs e)
		{
			this.txtDisplay.Text = ShowOperand((sender as Button).Tag.ToString()); // выводит на дисплей операнд
		}

		private void btnOperator_Click(object sender, RoutedEventArgs e)
		{
			SetOperator((sender as Button).Tag.ToString());
			GetResult();
			SetOperand();
		}

		private void btnEqualy_click(object sender, RoutedEventArgs e)
		{
			SetOperand();
			GetResult();
			ShowResult();
		}

		/// <summary>
		/// выводит на экран операнд memory
		/// </summary>
		/// <param name="tag"></param>
		private void Memory(string tag)
		{
			switch (tag)
			{
				case "M+":
					if (this.txtDisplay.Text == display && this.txtDisplay.Text != "0")
					{
						memory += double.Parse(display);
						this.txtMemory.Text = "Memory: " + memory.ToString();
					}
					else if (this.txtDisplay.Text == result)
					{
						memory += double.Parse(result);
						this.txtMemory.Text = "Memory: " + memory.ToString();
					}
					else if (this.txtDisplay.Text == "0")
						this.txtMemory.Text = "";
					break;

				case "MC":
					if (this.txtMemory.Text != "")
					{
						memory = 0;
						this.txtMemory.Text = "";
					}
					break;

				case "MR":

					if (this.txtMemory.Text != "" && Operand1 != null)
					{
						Operand1 = null;
						Memory(tag);
					}

					else if (this.txtMemory.Text != "" && Operand1 == null)
					{
						Operand1 = memory;
						this.txtDisplay.Text = Operand1.ToString();
					}

					else if (this.txtMemory.Text != "" && Operand2 != null)
					{
						Operand2 = memory;
						this.txtDisplay.Text = Operand2.ToString();
					}
					break;
			}
		}

		/// <summary>
		/// выводит на экран операнд выбранный пользователем
		/// </summary>
		/// <param name="tag">имя кнопки</param>
		private string ShowOperand(string tag)
		{
			display = this.txtDisplay.Text; //получаем текущее значение на дисплее
			switch (tag)
			{
				case "sign":
					if (display.StartsWith("-"))
						return display.Replace("-", "");
					else if (display == "0")
					{
						return display;
					}
					else
						return display = display.Insert(0, "-");
				case "clear":
					Operand1 = null;
					Operand2 = null;
					return display = "0";
				case "back":
					if (display.Length == 1) return display = "0";
					return display.Substring(0, display.Length - 1);
				case ",":
					if (display == "0" & tag != ",")
						return display = "";
					if (display == "0" & tag == ",")
						return display = "0,";
					if (display.Contains(",") & tag == ",")
						return display;
					break;
				default: // для всех цифровых кнопок
					if (display.Length > 13) return display;// если длина дисплея больше 15 символов вернуть дисплей
					if (display == "0") display = ""; // если на дисплее 0, очистить дисплей
					if (display == Operand1.ToString()) display = ""; // если на дисплее первый операнд, очистить дисплей
					break;
			}
			if (this.txtDisplay.Text == result)
			{
				Operand1 = null;
				Operand2 = null;
				display = null;
			}

			//if (Operand1 == Convert.ToDouble(result))
			//{
			//	Operand1 = null;
			//}

			return display += tag;
		}

		/// <summary>
		/// назначает значения операндам
		/// </summary>
		private void SetOperand()
		{
			curOperand = double.Parse(display);


			if (Operand1 == null && oper != null)
			{
				Operand1 = curOperand;
			}
			else
			{
				Operand2 = curOperand;
			}
			display = Operand1.ToString();
			this.txtDisplay.Text = display;
		}

		/// <summary>
		/// определяет тип операции
		/// </summary>
		/// <param name="tag">имя кнопки</param>
		private void SetOperator(string tag)
		{
			if (oper == null)
			{
				switch (tag)
				{
					case "add":
						oper = Operations.Add;
						break;
					case "substract":
						oper = Operations.Substract;
						break;
					case "multiply":
						oper = Operations.Multiplay;
						break;
					case "divide":
						oper = Operations.Divide;
						break;
				}
			}
			else if (oper != null)
			{
				oper = null;
				SetOperator(tag);
			}

			if (this.txtDisplay.Text == result)
			{
				Operand1 = Convert.ToDouble(result);
				Operand2 = null;
			}
		}

		/// <summary>
		/// возвращает результат операции в зависимости от выбранного оператора
		/// </summary>
		private void GetResult()
		{
			if (oper != null)
			{
				switch (oper)
				{
					case Operations.Add:
						result = (Operand1 + Operand2).ToString();
						break;
					case Operations.Substract:
						result = (Operand1 - Operand2).ToString();
						break;
					case Operations.Multiplay:
						result = (Operand1 * Operand2).ToString();
						break;
					case Operations.Divide:
						if (Operand2 == 0)
						{
							MessageBox.Show("Нельзя делить на 0");
							Operand1 = null;
							Operand2 = null;
							result = "0";
						}
						else
							result = (Operand1 / Operand2).ToString();
						break;
				}
			}
		}

		/// <summary>
		/// выводит на дисплей результат операции
		/// </summary>
		private void ShowResult()
		{
			double r = Convert.ToDouble(result);
			this.txtDisplay.Text = Math.Round(r, 6).ToString();
			oper = null;
		}
	}
}
