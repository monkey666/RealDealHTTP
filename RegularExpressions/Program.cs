using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            // todo; a way/method to get all correct strings from a given pattern
           Match m1 = Regex.Match ("One color? There are two colours in my head!", @"colou?rs?");
            // colour colours color colors
            // coloup?o?r
           Console.WriteLine(Regex.IsMatch("color", @"colou?r")); // True
           Console.WriteLine(Regex.IsMatch("colour", @"colou?r")); // True
           Console.WriteLine(Regex.IsMatch("colouur", @"colou?r"));

           Console.WriteLine(Regex.Match("aaa", @"(?i)aaa(?-i)a").Success);
           Console.WriteLine(Regex.Match("Aa", @"(?i)a(?-i)a").Success);

           Console.WriteLine(Regex.Match("what?", @"what?")); // what (incorrect)
           //Console.WriteLine(Regex.Match("AA", @"(?i) a (?-i)a", RegexOptions.IgnorePatternWhitespace ).Success);

           Console.WriteLine(Regex.Match("what?", @"what?")); // what (incorrect)

           Console.WriteLine(Regex.Escape(@"\ * + ? | { [ () ^ $ . #?")); // \?

           Console.WriteLine(Regex.Unescape(@"\?"));

           Console.WriteLine(@"hello\n");

           Regex bp = new Regex(@"^\d{2,3}/\d{2,3}$");
           Console.WriteLine(bp.Match("160/110")); // 160/110

           //Console.WriteLine(Regex.Matches("That is that hat.", @"[Tt]hat").Count);


           //Console.WriteLine(Regex.Matches("Ьhat", @"[^a-zA-Z_0-9]hat", RegexOptions.CultureInvariant).Count);



            // j\\j  -> \

           //Console.WriteLine(Regex.Match("j\\j", @"\\").Success);

            // 3 modes
            // non verb ->   \\ -> escape \ = \
            // verb -> \ -> \
            // reg -> \ -> escape  -> error
            // 
            // \\\\ -> escape \ escape \ -> \\
            // \\ -> \\
            // \\ -> escape \ -> \

            

         //  Console.WriteLine(m1);
         //  Console.WriteLine(m1.ToString());

           
         //  Console.WriteLine(Regex.Matches("Jennynifer", "Jen(ny|nifer)?").Count); // True
        //   Console.WriteLine(Regex.IsMatch("Jennynifer", "Jennifer")); // false
        //   Console.WriteLine(Regex.IsMatch("Jennynifer", "Jenny")); // True

            Console.ReadKey();

        }
    }
}
