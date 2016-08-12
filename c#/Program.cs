using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApplication
{
        
  public class Program
  {
    public static IEnumerable<long> NaturalNumbers()
    {
      var i = 0L;
      while (true) 
      {
        yield return i++;
      }
    }

    public static long Cube(long x){
      return x*x*x;
    }

    // Merges 2 infinite sequences
    public static IEnumerable<long> Merge(IEnumerable<long> la, IEnumerable<long> lb) 
    {
      var ea = la.GetEnumerator();
      var eb = lb.GetEnumerator();

      ea.MoveNext();
      eb.MoveNext();
      while (true) 
      {
        if (ea.Current < eb.Current) 
        {
          yield return ea.Current;
          ea.MoveNext();
        }
        else 
        {
          yield return eb.Current;
          eb.MoveNext();
        }
      }
    }

    // Takes matrix with the sum of cubes of 2 numbers and returns a sorted version of it 
    //   | 1    2    3    4    5
    //--------------------------
    // 1 | 2    9   28   65  126
    // 2 |      16  35   72  133
    // 3 |          54   91  152
    // 4 |              128  189
    // 5 |                   250
    //
    // returns a sorted list of with the some of cubes for pairs { (a,b) |  a >= i, b >= j }  
    public static IEnumerable<long> SumOfCubePairsSorted(int i, int j) 
    {
      yield return Cube(i) + Cube(j);

      var restOfResult = Merge(NaturalNumbers().Skip(i+1).Select(n => Cube(n) + Cube(j)), SumOfCubePairsSorted(i+1, j+1));

      foreach (var item in restOfResult)
      {
        yield return item;
      }
    }

    public static void Main(string[] args)
    {
      // NaturalNumbers().Skip(1).Take(5).Select(Cube).ToList().ForEach(Console.WriteLine);
      // Merge(NaturalNumbers().Skip(1).Take(5).Select(Cube), NaturalNumbers()).Take(40).ToList().ForEach(Console.WriteLine);
      // SumOfCubePairsSorted(1, 1).Take(20).ToList().ForEach(Console.WriteLine);
      var ta2 = Enumerable
                  .Zip(SumOfCubePairsSorted(1, 1), 
                      SumOfCubePairsSorted(1, 1).Skip(1),
                      (a, b) => new {A=a, B=b}) // no tuples here :(
                  .Where(x => x.A == x.B) 
                  .Select(x => x.A);

      Console.WriteLine("Ta(2) = [{0}, ...]", String.Join(", ", ta2.Take(20).Select(x => x.ToString())));
    }
  }
}
