using DualGenerator;
using System;
using System.ComponentModel;

int variables = 0;
int conditions = 0;

List<List<int>> Ax = new();
List<Operation> bConditions = new();
List<int> b = new();

List<int> x = new();
List<Operation> xConditions = new();
ObjectiveFunction objectiveFunction = ObjectiveFunction.Unknown;

Console.WriteLine("Duál generáló\n");

ReadInput();
//GenerateInput();
Console.WriteLine("\n\n");

Console.WriteLine("PRIMÁL");
ShowResult(Ax, bConditions, b, x, xConditions, objectiveFunction);
Console.WriteLine("\n\n");

var Bx = Transpose(Ax);
var newBConditions = GetNewBConditions(xConditions);
var newXConditions = GetNewXConditions(bConditions);
objectiveFunction = GetDualObjectiveFunction(objectiveFunction);

Console.WriteLine("DUÁL");
ShowResult(Bx, newBConditions, x, b, newXConditions, objectiveFunction);


Console.ReadKey();


void ReadInput()
{
    Console.Write("Adja meg a változók számát: ");

    if (!int.TryParse(Console.ReadLine()?.Trim(), out variables) || variables < 1)
    {
        throw new ArgumentException("Hibás formátum!");
    }

    Console.Write("Adja meg a feltételek számát: ");

    if (!int.TryParse(Console.ReadLine()?.Trim(), out conditions) || conditions < 1)
    {
        throw new ArgumentException("Hibás formátum!");
    }

    if (conditions < variables)
    {
        throw new ArgumentException("Nincs elég feltétel!");
    }

    Console.WriteLine();

    for (int i = 0; i < conditions; i++)
    {
        Console.WriteLine($"{i + 1}. feltétel");
        Ax.Add(new());
        for (int j = 0; j < variables; j++)
        {
            int xj;
            Console.Write($"x{j + 1}: ");

            if (!int.TryParse(Console.ReadLine()?.Trim(), out xj))
            {
                throw new ArgumentException("Hibás formátum!");
            }

            Ax[i].Add(xj);
        }

        Console.WriteLine("Feltételek:");
        Console.WriteLine("1. >=");
        Console.WriteLine("2. <=");
        Console.WriteLine("3. =");

        Console.Write("Feltétel sorszáma: ");

        int operationNumber;
        if (!int.TryParse(Console.ReadLine()?.Trim(), out operationNumber))
        {
            throw new InvalidEnumArgumentException("Hibás formátum!");
        }

        Operation operation;

        switch (operationNumber)
        {
            case 1:
            case 2:
            case 3:
                operation = (Operation)operationNumber;
                break;
            default:
                throw new InvalidEnumArgumentException("Hibás sorszám!");
        }

        Console.Write($"{OperationHelper.ToString(operation)} ");
        bConditions.Add(operation);

        int bi;
        if (!int.TryParse(Console.ReadLine()?.Trim(), out bi))
        {
            throw new ArgumentException("Hibás formátum!");
        }

        b.Add(bi);
        Console.WriteLine();
    }

    Console.WriteLine("Célfüggvény");

    for (int i = 0; i < variables; i++)
    {
        Console.Write($"x{i + 1}: ");

        int xi;
        if (!int.TryParse(Console.ReadLine()?.Trim(), out xi))
        {
            throw new ArgumentException("Hibás formátum!");
        }

        x.Add(xi);
    }

    Console.WriteLine("Célfüggvények:");
    Console.WriteLine("1. MIN");
    Console.WriteLine("2. MAX");

    Console.Write("Célfüggvény sorszáma: ");

    if (!Enum.TryParse(Console.ReadLine()?.Trim(), out objectiveFunction) || !Enum.IsDefined(typeof(ObjectiveFunction), objectiveFunction))
    {
        throw new InvalidEnumArgumentException("Hibás sorszám!");
    }

    Console.WriteLine();

    Console.WriteLine("Változókra vonatkozó feltételek");

    Console.WriteLine("Feltételek:");
    Console.WriteLine("1. >= 0");
    Console.WriteLine("2. <= 0");
    Console.WriteLine("3. előjel kötetlen");

    for (int i = 0; i < variables; i++)
    {
        Console.Write($"x{i + 1} feltételének sorszáma: ");

        int operationNumber;
        if (!int.TryParse(Console.ReadLine()?.Trim(), out operationNumber))
        {
            throw new ArgumentException("Hibás formátum!");
        }

        Operation operation;

        switch (operationNumber)
        {
            case 1:
            case 2:
                operation = (Operation)operationNumber;
                break;
            case 3:
                operation = Operation.Unrestricted;
                break;
            default:
                throw new InvalidEnumArgumentException("Hibás sorszám!");
        }

        xConditions.Add(operation);
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

List<Operation> GetNewBConditions(List<Operation> xConditions)
{
    if (xConditions.Any(x => x == Operation.Unknown || x == Operation.Equal))
    {
        throw new InvalidEnumArgumentException();
    }

    List<Operation> result = new();

    foreach (var op in xConditions)
    {
        if (op == Operation.Unrestricted)
        {
            result.Add(Operation.Equal);
        }
        else if (op == Operation.GreaterThanOrEqual)
        {
            result.Add(Operation.LessThanOrEqual);
        }
        else
        {
            result.Add(Operation.GreaterThanOrEqual);
        }
    }

    return result;
}
List<Operation> GetNewXConditions(List<Operation> bConditions)
{
    if (bConditions.Any(x => x == Operation.Unknown || x == Operation.Unrestricted))
    {
        throw new InvalidEnumArgumentException();
    }

    List<Operation> result = new();

    foreach (var op in bConditions)
    {
        if (op == Operation.Equal)
        {
            result.Add(Operation.Unrestricted);
            continue;
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
        var unrestricted = xConditions[i] == Operation.Unrestricted;

        Console.Write($"x{i + 1} {OperationHelper.ToString(xConditions[i])}{(unrestricted ? "" : " 0")}, ");
    }
}