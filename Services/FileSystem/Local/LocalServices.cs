

public class LocalServices: IFileServices
{
    //Método para creación de ruta local temporal
    public string CreateTempfilePath()
    {
        var fileName = $"{Guid.NewGuid()}.log";
        var directoryPath = Path.Combine("temps", "Mobiles");
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        return Path.Combine(directoryPath, fileName);
    }
}