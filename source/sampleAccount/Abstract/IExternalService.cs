using System.Threading.Tasks;

namespace sampleAccount.Abstract
{
    public interface IExternalService
    {
        Task<string> GetIBAN();
    }
}