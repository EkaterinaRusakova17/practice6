using System;

namespace MatrixCalculator
{
    interface IComparable
    {
        int CompareTo(object obj);
    }

    class SquareMatrix : IComparable
    {
        private int[,] matrix;
        private int size;

        public SquareMatrix(int size)
        {
            this.size = size;
            matrix = new int[size, size];
        }

        public SquareMatrix(int[,] matrix)
        {
            size = matrix.GetLength(0);
            this.matrix = new int[size, size];
            Array.Copy(matrix, this.matrix, size * size);
        }

        public SquareMatrix(int size, bool isRandom)
        {
            this.size = size;
            matrix = new int[size, size];
            if (isRandom)
            {
                Random rnd = new Random();
                for (int index = 0; index < size; ++index)
                {
                    for (int secondIndex = 0; secondIndex < size; ++secondIndex)
                    {
                        matrix[index, secondIndex] = rnd.Next(10);
                    }
                }
            }
        }

        public int Size
        {
            get { return size; }
        }

        public int this[int index, int secondIndex]
        {
            get
            {
                if (index >= 0 && index < size && secondIndex >= 0 && secondIndex < size)
                {
                    return matrix[index, secondIndex];
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (index >= 0 && index < size && secondIndex >= 0 && secondIndex < size)
                {
                    matrix[index, secondIndex] = value;
                }
            }
        }

        public static SquareMatrix operator +(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.Size != matrix2.Size)
            {
                throw new ArgumentException("Матрицы должны иметь одинаковый размер.");
            }

            SquareMatrix result = new SquareMatrix(matrix1.Size);

            for (int index = 0; index < matrix1.Size; ++index)
            {
                for (int secondIndex = 0; secondIndex < matrix1.Size; ++secondIndex)
                {
                    result[index, secondIndex] = matrix1[index, secondIndex] + matrix2[index, secondIndex];
                }
            }

            return result;
        }

        public static SquareMatrix operator *(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.Size != matrix2.Size)
            {
                throw new ArgumentException("Матрицы должны иметь одинаковый размер.");
            }

            SquareMatrix result = new SquareMatrix(matrix1.Size);

            for (int index = 0; index < matrix1.Size; ++index)
            {
                for (int secondIndex = 0; secondIndex < matrix1.Size; ++secondIndex)
                {
                    for (int thirdIndex = 0; thirdIndex < matrix1.Size; ++thirdIndex)
                    {
                        result[index, secondIndex] += matrix1[index, thirdIndex] * matrix2[thirdIndex, secondIndex];
                    }
                }
            }

            return result;
        }

        public static bool operator >(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.Size != matrix2.Size)
            {
                throw new ArgumentException("Матрицы должны иметь одинаковый размер.");
            }

            int sum1 = 0;
            int sum2 = 0;

            for (int index = 0; index < matrix1.Size; ++index)
            {
                for (int secondIndex = 0; secondIndex < matrix1.Size; ++secondIndex)
                {
                    sum1 += matrix1[index, secondIndex];
                    sum2 += matrix2[index, secondIndex];
                }
            }

            return sum1 > sum2;
        }

        public static bool operator <(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            if (matrix1.Size != matrix2.Size)
            {
                throw new ArgumentException("Матрицы должны иметь одинаковый размер.");
            }

            int sum1 = 0;
            int sum2 = 0;

            for (int index = 0; index < matrix1.Size; ++index)
            {
                for (int secondIndex = 0; secondIndex < matrix1.Size; ++secondIndex)
                {
                    sum1 += matrix1[index, secondIndex];
                    sum2 += matrix2[index, secondIndex];
                }
            }

            return sum1 < sum2;
        }

        public void Transpose()
        {
            for (int index = 0; index < size; ++index)
            {
                for (int secondindex = index + 1; secondindex < size; ++secondindex)
                {
                    int temp = matrix[index, secondindex];
                    matrix[index, secondindex] = matrix[secondindex, index];
                    matrix[secondindex, index] = temp;
                }
            }
        }

        public int Trace()
        {
            int sum = 0;

            for (int index = 0; index < size; ++index)
            {
                sum += matrix[index, index];
            }

            return sum;
        }

        public int CompareTo(object obj)
        {
            SquareMatrix other = obj as SquareMatrix;

            if (other != null)
            {
                if (this.size != other.size)
                {
                    throw new ArgumentException("Матрицы должны иметь одинаковый размер.");
                }

                int thisSum = 0;
                int otherSum = 0;

                for (int index = 0; index < size; ++index)
                {
                    for (int secondindex = 0; secondindex < size; ++secondindex)
                    {
                        thisSum += matrix[index, secondindex];
                        otherSum += other[index, secondindex];
                    }
                }

                if (thisSum > otherSum)
                {
                    return 1;
                }
                else if (thisSum < otherSum)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                throw new ArgumentException("Невозможно сравнить объекты.");
            }
        }
    }

    class Program
    {
        delegate void Diagonalize(SquareMatrix matrix);

        static void Main(string[] args)
        {
            SquareMatrix matrix1 = new SquareMatrix(3, true);
            SquareMatrix matrix2 = new SquareMatrix(3, true);
            SquareMatrix matrix3 = matrix1 + matrix2;

            matrix3.Transpose();
            int trace = matrix3.Trace();

            Console.WriteLine("Матрица 1:");
            PrintMatrix(matrix1);
            Console.WriteLine("Матрица 2:");
            PrintMatrix(matrix2);
            Console.WriteLine("Сумма матриц:");
            PrintMatrix(matrix3);
            Console.WriteLine("Транспонированная матрица 3:");
            PrintMatrix(matrix3);
            Console.WriteLine("След матрицы 3: " + trace);

            Diagonalize diagonalizer = delegate (SquareMatrix matrix)
            {
                for (int index = 1; index < matrix.Size; ++index)
                {
                    for (int secondindex = 0; secondindex < index; ++secondindex)
                    {
                        matrix[index, secondindex] = 0;
                    }
                }
            };

            diagonalizer(matrix3);

            Console.WriteLine("Диагонализация матрицы 3:");
            PrintMatrix(matrix3);
        }

        static void PrintMatrix(SquareMatrix matrix)
        {
            for (int index = 0; index < matrix.Size; ++index)
            {
                for (int secondindex = 0; secondindex < matrix.Size; ++secondindex)
                {
                    Console.Write(matrix[index, secondindex] + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
