using ERPNextIntegration.Dtos.ErpNext;
using ERPNextIntegration.Dtos.ErpNext.SalesInvoice;

namespace ERPNextIntegration.Dtos.QBO
{
    public interface IQboDto
    {
        public IErpNextDto ToErpNextDto();
    }
}