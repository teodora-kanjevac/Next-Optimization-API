using NextOptimization.Business.DTOs;

namespace NextOptimization.Business.Services
{
    public interface IEmailService
    {
        void SendMail(EmailDTO emailDTO, string template);
    }
}