public interface IBroxelLog
{
    Task UploadLoadFileAsync(Stream fileSteamLocal, string file);

    Task SendToGCP(string file);
}