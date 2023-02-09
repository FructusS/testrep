using Avalonia.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Avalonia.Interactivity;
using System.Linq;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        List<String> operationInString = new List<String>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClickCalc(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) {
                Input.Text += btn.Content;
            }
        }

        
        private void ClickStartCalcul(object sender, RoutedEventArgs e)
        {
            operationInString.Clear();

            ParsStringToList(Input.Text.ToCharArray());

            GetAnswer();

            WindrawAnswer();
        }

        // ������� ������ �� �������� � ��������� ������ ������� (� ������ ������� ����� ���������, ����� �������� �� �����, � �����), ����� � �������� �������� �������� 123123123
        public void ParsStringToList(char[] stringForPars)
        {
            // ��� �������� ���������� ������ ����� ��� ���
            bool previousSimbol = true;
            // ��� �������� �������� ������
            Stack<String> stackSimbol = new Stack<String>();

            for (int i = 0; i < stringForPars.Length; i++)
            {
                String lastInStack = "";
                if (stackSimbol.Count != 0)
                {
                    lastInStack = stackSimbol.Peek();
                }
                switch (stringForPars[i])
                {
                    case ('+'):
                        if (lastInStack == "-" || lastInStack == "+" || lastInStack == "*" || lastInStack == "/" || lastInStack == "^")
                        {
                            operationInString.Add(stackSimbol.Pop());
                        }
                        stackSimbol.Push("+");
                        previousSimbol = true;
                        break;
                    case ('-'):
                        if (lastInStack == "-" || lastInStack == "+" || lastInStack == "*" || lastInStack == "/" || lastInStack == "^")
                        {
                            operationInString.Add(stackSimbol.Pop());
                        }
                        stackSimbol.Push("-");
                        previousSimbol = true;
                        break;
                    case ('*'):
                        if (lastInStack == "*" || lastInStack == "/" || lastInStack == "^")
                        {
                            operationInString.Add(stackSimbol.Pop());
                        }
                        stackSimbol.Push("*");
                        previousSimbol = true;
                        break;
                    case ('/'):
                        if (lastInStack == "*" || lastInStack == "/" || lastInStack == "^")
                        {
                            operationInString.Add(stackSimbol.Pop());
                        }
                        stackSimbol.Push("/");
                        previousSimbol = true;
                        break;
                    case ('^'):
                        if (lastInStack == "^")
                        {
                            operationInString.Add(stackSimbol.Pop());
                        }
                        stackSimbol.Push("^");
                        previousSimbol = true;
                        break;
                    case ('('):
                        stackSimbol.Push("(");
                        previousSimbol = true;
                        break;
                    case (')'):
                        // �������� ���� �� ����� ������ ������
                        bool stopPriority = true;
                        // ��������, ��� ����� ����������� ������ ��� ������ ����������� ������
                        try
                        {
                            // ������ ��� �������� � �����, ���� �� �������� ����������� ������
                            while (stopPriority)
                            {
                                if (stackSimbol.Peek() == "(")
                                {
                                    stackSimbol.Pop();
                                    stopPriority = false;
                                }
                                else
                                {
                                    operationInString.Add(stackSimbol.Pop());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Answer.Text = "�� ������� ������ ����������� ������";
                            i = stringForPars.Length;
                            break;
                        }
                        previousSimbol = true;
                        break;
                    case (' '):
                        break;
                    default:
                        if (previousSimbol)
                        {
                            // ���� ���������� ������ �� �����, �������� ������ �����
                            operationInString.Add(char.ToString(stringForPars[i]));
                            previousSimbol = false;
                        }
                        else
                        {
                            // ���� ����������� ������ �����, �������� ���� ������� � �������� � ���� ����� �����
                            int idLastElem = operationInString.Count - 1;

                            String newLast = operationInString[idLastElem] + stringForPars[i];
                            operationInString.RemoveAt(idLastElem);
                            operationInString.Add(newLast);
                            int l = 0;
                        }
                        break;
                }
            }
            // ����� ����� ���������� �� ����� ��������� ��������
            while (true)
            {
                if (stackSimbol.Count == 0)
                {
                    break;
                }
                else
                {
                    operationInString.Add(stackSimbol.Pop());
                }
            }
        }

        // ������� �������, ������� ����� � ����� operationInString
        public void GetAnswer()
        {
            Stack<String> result = new Stack<String>();

            for (int i = 0; i < operationInString.Count; i++)
            {
                String elem = operationInString[i];
                if (elem == "+" || elem == "-" || elem == "*" || elem == "/" || elem == "^")
                {
                    // ����� �� ������� ���������
                    try
                    {
                        double secondOperand = Double.Parse(result.Pop());
                        double firstOperand = Double.Parse(result.Pop());
                        double res;

                        switch (elem)
                        {
                            case ("+"):
                                res = firstOperand + secondOperand;
                                result.Push(res + "");
                                break;
                            case ("-"):
                                res = firstOperand - secondOperand;
                                result.Push(res + "");
                                break;
                            case ("*"):
                                res = firstOperand * secondOperand;
                                result.Push(res + "");
                                break;
                            case ("/"):
                                res = firstOperand / secondOperand;
                                result.Push(res + "");
                                break;
                            case ("^"):
                                res = Math.Pow(firstOperand, secondOperand);
                                result.Push(res + "");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Answer.Text = "�� ������� ���������";
                        break;
                    }
                }
                else
                {
                    result.Push(elem);
                }
            }

            operationInString.Clear();

            foreach (String ele in result)
            {
                operationInString.Add(ele);
            }
        }

        public void WindrawAnswer()
        {
            String windrawData = "";
            foreach (String elem in operationInString)
            {
                windrawData += elem;
            }

            Answer.Text = windrawData;
        }

        private void ClickDelete(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Content)
                {
                    case "C--":
                        Input.Clear();
                        break;
                    case "<--":
                        Input.Text = Input.Text.Substring(0, Input.Text.Length - 1);
                        break;
                    default:
                        Input.Text = btn.Content.ToString();
                        break;
                }
            }
        }
    }
}
