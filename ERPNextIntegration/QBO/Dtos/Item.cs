using Microsoft.OData.Edm;

namespace ERPNextIntegration.QBO.Dtos
{
    public class Item
    { 
        public string FullyQualifiedName { get; set;}
        public string domain { get; set;}
        public string Id { get; set;}
        public string Name { get; set;}
        public bool TrackQtyOnHand { get; set;}
        public string Type { get; set;}
        public decimal PurchaseCost { get; set;}
        public int QtyOnHand { get; set;}
        public Reference IncomeAccountRef { get; set;}
        public Reference AssetAccountRef { get; set;}
        public bool Taxable { get; set;}
        public MetaData MetaData { get; set;}
        public bool sparse { get; set;}
        public bool Active { get; set;}
        public string SyncToken { get; set;}
        public Date InvStartDate { get; set;}
        public decimal UnitPrice { get; set;}
        public Reference ExpenseAccountRef { get; set;}
        public string PurchaseDesc { get; set;}
        public string Description { get; set;}
    }
}