using System;
using System.Threading.Tasks;
using DevStore.Core.Messages.Integration;
using DevStore.Pagamentos.API.Models;

namespace DevStore.Pagamentos.API.Services
{
    public interface IPagamentoService
    {
        Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);
        Task<ResponseMessage> CapturarPagamento(Guid pedidoId);
        Task<ResponseMessage> CancelarPagamento(Guid pedidoId);
    }
}