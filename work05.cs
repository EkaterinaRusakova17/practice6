using System;

namespace MatrixCalculator
{
    class ChainApplication
    {
        private MatrixHandler firstHandler;

        public ChainApplication()
        {
            firstHandler = new TransposeHandler();
            firstHandler.SetSuccessor(new TraceHandler());
        }

        public void ProcessRequest(SquareMatrix matrix)
        {
            Console.WriteLine("Выберите операцию:");
            Console.WriteLine("1. Транспонировать матрицу");
            Console.WriteLine("2. Вычислить след матрицы");
            Console.WriteLine("3. Дагонализировать матрицу");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    firstHandler.HandleRequest(matrix, "transpose");
                    break;
                case "2":
                    firstHandler.HandleRequest(matrix, "trace");
                    break;
                case "3":
                    firstHandler.HandleRequest(matrix, "diagonalize");
                    break;
                default:
                    Console.WriteLine("Запрос не обработан");
                    break;
            }
        }
    }
  

    class TransposeHandler : MatrixHandler
    {
        public override void HandleRequest(SquareMatrix matrix, string operation)
        {
            if (operation == "transpose")
            {
                matrix.Transpose();
                Console.WriteLine("Транспонированная матрица:");
                PrintMatrix(matrix);
            }

            else if (successor != null)
            {
                successor.HandleRequest(matrix, operation);
            }
        }

        private void PrintMatrix(SquareMatrix matrix)
        {
            for (int index = 0; index < matrix.Size; ++index)
            {
                for (int secondIndex = 0; secondIndex < matrix.Size; secondIndex++)
                {
                    Console.Write(matrix[index, secondIndex] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class TraceHandler : MatrixHandler
    {
        public override void HandleRequest(SquareMatrix matrix, string operation)
        {
            if (operation == "trace")
            {
                int trace = matrix.Trace();
                Console.WriteLine("След матрицы:" + trace);
            }

            else if (successor != null)
            {
                successor.HandleRequest(matrix, operation);
            }
        }
    }

    class DiagonalizeHandler : MatrixHandler
    {
        delegate void Diagonalize(SquareMatrix matrix);
        public override void HandleRequest(SquareMatrix matrix, string operation)
        {
            if (operation == "diagonalize")
            {
                matrix.Diagonalize();
                Console.WriteLine("Диагонализированная матрица:");
                PrintMatrix(matrix);
            }

            else if (successor != null)
            {
                successor.HandleRequest(matrix, operation);
            }
        }

        private void PrintMatrix(SquareMatrix matrix)
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

    interface IComparable
    {
        int CompareTo(object obj);
    }

    static class SquareMatrixExtensions
    {
        public static SquareMatrix Transpose(this SquareMatrix matrix)
        {
            SquareMatrix transposedMatrix = new SquareMatrix(matrix.Size);

            for (int index = 0; index < matrix.Size; ++index)
            {
                for (int secondIndex = 0; secondIndex < matrix.Size; ++secondIndex)
                {
                    transposedMatrix[index, secondIndex] = matrix[secondIndex, index];
                }
            }
            return transposedMatrix;
        }
        public static int Trace(this SquareMatrix matrix)
        {
            int trace = 0;

            for (int index = 0; index < matrix.Size; ++index)
            {
                trace += matrix[index, index];
            }

            return trace;
        }
        public static void Diagonalize(this SquareMatrix matrix)
        {
            for (int index = 0; index < matrix.Size; ++index)
            {
                for (int secondIndex = 0; secondIndex < matrix.Size; ++secondIndex)
                {
                    matrix[index, secondIndex] = 0;
                }
            }
        }
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

    abstract class MatrixHandler
    {
        protected MatrixHandler successor;

        public void SetSuccessor(MatrixHandler successor)
        {
            this.successor = successor;
        }

        public abstract void HandleRequest(SquareMatrix matrix, string operation);
    }
    class Program
    {
        static void Main(string[] args)
        {
            ChainApplication app = new ChainApplication();

            SquareMatrix matrix1 = new SquareMatrix(3, true);
            SquareMatrix matrix2 = new SquareMatrix(3, true);
            SquareMatrix matrix3 = matrix1 + matrix2;

            Console.WriteLine("Матрица 1:");
            PrintMatrix(matrix1);
            Console.WriteLine("Матрица 2:");
            PrintMatrix(matrix2);
            Console.WriteLine("Сумма матриц:");
            PrintMatrix(matrix3);

            app.ProcessRequest(matrix3);

            Console.WriteLine("Диагонализация матрицы 3:");
            app.ProcessRequest(matrix3);
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
