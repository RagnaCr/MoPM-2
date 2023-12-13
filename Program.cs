using System;
using System.Collections.Generic;

namespace MoPM
{
    public enum SortingType
    {
        BUBBLE,
        SHELL,
        MERGE,
        QUICK
    }

    public interface Sorter
    {
        List<int> Sort(List<int> input);
    }

    public class BubbleSorter : Sorter
    {
        public List<int> Sort(List<int> input)
        {
            for (int i = 0; i < input.Count - 1; i++)
            {
                for (int j = 0; j < input.Count - i - 1; j++)
                {
                    if (input[j] > input[j + 1])
                    {
                        int temp = input[j];
                        input[j] = input[j + 1];
                        input[j + 1] = temp;
                    }
                }
            }
            return input;
        }
    }


    public class ShellSorter : Sorter
    {
        public List<int> Sort(List<int> input)
        {
            int n = input.Count;

            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i += 1)
                {
                    int temp = input[i];

                    int j;
                    for (j = i; j >= gap && input[j - gap] > temp; j -= gap)
                    {
                        input[j] = input[j - gap];
                    }

                    input[j] = temp;
                }
            }
            return input;
        }
    }

    public class MergeSorter : Sorter
    {
        public List<int> Sort(List<int> input)
        {
            MergeSort(input);
            return input;
        }

        private void MergeSort(List<int> array)
        {
            int n = array.Count;
            if (n <= 1)
                return;

            int mid = n / 2;
            List<int> left = new List<int>(array.GetRange(0, mid));
            List<int> right = new List<int>(array.GetRange(mid, n - mid));

            MergeSort(left);
            MergeSort(right);

            Merge(array, left, right);
        }

        private void Merge(List<int> array, List<int> left, List<int> right)
        {
            int leftSize = left.Count;
            int rightSize = right.Count;
            int i = 0, j = 0, k = 0;

            while (i < leftSize && j < rightSize)
            {
                if (left[i] <= right[j])
                {
                    array[k] = left[i];
                    i++;
                }
                else
                {
                    array[k] = right[j];
                    j++;
                }
                k++;
            }

            while (i < leftSize)
            {
                array[k] = left[i];
                i++;
                k++;
            }

            while (j < rightSize)
            {
                array[k] = right[j];
                j++;
                k++;
            }
        }
    }


    public class QuickSorter : Sorter
    {
        public List<int> Sort(List<int> input)
        {
            QuickSort(input, 0, input.Count - 1);
            return input;
        }

        private void QuickSort(List<int> array, int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(array, low, high);

                QuickSort(array, low, partitionIndex - 1);
                QuickSort(array, partitionIndex + 1, high);
            }
        }

        private int Partition(List<int> array, int low, int high)
        {
            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            Swap(array, i + 1, high);
            return i + 1;
        }

        private void Swap(List<int> array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }


    class Program
    {
        static void Main()
        {
            foreach (int count in new[] { 10, 1000, 10000, 1000000 })
            {
                Console.WriteLine($"-----------| Number of elements: {count} |-----------\n");
                List<int> array = GenerateArray(count);

                foreach (SortingType type in Enum.GetValues(typeof(SortingType)))
                {
                    Console.WriteLine($"Sorting type: {type}");
                    Sorter sorter = GetSorter(type);
                    MeasureTime(array, sorter);
                }
            }
        }

        static List<int> GenerateArray(int count)
        {
            Random random = new Random();
            List<int> array = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                array.Add(random.Next(0, count));
            }
            return array;
        }

        static Sorter GetSorter(SortingType type)
        {
            switch (type)
            {
                case SortingType.BUBBLE:
                    return new BubbleSorter();
                case SortingType.SHELL:
                    return new ShellSorter();
                case SortingType.MERGE:
                    return new MergeSorter();
                case SortingType.QUICK:
                    return new QuickSorter();
                default:
                    return new BubbleSorter();
            }
        }

        static void MeasureTime(List<int> array, Sorter sorter)
        {
            List<int> copyArray = new List<int>(array);

            DateTime startTime = DateTime.Now;
            sorter.Sort(copyArray);
            DateTime endTime = DateTime.Now;

            Console.WriteLine($"Execution time: {(endTime - startTime).TotalMilliseconds} ms");

            // Виведення перших 50 елементів відсортованого масиву в одному рядку
            Console.Write($"Sorted array: ");
            for (int i = 0; i < Math.Min(50, copyArray.Count); i++)
            {
                Console.Write($"{copyArray[i]}");
                if (i < Math.Min(50, copyArray.Count) - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine("\n");
        }
    }
}