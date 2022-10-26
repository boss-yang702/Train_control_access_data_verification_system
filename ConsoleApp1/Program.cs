using System.Text.RegularExpressions;
//string s = "KAF15+545";
//string ss = Regex.Replace(s, "[a - zA-Z]", "", RegexOptions.IgnoreCase);
//string[] SA= ss.Split('+');
//int number = Convert.ToInt32(SA[0])*1000 + Convert.ToInt32(SA[1]);
//Console.WriteLine(number); 
string bh = Regex.Replace("105 - 3 - 04 - 005 - 3", "[ \n\r]", "", RegexOptions.IgnoreCase);
Match bh_pre = Regex.Match(bh, @"\d+-\d+-\d+", RegexOptions.IgnoreCase);
string s = bh_pre.Value;