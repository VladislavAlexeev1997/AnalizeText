using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizeText
{
    /*
     * Класс, предназначенный для сравнения элементов коллекции целых чисел.
     * Используется при определении равенства двух коллекций целых чисел.
     * Необходим для тестирования.
     */
    public class IntSequenceEqualComparer : IEqualityComparer<int>
    {
        public bool Equals(int first, int second)
        {
            return first == second;
        }

        public int GetHashCode(int obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

    /*
     * Класс, предназначенный для сравнения элементов коллекции вещественных чисел.
     * Используется при определении равенства двух коллекций вещественных чисел.
     * Необходим для тестирования.
     */
    public class DoubleSequenceEqualComparer : IEqualityComparer<double>
    {
        public bool Equals(double first, double second)
        {
            return first == second;
        }

        public int GetHashCode(double obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
