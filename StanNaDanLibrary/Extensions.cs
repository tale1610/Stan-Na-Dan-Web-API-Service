namespace StanNaDanLibrary;
//ovo kreiramo nasu fju za bolje citanje errora koji ce da nam se desavaju

public static class Extensions
{
    public static string FormatExceptionMessage(this Exception ex)
    {
        //parametar je objekat nad kojim se metoda zove!
        StringBuilder sb = new();
        //Koristimo stringBuilter zato sto je bolji za konkatenaciju stringova i zgodniji je

        int indent = 0; //KOLIKO CEMO DA UVUCEMO SVAKI SLEDECI INNER EXCEPTION
        Exception temp = ex;

        while (temp != null)
        {
            if (indent > 0)
            {
                sb.Append($"{new string('-', indent)}>");//ovo ce da ponovi crticu onoliko puta koliki je indent odnosno uvlacenje
            }
            sb.AppendLine(temp.Message);
            indent += 2;
            temp = temp.InnerException;//svaki put kad se jedna iteracija zavrsi smestamo u temp ovaj inner ex
        }
        return sb.ToString();
    }

    internal static string HandleError(this Exception e)
    {
        StringBuilder sb = new();

        sb.AppendLine($"({e.GetType().Name}):");
        sb.Append($"{e.Message}");
        int indent = 4;

        Exception? exception = e.InnerException;

        while (exception != null)
        {
            sb.AppendLine($"{new string(' ', indent)}-> ({e.GetType().Name}):");
            sb.Append($"{exception.Message}");
            indent += 4;
            exception = exception.InnerException;
        }

        string errorText = sb.ToString();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(errorText);
        Console.ResetColor();
        return errorText;
    }

    internal static ErrorMessage ToError(this string message, int statusCode = 400)
    {
        return new ErrorMessage(message, statusCode);
    }

    internal static ErrorMessage GetError(string message, int statusCode = 400)
    {
        return new ErrorMessage(message, statusCode);
    }
}

