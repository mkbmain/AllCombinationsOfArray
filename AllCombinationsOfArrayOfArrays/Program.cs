using System;
using System.Collections.Generic;
using System.Linq;

namespace AllCombinationsOfArrayOfArrays
{
    class Program
    {

        public class Results<T>
        {
            public T Value { get; set; }
            public List<Results<T>> ResultsList { get; set; }
        }

        private static T[][] AllInOne<T>(params T[][] items)
        {
            var results = throughArrays(items);
            return FlatResult<T>(results);
        }

        private static List<Results<T>> throughArrays<T>(params T[][] items)
        {
            var list = new List<Results<T>>();
            foreach (var t in items)
            {
                list.AddRange(t.Select(item => new Results<T>()
                    {Value = item, ResultsList = throughArrays(items.Skip(1).ToArray())}));
                break;
            }

            return list.Any() ? list : null;
        }
        private static T[][] FlatResult<T>(List<Results<T>> items)
        {
            List<T[]> list = new List<T[]>();
            if(items==null){return list.Select(f=>f).ToArray();}

            foreach (var item in items)
            {
                list.Add(new T[]{item.Value});
                var res = FlatResult<T>(item.ResultsList);

                foreach (var r in res)
                {
                    list.Add( FlatMe(item.Value, r));  
                }
            }
            var b = list.Select(f => f).GroupBy(f => f.Length).OrderByDescending(x => x.Key)
                .ToDictionary(x=>x.Key,x=>x.ToArray()).Take(1).SelectMany(x=>x.Value).ToArray();

            return b;
        }


        private static T[] FlatMe<T>(T item, T[] tail)
        {
            tail = tail.Where(f => f != null).ToArray();
            var ar = new T[tail.Length + 1];
            ar[0] = item;
            for (int i = 0; i < tail.Length; i++)
            {
                ar[i + 1] = tail[i];
            }

            return ar;
        }



        static void Main(string[] args)
        {
         
            var maxResult = AllInOne<int>(Enumerable.Range(1, 10).ToArray(),
                Enumerable.Range(1, 10).ToArray(), Enumerable.Range(1, 5).ToArray(),
                Enumerable.Range(1, 5).ToArray(), Enumerable.Range(1, 15).ToArray(),
                Enumerable.Range(1, 5).ToArray(), Enumerable.Range(3, 6).ToArray()
            );
            var result = AllInOne(new[] {1, 2,3}, new[] {1,2,3}, new[] { 1,2,3});
            
            
            var result2 = AllInOne(new[] {"S", "M", "L", "XL", "XXL"}, new[] {"Black", "Red", "blue"},
                new[] {"Men", "Women"});
            
        }
    }
}