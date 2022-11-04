using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;
namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
 
            //string s = "KAF15+545";
            //string ss = Regex.Replace(s, "[a - zA-Z]", "", RegexOptions.IgnoreCase);
            //string[] SA= ss.Split('+');
            //int number = Convert.ToInt32(SA[0])*1000 + Convert.ToInt32(SA[1]);
            //Console.WriteLine(number); 
            //string bh = Regex.Replace("105 - 3 - 04 - 005 - 3", "[ \n\r]", "", RegexOptions.IgnoreCase);
            //Match bh_pre = Regex.Match(bh, @"\d+-\d+-\d+", RegexOptions.IgnoreCase);
            //string s = bh_pre.Value;
            //string input = "(1/3)";
            //string[] ss = Regex.Replace(input,@"[\(\)]","").Split('/');
            //Console.WriteLine(ss[0]+ss[1]);

            foreach(char item in "XII")//老师给的表格
            {
                Console.Write(Convert.ToInt32(item)+" ");
            }
            Console.WriteLine();

            foreach (char item in "XⅡ")//李姐的罗马数字
            {
                Console.Write(Convert.ToInt32(item)+" ");
            }
            Console.ReadKey();
        }
    }
}
