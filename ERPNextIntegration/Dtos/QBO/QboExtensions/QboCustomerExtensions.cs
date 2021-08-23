using System.Collections.Generic;
using ERPNextIntegration.Dtos.ErpNext.Customer;
using QuickBooksSharp.Entities;
namespace ERPNextIntegration.Dtos.QBO.QboExtensions
{
    public static class QboCustomerExtensions
    {
        public static ErpCustomer ToErpCustomer(this Customer customer)
        {
            return new ErpCustomer
            {
                name = customer.FullyQualifiedName,
                //owner = null,
                //creation = null,
                //modified = null,
                //modified_by = null,
                //idx = null,
                docstatus = 0,
                company = "Time Laboratories",
                //naming_series = null,
                customer_name = customer.FullyQualifiedName,
                //customer_type = customer.,
                customer_group = "All Customer Groups",
                territory = "All Territories",
                so_required = false,
                dn_required = false,
                disabled = false,
                is_internal_customer = false,
                exempt_from_sales_tax = false,
                language = "en",
                is_frozen = false,
                //default_commission_rate = null,
                //doctype = null,
                //companies = null,
                //accounts = null,
                //credit_limits = null,
                //sales_team = null
            };
        }

        public static ErpAddress ToErpBillingAddress(this Customer customer)
        {
            var address = customer.ToErpAddress(customer.BillAddr);
            address.name = customer.FullyQualifiedName + "-Billing";
            address.address_type = "Billing";
            address.is_primary_address = true;
            address.is_shipping_address = customer.ShipAddr == null;
            return address;
        }
        
        public static ErpAddress ToErpShippingAddress(this Customer customer)
        {
            var address = customer.ToErpAddress(customer.ShipAddr);
            address.name = customer.FullyQualifiedName + "-Shipping";
            address.address_type = "Shipping";
            address.is_primary_address = false;
            address.is_shipping_address = true;
            return address;
        }

        private static ErpAddress ToErpAddress(this Customer customer, PhysicalAddress address)
        {
            return new ErpAddress
            {
                docstatus = 0,
                address_title = customer.FullyQualifiedName,
                address_line1 = address.Line1,
                city = address.City,
                state = address.CountrySubDivisionCode,
                country = address.Country,
                pincode = address.PostalCode,
                email_id = customer.PrimaryEmailAddr?.Address,
                disabled = false,
                is_your_company_address = false,
                doctype = "Address",
                links = new List<ErpAddressLink>
                {
                    new ErpAddressLink
                    {
                        parentfield = "links",
                        parenttype = "Address",
                        docstatus = 0,
                        link_doctype = "Customer",
                        link_name = customer.FullyQualifiedName,
                        link_title = customer.FullyQualifiedName,
                        doctype = "Dynamic Link"
                    }
                }
            };
        }
    }
}