



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace _24
{




    public partial class MainWindow : Window
    {


        enum type { none, equal, pocker, left, right, symbol ,result};
  
        String text;
        Button button;
        int[] pocker_number;
        int left_count;
        int right_count;



        public MainWindow()
        {
            InitializeComponent();
  
            pocker_number = new int[4] { 0, 0, 0, 0 };
            text = "";
        }



        private void start_Click(object sender, RoutedEventArgs e)
        {
            button = (Button)sender;
            button.Content = "Restart";
            screen.Text = "";
            text = "";
            left_count = 0;
            right_count = 0;
            reset();
        }




        private void clear_Click(object sender, RoutedEventArgs e)
        {
            screen.Text = "";
            text = "";
            left_count = 0;
            right_count = 0;
            for (int j = 'A'; j <= 'D'; j++)
                pocker_change(j, pocker_number[j - 'A']);

        }




        private void back_Click(object sender, RoutedEventArgs e)
        {
            type tp;
            tp = type_check();
            
            
            if (tp == type.result)
            {
                MessageBox.Show("You need to choose Restart or Clear!");
                return;
            }
            if (text.Length > 0)
            {

                char ch = text[text.Length - 1];
                if ('A' <= ch && ch <= 'D')
                {
                    int number = pocker_number[ch - 'A'];
                    pocker_change(ch, number);
                    if (number >= 10)
                    {
                        screen.Text = screen.Text.Substring(0, screen.Text.Length - 2);
                        text = text.Substring(0, text.Length - 1);
                        return;
                    }
                }
                else if (ch == '(') left_count--;
                else if (ch == ')') right_count--;
                screen.Text = screen.Text.Substring(0, screen.Text.Length - 1);
                text = text.Substring(0, text.Length - 1);
            }
            else MessageBox.Show("wrong format!");

        }





        private void symbol_Click(object sender, RoutedEventArgs e)
        {
            button = (Button)sender;
            type tp;
            tp = type_check();
            if (tp == type.result)
            {
                MessageBox.Show("You need to choose Restart or Clear!");
                return;
            }
            if (tp == type.pocker || tp == type.right)
            {
      
                screen.Text += button.Content;
                text += button.Content;
            }
            else MessageBox.Show("wrong format!");
        }





        private void pocker_Click(object sender, RoutedEventArgs e)
        {
            if (pocker_number[0] == 0)
            {
                MessageBox.Show("You need to shuffle first !");
                return;
            }
            button = (Button)sender;
            string name = button.Name;
            char id = name[name.Length - 1];
          
            type tp;
            tp = type_check();
            if (tp == type.result)
            {
                MessageBox.Show("You need to choose Restart or Clear!");
                return;
            }
            if (tp == type.none || tp == type.left || tp == type.symbol)
            {

  
                screen.Text += pocker_number[id - 'A'];
                text += id;
                pocker_change(id, 0);
            }
            else MessageBox.Show("wrong format!");
        }





        private void left_Click(object sender, RoutedEventArgs e)
        {

            type tp;
            tp = type_check();

            if (tp == type.result)
            {
                MessageBox.Show("You need to choose Restart or Clear!");
                return;
            }
            if (tp == type.none || tp == type.left || tp == type.symbol)
            {
    
                screen.Text += '(';
                text += '(';
                left_count++;

            }
            else MessageBox.Show("wrong format!");
        }


        private void right_Click(object sender, RoutedEventArgs e)
        {
            type tp;
            tp = type_check();
            if (tp == type.result)
            {
                MessageBox.Show("You need to choose Restart or Clear!");
                return;
            }
            if (left_count > right_count)
            {
               
                if (tp == type.none || tp == type.right || tp == type.pocker)
                {
                    right_count++;
      
                    screen.Text += ')';
                    text += ')';

                }
            }
            else MessageBox.Show("wrong format!");
        }

        private void equal_Click(object sender, RoutedEventArgs e)
        {
            if(!(text.Contains('A')&& text.Contains('A') && text.Contains('A') && text.Contains('A') ))
            {
                MessageBox.Show("You haven't chosen all the pockers!");
                return;
            }
            if (text.Length != 0&& left_count == right_count)
            {
                type tp;
                tp = type_check();
                if (tp == type.result)
                {
                    MessageBox.Show("You need to choose Restart or Clear!");
                    return;
                }
                if (tp == type.pocker || tp == type.right)
                {
               
                    int result = Calculate();
                    if (result == 24) text=screen.Text = "Congratulation! You win!";
                
                    else text=screen.Text = "Sorry, your answer is " + Convert.ToString(result);
                     
                }
                else MessageBox.Show("wrong format!");
            }
            else MessageBox.Show("wrong format!");
        }


        private int Calculate()
        {

            Stack<char> operStack = new Stack<char>();
            operStack.Push('#');
            string exp = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(')
                    operStack.Push('(');
                else if (text[i] == ')')
                    while (operStack.Peek() != '(')
                    {
                        exp += operStack.Pop();
                     
                    }

                else if (text[i] == 'A' || text[i] == 'B' || text[i] == 'C' || text[i] == 'D')
                    exp += text[i];
                else if (priority(text[i]) > priority(operStack.Peek()))
                    operStack.Push(text[i]);
                else if (priority(text[i]) <= priority(operStack.Peek())|| operStack.Peek()=='(')
                { exp += operStack.Pop(); i--; }
             
            }

            while (operStack.Peek() != '#')
            {
                exp += operStack.Pop();
            }
            Stack<int> numberStack = new Stack<int>();

            for (int i = 0; i < exp.Length; i++)
            {
                if (exp[i] == 'A' || exp[i] == 'B' || exp[i] == 'C' || exp[i] == 'D')
                    numberStack.Push(pocker_number[exp[i] - 'A']);
                else
                {
                    int cal;
                    switch (exp[i])
                    {
                        case '+': cal = numberStack.Pop() + numberStack.Pop(); numberStack.Push(cal); break;
                        case '-': cal = numberStack.Pop() - numberStack.Pop(); numberStack.Push(-cal); break;
                        case 'x': cal = numberStack.Pop() * numberStack.Pop(); numberStack.Push(cal); break;
                        case '/': cal = numberStack.Pop() / numberStack.Pop(); numberStack.Push(cal); break;
                    }
                }
            }
            return numberStack.Peek();
        }


        

        private int priority(char ch)
        {
            switch (ch)
            {
                case '#': 
                case '(': 
                case ')': return 1;
                case '+':
                case '-': return 2;
                case 'x':
                case '/': return 3;

                default: return 0;
            }
        }




        private void reset()
        {
            Random random = new Random();
            int site = 13;
            int[] index = new int[14];
            for (int i = 0; i < 14; i++)
                index[i] = i;

            int result;
            for (int j = 'A'; j <='D'; j++)
            {
                result = random.Next(1, site);
                pocker_change(j, index[result]);
                pocker_number[j - 'A'] =index[result];
                index[result] = index[site];
                site--;
            }
        }



        private void pocker_change(int id, int number)
        {
            ImageBrush imageBrush= new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("image/"+Convert.ToString(number) + ".jpg", UriKind.Relative));

            switch (id)
            {
                case 'A': pockerA.Background = imageBrush; break;
                case 'B': pockerB.Background = imageBrush; break;
                case 'C': pockerC.Background = imageBrush; break;
                case 'D': pockerD.Background = imageBrush; break;
            }

        }


        private type type_check()
        {
            if (text.Length == 0) return type.none;

            char ch = text[text.Length - 1];
            switch (ch)
            { case 'x':
              case '/':
              case '+':
              case '-': return type.symbol;
              case 'A':
              case 'B':
              case 'C':
              case 'D':  return type.pocker;
              case '(': return type.left;
              case ')': return type.right;
              default: return type.result;
             }

        }
    }
}








    

















