
using DualGenerator;

int variables = 0;
int conditions = 0;
List<List<int>> x = new();
List<int> b = new();
List<Operation> operations = new();
List<int> function = new();
ObjectiveFunction objectiveFunction = ObjectiveFunction.Unknown;

Console.WriteLine("Duál generáló\n");

//ReadInput();
Random random = new();
for (int i = 0; i < 3; i++)
{
    x.Add(new());
    for (int j = 0; j < 3; j++)
    {
        x[i].Add(random.Next(-20, 20));
    }

    var rand = random.NextDouble();
    if (rand < 1d / 3d)
    {
        operations.Add(Operation.GreaterThanOrEqual);
    }
    else if (rand < 2d / 3d)
    {
        operations.Add(Operation.LessThanOrEqual);
    }
    else
    {
        operations.Add(Operation.Equal);
    }

    b.Add(random.Next(-10, 10));
    function.Add(random.Next(-10, 10));
    objectiveFunction = random.NextDouble() > 0.5 ? ObjectiveFunction.Max : ObjectiveFunction.Min;
}
ShowMatrix(x, b, function);
Console.WriteLine("\n");
var result = GenerateDual(x);
ShowMatrix(result, function, b);
Console.ReadKey();


void ShowMatrix(List<List<int>> matrix, List<int> l1, List<int> l2)
{
    for (int i = 0; i < matrix.Count; i++)
    {
        for (int j = 0; j < matrix.First().Count; j++)
        {
            string preseage = "";
            if (j > 0 || matrix[i][j] < 0)
            {
                preseage = matrix[i][j] >= 0 ? "+" : "-";
            }
            Console.Write($"{preseage}{Math.Abs(matrix[i][j])}");
        }

        var operation = "";
        switch (operations[i])
        {
            case Operation.GreaterThanOrEqual:
                operation = ">=";
                break;
            case Operation.LessThanOrEqual:
                operation = "<=";
                break;
            case Operation.Equal:
                operation = "=";
                break;
            case Operation.Unknown:
            default:
                break;
        }

        Console.Write($" {operation} {l1[i]}");

        Console.WriteLine();
    }

    for (int i = 0; i < l2.Count; i++)
    {
        string preseage = "";
        if (i > 0 || l2[i] < 0)
        {
            preseage = l2[i] >= 0 ? "+" : "-";
        }
        Console.Write($"{preseage}{Math.Abs(l2[i])}");

    }

    Console.Write($"->{objectiveFunction}");
}

void ReadInput()
{
    Console.Write("Adja meg a változók számát: ");

    if (!int.TryParse(Console.ReadLine().Trim(), out variables) || variables < 0)
    {
        Console.WriteLine("Hibás formátum!");
        return;
    }

    Console.Write("Adja meg a feltételek számát: ");

    if (!int.TryParse(Console.ReadLine().Trim(), out conditions) || conditions < 0)
    {
        Console.WriteLine("Hibás formátum!");
        return;
    }

    if (conditions < variables)
    {
        Console.WriteLine("Nincs elég feltétel!");
        return;
    }

    for (int i = 0; i < conditions; i++)
    {
        x.Add(new());
        for (int j = 0; j < variables; j++)
        {
            int xj;
            Console.Write($"x{j}: ");

            if (!int.TryParse(Console.ReadLine().Trim(), out xj))
            {
                Console.WriteLine("Hibás formátum!");
                return;
            }

            x[i].Add(xj);
        }

        Console.Write($"Condition: < ");
        int bi;
        if (!int.TryParse(Console.ReadLine().Trim(), out bi))
        {
            Console.WriteLine("Hibás formátum!");
            return;
        }

        b.Add(bi);
    }
}

List<List<int>> GenerateDual(List<List<int>> matrix)
{
    List<List<int>> matrixT = new();

    for (int i = 0; i < matrix.First().Count; i++)
    {
        var a = matrix.Select(row => row[i]);
        matrixT.Add(a.ToList());
    }

    return matrixT;
}
