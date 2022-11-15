using DualGenerator;

int variables = 0;
int conditions = 0;

List<List<int>> Ax = new();
List<Operation> bConditions = new();
List<int> b = new();

List<int> x = new();
List<Operation> xConditions = new();
ObjectiveFunction objectiveFunction = ObjectiveFunction.Unknown;

Console.WriteLine("Duál generáló\n");

//ReadInput();
GenerateInput();

ShowResult(Ax, bConditions, b, x, xConditions, objectiveFunction);
Console.WriteLine("\n\n");

var Bx = Transpose(Ax);
var newBConditions = GetNewBConditions(xConditions);
var newXConditions = GetNewXConditions(bConditions);
objectiveFunction = GetDualObjectiveFunction(objectiveFunction);

ShowResult(Bx, newBConditions, x, b, newXConditions, objectiveFunction);
Console.ReadKey();



void ReadInput()
{
    Console.Write("Adja meg a változók számát: ");

    if (!int.TryParse(Console.ReadLine()?.Trim(), out variables) || variables < 0)
    {
        Console.WriteLine("Hibás formátum!");
        return;
    }

    Console.Write("Adja meg a feltételek számát: ");

    if (!int.TryParse(Console.ReadLine()?.Trim(), out conditions) || conditions < 0)
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
        Ax.Add(new());
        for (int j = 0; j < variables; j++)
        {
            int xj;
            Console.Write($"x{j}: ");

            if (!int.TryParse(Console.ReadLine()?.Trim(), out xj))
            {
                Console.WriteLine("Hibás formátum!");
                return;
            }

            Ax[i].Add(xj);
        }

        Console.Write($"Condition: < ");
        int bi;
        if (!int.TryParse(Console.ReadLine()?.Trim(), out bi))
        {
            Console.WriteLine("Hibás formátum!");
            return;
        }

        b.Add(bi);
    }
}
void GenerateInput()
{
    Random random = new();

    for (int i = 0; i < 3; i++)
    {
        Ax.Add(new());
        for (int j = 0; j < 3; j++)
        {
            Ax[i].Add(random.Next(-20, 20));
        }

        var rand = random.NextDouble();
        if (rand < 1d / 3d)
        {
            bConditions.Add(Operation.GreaterThanOrEqual);
        }
        else if (rand < 2d / 3d)
        {
            bConditions.Add(Operation.LessThanOrEqual);
        }
        else
        {
            bConditions.Add(Operation.Equal);
        }

        b.Add(random.Next(-10, 10));
        x.Add(random.Next(-10, 10));
        xConditions.Add(Operation.GreaterThanOrEqual);
        objectiveFunction = random.NextDouble() > 0.5 ? ObjectiveFunction.Max : ObjectiveFunction.Min;
    }
}

List<List<int>> Transpose(List<List<int>> matrix)
{
    List<List<int>> matrixT = new();

    for (int i = 0; i < matrix.First().Count; i++)
    {
        var a = matrix.Select(row => row[i]);
        matrixT.Add(a.ToList());
    }

    return matrixT;
}
//TODO test
List<Operation> GetNewBConditions(List<Operation> list)
{
    List<Operation> result = new();

    if (list.Any(x => x == Operation.Unknown))
    {
        throw new ArgumentException();
    }

    foreach (var op in list)
    {
        if (op == Operation.Equal)
        {
            result.Add(Operation.Arbitrary);
        }
        else if (op == Operation.Arbitrary)
        {
            result.Add(Operation.Equal);
        }

        result.Add(op);
    }

    return result;
}
List<Operation> GetNewXConditions(List<Operation> list)
{
    List<Operation> result = new();

    if (list.Any(x => x == Operation.Unknown))
    {
        throw new ArgumentException();
    }

    foreach (var op in list)
    {
        if (op == Operation.Equal)
        {
            result.Add(Operation.Arbitrary);
        }
        else if (op == Operation.Arbitrary)
        {
            result.Add(Operation.Equal);
        }

        result.Add(op);
    }

    return result;
}

ObjectiveFunction GetDualObjectiveFunction(ObjectiveFunction objectiveFunction)
{
    ObjectiveFunction newFunction;

    switch (objectiveFunction)
    {
        case ObjectiveFunction.Max:
            newFunction = ObjectiveFunction.Min;
            break;
        case ObjectiveFunction.Min:
            newFunction = ObjectiveFunction.Max;
            break;
        default:
            newFunction = ObjectiveFunction.Unknown;
            break;
    }

    return newFunction;
}


void ShowResult(List<List<int>> Ax, List<Operation> bOperations, List<int> b, List<int> x, List<Operation> xConditions, ObjectiveFunction objectiveFunction)
{
    for (int i = 0; i < Ax.Count; i++)
    {
        for (int j = 0; j < Ax.First().Count; j++)
        {
            string preseage = "";
            string space = (j == 0 ? "" : " ");
            if (j > 0 || Ax[i][j] < 0)
            {
                preseage = Ax[i][j] >= 0 ? $"+{space}" : $"-{space}";
            }
            Console.Write($"{preseage}{Math.Abs(Ax[i][j])} * x{j + 1} ");
        }

        var operation = OperationHelper.ToString(bOperations[i]);

        Console.Write($" {operation} {b[i]}\n");
    }

    Console.WriteLine();
    for (int i = 0; i < x.Count; i++)
    {
        string preseage = "";
        string space = (i == 0 ? "" : " ");
        if (i > 0 || x[i] < 0)
        {
            preseage = x[i] >= 0 ? $"+{space}" : $"-{space}";
        }
        Console.Write($"{preseage}{Math.Abs(x[i])} * x{i + 1} ");
    }
    Console.Write($"-> {objectiveFunction}\n");

    for (int i = 0; i < x.Count; i++)
    {
        var arbitrary = xConditions[i] == Operation.Arbitrary;

        Console.Write($"x{i + 1} {OperationHelper.ToString(xConditions[i])}{(arbitrary ? "" : " 0")}, ");
    }
}