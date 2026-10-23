using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab5_6
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            try { Console.InputEncoding = System.Text.Encoding.UTF8; } catch { }

            int choice = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Работа с двумерным массивом");
                Console.WriteLine("2. Работа с рваным массивом");
                Console.WriteLine("3. Работа со строкой");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт меню: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Ошибка! Введите число.");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1: Menu2D(); break;
                        case 2: MenuJagged(); break;
                        case 3: MenuString(); break;
                        case 0: Console.WriteLine("\nВыход..."); break;
                        default:
                            Console.WriteLine("\nНеверный пункт.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nПроизошла ошибка: {ex.Message}");
                    Console.ReadKey();
                }

            } while (choice != 0);
        }

        static void PrintHeaders(params string[] headers)
        {
            foreach (string h in headers)
                Console.WriteLine("     " + h);
        }
        static int ReadInt(string prompt, int minValue = int.MinValue)
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(prompt))
                    Console.Write(prompt);
                string s = Console.ReadLine();
                if (int.TryParse(s, out int value) && value >= minValue)
                    return value;
                Console.WriteLine("Ошибка! Введите корректное целое число.");
            }
        }

        static int FindMaxRecursive(int[] arr, int index = 0)
        {
            if (arr == null || arr.Length == 0)
                return int.MinValue;
            if (index == arr.Length - 1)
                return arr[index];
            int maxRest = FindMaxRecursive(arr, index + 1);
            return arr[index] > maxRest ? arr[index] : maxRest;
        }
        static void Menu2D()
        {
            Console.Clear();
            PrintHeaders("ДВУМЕРНЫЙ МАССИВ", "Задание: добавить строку после строки с max элементом");

            Console.WriteLine("1. Заполнить случайными числами");
            Console.WriteLine("2. Заполнить вручную");
            Console.Write("Выбор: ");
            int mode = ReadInt("", 1);
            bool fillRandom = (mode == 1);

            int[,] mat = CreateMatrix(fillRandom);
            Console.WriteLine("\nИсходная матрица:");
            PrintMatrix(mat);

            int[,] result = AddRowAfterMax(mat);
            Console.WriteLine("\nМатрица после добавления строки:");
            PrintMatrix(result);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static int[,] CreateMatrix(bool fillRandom = true)
        {
            int rows = ReadInt("Введите количество строк: ", 1);
            int cols = ReadInt("Введите количество столбцов: ", 1);

            int[,] mat = new int[rows, cols];

            if (fillRandom)
            {
                Random rnd = new Random();
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        mat[i, j] = rnd.Next(1, 100);
                Console.WriteLine("Матрица заполнена случайными числами.");
            }
            else
            {
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                    {
                        Console.Write($"mat[{i},{j}] = ");
                        mat[i, j] = ReadInt("", int.MinValue);
                    }
            }
            return mat;
        }

        static void PrintMatrix(int[,] mat)
        {
            if (mat == null || mat.Length == 0)
            {
                Console.WriteLine("Матрица пуста.");
                return;
            }
            int rows = mat.GetLength(0);
            int cols = mat.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write($"{mat[i, j],5}");
                Console.WriteLine();
            }
        }

        static void PrintMatrix(int[][] jag)
        {
            if (jag == null || jag.Length == 0)
            {
                Console.WriteLine("Массив пуст.");
                return;
            }
            for (int i = 0; i < jag.Length; i++)
            {
                if (jag[i] == null || jag[i].Length == 0)
                {
                    Console.WriteLine("(пустая строка)");
                    continue;
                }
                Console.WriteLine(string.Join(" ", jag[i]));
            }
        }

        static int[,] AddRowAfterMax(int[,] mat)
        {
            if (mat == null || mat.Length == 0)
            {
                Console.WriteLine("Ошибка: матрица пуста, добавить строку нельзя.");
                return mat;
            }

            int rows = mat.GetLength(0);
            int cols = mat.GetLength(1);

            int maxI = 0, maxJ = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (mat[i, j] > mat[maxI, maxJ]) { maxI = i; maxJ = j; }

            Console.WriteLine($"\nНаибольший элемент: {mat[maxI, maxJ]} в строке {maxI}");

            int[,] result = new int[rows + 1, cols];

            for (int i = 0; i <= maxI; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = mat[i, j];

            for (int j = 0; j < cols; j++)
                result[maxI + 1, j] = 0;

            for (int i = maxI + 1; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i + 1, j] = mat[i, j];

            return result;
        }

        static void MenuJagged()
        {
            Console.Clear();
            PrintHeaders("РВАНЫЙ МАССИВ", "Задание: добавить строку в начало");

            Console.WriteLine("1. Заполнить случайными числами");
            Console.WriteLine("2. Заполнить вручную");
            Console.Write("Выбор: ");
            int mode = ReadInt("", 1);
            bool fillRandom = (mode == 1);

            int[][] jag = CreateJagged(fillRandom);
            Console.WriteLine("\nИсходный рваный массив:");
            PrintMatrix(jag);

            int globalMax = int.MinValue;
            foreach (int[] row in jag)
            {
                if (row.Length > 0)
                {
                    int rowMax = FindMaxRecursive(row);
                    if (rowMax > globalMax) globalMax = rowMax;
                }
            }
            if (globalMax != int.MinValue)
                Console.WriteLine($"\nМаксимальный элемент (рекурсивно): {globalMax}");

            int[][] result = AddRowToStart(jag);
            Console.WriteLine("\nПосле добавления строки в начало:");
            PrintMatrix(result);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static int[][] CreateJagged(bool fillRandom = true)
        {
            int rows = ReadInt("Введите количество строк: ", 1);
            int[][] jag = new int[rows][];
            Random rnd = new Random();

            for (int i = 0; i < rows; i++)
            {
                int cols = ReadInt($"Введите количество элементов в строке {i}: ", 1);
                jag[i] = new int[cols];

                if (fillRandom)
                {
                    for (int j = 0; j < cols; j++)
                        jag[i][j] = rnd.Next(1, 100);
                }
                else
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Console.Write($"  jag[{i}][{j}] = ");
                        jag[i][j] = ReadInt("", int.MinValue);
                    }
                }
            }
            return jag;
        }
        static int[][] AddRowToStart(int[][] jag)
        {
            int newLen = (jag == null ? 1 : jag.Length + 1);
            int[][] result = new int[newLen][];

            result[0] = new int[] { 0, 0, 0 };

            if (jag != null)
                for (int i = 0; i < jag.Length; i++)
                    result[i + 1] = jag[i];

            return result;
        }
        static void MenuString()
        {
            Console.Clear();
            PrintHeaders("РАБОТА СО СТРОКОЙ", "Задание: перевернуть слова и отсортировать по длине");

            Console.WriteLine("1. Ввести строку с клавиатуры");
            Console.WriteLine("2. Взять тестовую строку");
            Console.Write("Выбор: ");
            int mode = ReadInt("", 1);

            string input;
            if (mode == 1)
            {
                Console.WriteLine("Введите строку:");
                input = Console.ReadLine();
            }
            else
            {
                string[] testStrings = {
                    "В лесу родилась елочка! В лесу она росла.",
                    "Зимой и летом стройная, зеленая была.",
                };
                Random rnd = new Random();
                input = testStrings[rnd.Next(testStrings.Length)];
                Console.WriteLine("Тестовая строка:");
                Console.WriteLine(input);
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Строка пуста — обрабатывать нечего.");
                Console.ReadKey();
                return;
            }

            string result = ProcessString(input);
            Console.WriteLine("\nРезультат:");
            Console.WriteLine(result);

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static string ProcessString(string text)
        {
            MatchCollection matches = Regex.Matches(text, @"[^.!?]+[.!?]?");
            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < matches.Count; k++)
            {
                if (k > 0) sb.Append(" ");
                sb.Append(ProcessSentence(matches[k].Value));
            }
            return sb.ToString();
        }
        static string ProcessSentence(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence)) return "";

            char endChar = '\0';
            string trimmed = sentence.TrimEnd();

            if (trimmed.Length > 0 && ".!?".Contains(trimmed[^1]))
            {
                endChar = trimmed[^1];
                trimmed = trimmed.Substring(0, trimmed.Length - 1);
            }

            string[] words = trimmed.Split(new char[] { ' ', ',', ';', ':' },
                StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
                return endChar != '\0' ? endChar.ToString() : "";

            for (int i = 0; i < words.Length; i++)
            {
                char[] chars = words[i].ToCharArray();
                Array.Reverse(chars);
                words[i] = new string(chars);
            }

            words = words.OrderByDescending(w => w.Length).ToArray();

            return string.Join(" ", words) + (endChar != '\0' ? endChar.ToString() : "");
        }
    }
}