using TestTask.Core.Models;
using TestTask.Core.Services.Interface;

namespace TestTask.Core.Services;

public class CabTripFileServiceProvider : IFileServiceProvider<CabTripModel>
{
    public async Task<IFileService<CabTripModel>> GetFileServiceAsync(string path)
    {
        var fileService = new CabTripFileService(path);
        await fileService.InitAsync();

        return fileService;
    }
}
