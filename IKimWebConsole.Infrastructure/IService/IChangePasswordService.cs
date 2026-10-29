using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IChangePasswordService
    {
        Task Initiate(Domain.ChangePassword mChangePassword);
    }
}
