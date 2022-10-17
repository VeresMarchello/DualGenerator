using DualGenerator;

int variablesCount = 0;
int conditionsCount = 0;//without variable conditions

List<List<int>> aMatrix = new();//x1 + 2 * x2 + ...
List<int> b = new();//= 3
List<Operation> operations = new();//x1 + ... >= 3
List<Operation> variableConditions = new();//x1, x2 >= 0, x3 = 0 
List<int> functionVariables = new();//3 * x1 - x3
LinearFunction linearFunction = LinearFunction.Unknown;//--> MAX

Console.WriteLine("Duál generáló\n");

//ReadInput();
GenerateInput();
ShowResult(aMatrix, b, functionVariables, operations, variableConditions);

Console.WriteLine("\n");

var result = GenerateDual(aMatrix);
var dualOperations = GetDualOperations(operations, false);
var dualVariableConditions = GetDualOperations(variableConditions, true);
linearFunction = linearFunction == LinearFunction.Max ? LinearFunction.Min : LinearFunction.Max; 

ShowResult(result, functionVariables, b, dualOperations, dualVariableConditions);
Console.ReadKey();

void GenerateInput()
{
    Random random = new();

    //Generates A Matrix
    for (int i = 0; i < 3; i++)
    {
        aMatrix.Add(new());
        for (int j = 0; j < 3; j++)
        {
            aMatrix[i].Add(random.Next(-20, 20));
        }

        //Generates operations in each A Matrix row
        //Generates variable conditions
        var rand = random.NextDouble();
        if (rand < 1d / 3d)
        {
            operations.Add(Operation.GreaterThanOrEqual);
            variableConditions.Add(Operation.GreaterThanOrEqual);
        }
        else if (rand < 2d / 3d)
        {
            operations.Add(Operation.LessThanOrEqual);
            variableConditions.Add(Operation.LessThanOrEqual);
        }
        else
        {
            operations.Add(Operation.Equal);
            variableConditions.Add(Operation.Equal);
        }

        //Generates b constants in each A Matrix row
        b.Add(random.Next(-10, 10));

        //Generates linear function variables
        functionVariables.Add(random.Next(-10, 10));
    }
    //Generates linear function type (MIN / MAX)
    linearFunction = random.NextDouble() > 0.5 ? LinearFunction.Max : LinearFunction.Min;
}

void ShowResult(List<List<int>> aMatrix, List<int> b, List<int> functionVariables, List<Operation> operations, List<Operation> variableConditions)
{
    for (int i = 0; i < aMatrix.Count; i++)
    {
        for (int j = 0; j < aMatrix.First().Count; j++)
        {
            string preseage = "";
            if (j > 0 || aMatrix[i][j] < 0)
            {
                preseage = aMatrix[i][j] >= 0 ? "+" : "-";
            }
            Console.Write($"{preseage}{Math.Abs(aMatrix[i][j])}*x{j + 1}");
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

        Console.Write($" {operation} {b[i]}\n");
    }
    Console.WriteLine();

    for (int i = 0; i < variableConditions.Count; i++)
    {
        var operation = "";
        switch (variableConditions[i])
        {
            case Operation.GreaterThanOrEqual:
                operation = ">=";
                break;
            case Operation.LessThanOrEqual:
                operation = "<=";
                break;
            case Operation.Equal:
                operation = "előjel kötetlen";
                break;
            case Operation.Unknown:
            default:
                break;
        }

        Console.Write($"x{i + 1} {operation} 0, ");
    }

    Console.WriteLine();

    for (int i = 0; i < functionVariables.Count; i++)
    {
        string preseage = "";
        if (i > 0 || functionVariables[i] < 0)
        {
            preseage = functionVariables[i] >= 0 ? "+" : "-";
        }

        Console.Write($"{preseage}{Math.Abs(functionVariables[i])}*x{i + 1}");
    }

    Console.Write($"->{linearFunction}");
}

void ReadInput()
{
    Console.Write("Adja meg a változók számát: ");

    if (!int.TryParse(Console.ReadLine().Trim(), out variablesCount) || variablesCount < 0)
    {
        Console.WriteLine("Hibás formátum!");
        return;
    }

    Console.Write("Adja meg a feltételek számát: ");

    if (!int.TryParse(Console.ReadLine().Trim(), out conditionsCount) || conditionsCount < 0)
    {
        Console.WriteLine("Hibás formátum!");
        return;
    }

    if (conditionsCount < variablesCount)
    {
        Console.WriteLine("Nincs elég feltétel!");
        return;
    }

    for (int i = 0; i < conditionsCount; i++)
    {
        aMatrix.Add(new());
        for (int j = 0; j < variablesCount; j++)
        {
            int xj;
            Console.Write($"x{j}: ");

            if (!int.TryParse(Console.ReadLine().Trim(), out xj))
            {
                Console.WriteLine("Hibás formátum!");
                return;
            }

            aMatrix[i].Add(xj);
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

//TODO:

List<Operation> GetDualOperations(List<Operation> operations, bool isVariableOperation)
{
    if (isVariableOperation)
    {
        return operations;
    }

    List<Operation> dualOperations = new();

    foreach (var item in operations)
    {
        if (item == Operation.GreaterThanOrEqual)
        {
            dualOperations.Add(Operation.LessThanOrEqual);
        }
        else if (item == Operation.LessThanOrEqual)
        {
            dualOperations.Add(Operation.GreaterThanOrEqual);
        }
        else
        {
            dualOperations.Add(item);
        }
    }

    return dualOperations;
}
