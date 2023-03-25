using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Invoice;

namespace KingsFarms.Core.Api.Services;

public class InvoiceInfoService : IInvoiceInfoService
{
    public CustomerInvoicesViewModel PrepareInvoice(DateTime weekDate, int weekQty, CustomerDashboardViewModel? viewModel, string? newInvoiceNumber, string? usdaMemo)
    {
        if (viewModel == null) return new CustomerInvoicesViewModel();

        var model = new CustomerInvoicesViewModel
        {
            InvoiceNumber = newInvoiceNumber,
            CustomerHeader = viewModel.CustomerHeader,
            InvoiceDate = weekDate,
            DueDate = weekDate,
            Price = new CustomerPriceViewModel
            {
                Rate = viewModel.Price.Rate,
                ShipmentRate = viewModel.Price.ShipmentRate,
                BoxSize = viewModel.Price.BoxSize
            },
            Cost = new InvoiceCostViewModel
            {
                Quantity = weekQty,
                Amount = viewModel.Price.Rate * weekQty
            },
            Bill = new InvoiceBillViewModel
            {
                ShipmentCost = GetShippingBoxCost(weekQty, viewModel.Price.BoxSize, viewModel.Price.ShipmentRate),
                Billed = viewModel.Price.Rate * weekQty + GetShippingBoxCost(weekQty, viewModel.Price.BoxSize, viewModel.Price.ShipmentRate)
            },
            Memo = string.IsNullOrEmpty(usdaMemo) ? string.Empty : usdaMemo
        };

        switch (viewModel.CustomerHeader.CustomerKey)
        {
            case "MYT-DEN":
                model.Bill.ChargesDiscounts = 5.76m;
                model.Bill.Billed += 5.76m;
                break;
            case "TRI-CHA":
                model.Bill.ChargesDiscounts = -30m;
                model.Bill.Billed -= 30m;
                break;
            case "TRI-ELL":
                model.Bill.ChargesDiscounts = -30m;
                model.Bill.Billed -= 30m;
                break;
        }

        return model;
    }

    private static decimal GetShippingBoxCost(int weekQty, decimal boxSize, decimal shipmentRate)
    {
        if (shipmentRate == 0) return 0;

        var shipmentCost = weekQty / boxSize * shipmentRate;
        return Math.Round(shipmentCost);
    }
}