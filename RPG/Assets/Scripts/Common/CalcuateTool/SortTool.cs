using System;
using System.Collections.Generic;

namespace Common.CalcuateTool
{
    public static class SortTool
    {
        public static void Sort<T>(List<T> list, Func<T, T, int> compareFunc)
        {
            QuickSort(list, compareFunc);
        }

        // 快速排序
        public static void QuickSort<T>(List<T> list, Func<T, T, int> compareFunc)
        {
            if (list == null || list.Count <= 1 || compareFunc == null)
            {
                return;
            }

            QuickSortInternal(list, 0, list.Count - 1, compareFunc);
        }

        private static void QuickSortInternal<T>(List<T> list, int left, int right, Func<T, T, int> compareFunc)
        {
            if (left < right)
            {
                int pivotIndex = Partition(list, left, right, compareFunc);
                QuickSortInternal(list, left, pivotIndex - 1, compareFunc);
                QuickSortInternal(list, pivotIndex + 1, right, compareFunc);
            }
        }

        private static int Partition<T>(List<T> list, int left, int right, Func<T, T, int> compareFunc)
        {
            T pivot = list[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (compareFunc(list[j], pivot) < 0)
                {
                    i++;
                    // Swap elements
                    T temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }

            // Swap the pivot element with the element at i + 1
            T tempPivot = list[i + 1];
            list[i + 1] = list[right];
            list[right] = tempPivot;

            return i + 1;
        }
    }
}