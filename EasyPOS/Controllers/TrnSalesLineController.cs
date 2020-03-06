﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPOS.Controllers
{
    class TrnSalesLineController
    {
        // ============
        // Data Context
        // ============
        public Data.easyposdbDataContext db = new Data.easyposdbDataContext(Modules.SysConnectionStringModule.GetConnectionString());

        // ===============
        // List Sales Line
        // ===============
        public List<Entities.TrnSalesLineEntity> ListSalesLine(Int32 salesId)
        {
            var salesLines = from d in db.TrnSalesLines
                             where d.SalesId == salesId
                             select new Entities.TrnSalesLineEntity
                             {
                                 Id = d.Id,
                                 SalesId = d.SalesId,
                                 ItemId = d.ItemId,
                                 ItemDescription = d.MstItem.ItemDescription,
                                 UnitId = d.UnitId,
                                 Unit = d.MstUnit.Unit,
                                 Price = d.Price,
                                 DiscountId = d.DiscountId,
                                 Discount = d.MstDiscount.Discount,
                                 DiscountRate = d.DiscountRate,
                                 DiscountAmount = d.DiscountAmount,
                                 NetPrice = d.NetPrice,
                                 Quantity = d.Quantity,
                                 Amount = d.Amount,
                                 TaxId = d.TaxId,
                                 Tax = d.MstTax.Tax,
                                 TaxRate = d.TaxRate,
                                 TaxAmount = d.TaxAmount,
                                 SalesAccountId = d.SalesAccountId,
                                 AssetAccountId = d.AssetAccountId,
                                 CostAccountId = d.CostAccountId,
                                 TaxAccountId = d.TaxAccountId,
                                 SalesLineTimeStamp = d.SalesLineTimeStamp.ToShortDateString(),
                                 UserId = d.UserId,
                                 Preparation = d.Preparation,
                                 Price1 = d.Price1,
                                 Price2 = d.Price2,
                                 Price2LessTax = d.Price2LessTax,
                                 PriceSplitPercentage = d.PriceSplitPercentage,
                             };

            return salesLines.OrderByDescending(d => d.Id).ToList();
        }

        // =================
        // Detail Sales Line
        // =================
        public Entities.TrnSalesLineEntity DetailSalesLine(Entities.TrnSalesLineEntity objSalesLine)
        {
            var salesLines = from d in db.TrnSalesLines
                             where d.Id == objSalesLine.Id
                             select new Entities.TrnSalesLineEntity
                             {
                                 Id = d.Id,
                                 SalesId = d.SalesId,
                                 ItemId = d.ItemId,
                                 ItemDescription = d.MstItem.ItemDescription,
                                 UnitId = d.UnitId,
                                 Unit = d.MstUnit.Unit,
                                 Price = d.Price,
                                 DiscountId = d.DiscountId,
                                 Discount = d.MstDiscount.Discount,
                                 DiscountRate = d.DiscountRate,
                                 DiscountAmount = d.DiscountAmount,
                                 NetPrice = d.NetPrice,
                                 Quantity = d.Quantity,
                                 Amount = d.Amount,
                                 TaxId = d.TaxId,
                                 Tax = d.MstTax.Tax,
                                 TaxRate = d.TaxRate,
                                 TaxAmount = d.TaxAmount,
                                 SalesAccountId = d.SalesAccountId,
                                 AssetAccountId = d.AssetAccountId,
                                 CostAccountId = d.CostAccountId,
                                 TaxAccountId = d.TaxAccountId,
                                 SalesLineTimeStamp = d.SalesLineTimeStamp.ToShortDateString(),
                                 UserId = d.UserId,
                                 Preparation = d.Preparation,
                                 Price1 = d.Price1,
                                 Price2 = d.Price2,
                                 Price2LessTax = d.Price2LessTax,
                                 PriceSplitPercentage = d.PriceSplitPercentage,
                             };

            return salesLines.FirstOrDefault();
        }

        // ================
        // List Search Item
        // ================
        public List<Entities.MstItemEntity> ListSearchItem(String filter)
        {
            var items = from d in db.MstItems
                        where d.BarCode.Contains(filter)
                        || d.ItemDescription.Contains(filter)
                        || d.GenericName.Contains(filter)
                        select new Entities.MstItemEntity
                        {
                            Id = d.Id,
                            BarCode = d.BarCode,
                            ItemDescription = d.ItemDescription,
                            GenericName = d.GenericName,
                            OutTaxId = d.OutTaxId,
                            OutTax = d.MstTax1.Tax,
                            OutTaxRate = d.MstTax1.Rate,
                            UnitId = d.UnitId,
                            Unit = d.MstUnit.Unit,
                            Price = d.Price,
                            OnhandQuantity = d.OnhandQuantity,
                            IsInventory = d.IsInventory
                        };

            return items.OrderBy(d => d.ItemDescription).ToList();
        }

        // ===========
        // Detail Item
        // ===========
        public Entities.MstItemEntity DetailItem(String barcode)
        {
            var item = from d in db.MstItems
                       where d.BarCode.Equals(barcode)
                       select new Entities.MstItemEntity
                       {
                           Id = d.Id,
                           BarCode = d.BarCode,
                           ItemDescription = d.ItemDescription,
                           GenericName = d.GenericName,
                           OutTaxId = d.OutTaxId,
                           OutTax = d.MstTax1.Tax,
                           OutTaxRate = d.MstTax1.Rate,
                           UnitId = d.UnitId,
                           Unit = d.MstUnit.Unit,
                           Price = d.Price,
                           OnhandQuantity = d.OnhandQuantity
                       };

            return item.FirstOrDefault();
        }

        // ===============
        // List Item Price
        // ===============
        public List<Entities.MstItemPriceEntity> ListItemPrice(Int32 itemId)
        {
            var itemPrices = from d in db.MstItemPrices
                             where d.ItemId == itemId
                             select new Entities.MstItemPriceEntity
                             {
                                 Id = d.Id,
                                 PriceDescription = d.PriceDescription,
                                 Price = d.Price,
                                 TriggerQuantity = d.TriggerQuantity
                             };

            return itemPrices.OrderByDescending(d => d.Id).ToList();
        }

        // ======================
        // Dropdown List Discount
        // ======================
        public List<Entities.MstDiscountEntity> DropdownListDiscount()
        {
            var discounts = from d in db.MstDiscounts
                            select new Entities.MstDiscountEntity
                            {
                                Id = d.Id,
                                Discount = d.Discount,
                                DiscountRate = d.DiscountRate
                            };

            return discounts.ToList();
        }

        // ==============
        // Add Sales Line
        // ==============
        public String[] AddSalesLine(Entities.TrnSalesLineEntity objSalesLine)
        {
            try
            {
                var currentUserLogin = from d in db.MstUsers where d.Id == Convert.ToInt32(Modules.SysCurrentModule.GetCurrentSettings().CurrentUserId) select d;
                if (currentUserLogin.Any() == false)
                {
                    return new String[] { "Current login user not found.", "0" };
                }

                var sales = from d in db.TrnSales
                            where d.Id == objSalesLine.SalesId
                            select d;

                if (sales.Any() == false)
                {
                    return new String[] { "Sales transaction not found.", "0" };
                }

                var item = from d in db.MstItems where d.Id == objSalesLine.ItemId select d;
                if (item.Any() == false)
                {
                    return new String[] { "Item not found.", "0" };
                }

                if (Modules.SysCurrentModule.GetCurrentSettings().AllowNegativeInventory == "False")
                {
                    if (item.FirstOrDefault().IsInventory == true)
                    {
                        if (item.FirstOrDefault().OnhandQuantity <= 0)
                        {
                            return new String[] { "Item " + item.FirstOrDefault().ItemDescription + " has negative inventory", "0" };
                        }
                        else
                        {
                            if (item.FirstOrDefault().OnhandQuantity < objSalesLine.Quantity)
                            {
                                return new String[] { "Item " + item.FirstOrDefault().ItemDescription + " has negative inventory", "0" };
                            }
                        }
                    }
                }

                var discount = from d in db.MstDiscounts where d.Id == objSalesLine.DiscountId select d;
                if (discount.Any() == false)
                {
                    return new String[] { "Discount not found.", "0" };
                }

                var tax = from d in db.MstTaxes where d.Id == objSalesLine.TaxId select d;
                if (tax.Any() == false)
                {
                    return new String[] { "Tax not found.", "0" };
                }

                var user = from d in db.MstUsers where d.Id == Convert.ToInt32(Modules.SysCurrentModule.GetCurrentSettings().CurrentUserId) select d;
                if (user.Any() == false)
                {
                    return new String[] { "User not found.", "0" };
                }

                Data.TrnSalesLine newSaleLine = new Data.TrnSalesLine
                {
                    SalesId = objSalesLine.SalesId,
                    ItemId = objSalesLine.ItemId,
                    UnitId = item.FirstOrDefault().UnitId,
                    Price = objSalesLine.Price,
                    DiscountId = objSalesLine.DiscountId,
                    DiscountRate = objSalesLine.DiscountRate,
                    DiscountAmount = objSalesLine.DiscountAmount,
                    NetPrice = objSalesLine.NetPrice,
                    Quantity = objSalesLine.Quantity,
                    Amount = objSalesLine.Amount,
                    TaxId = objSalesLine.TaxId,
                    TaxRate = objSalesLine.TaxRate,
                    TaxAmount = objSalesLine.TaxAmount,
                    SalesAccountId = 159,
                    AssetAccountId = 255,
                    CostAccountId = 238,
                    TaxAccountId = 87,
                    SalesLineTimeStamp = DateTime.Now.Date,
                    UserId = user.FirstOrDefault().Id,
                    Preparation = objSalesLine.Preparation,
                    Price1 = 0,
                    Price2 = 0,
                    Price2LessTax = 0,
                    PriceSplitPercentage = 0,
                };

                db.TrnSalesLines.InsertOnSubmit(newSaleLine);
                db.SubmitChanges();

                String newObject = Modules.SysAuditTrailModule.GetObjectString(newSaleLine);

                Entities.SysAuditTrailEntity newAuditTrail = new Entities.SysAuditTrailEntity()
                {
                    UserId = currentUserLogin.FirstOrDefault().Id,
                    AuditDate = DateTime.Now,
                    TableInformation = "TrnSalesLine",
                    RecordInformation = "",
                    FormInformation = newObject,
                    ActionInformation = "AddSalesLine"
                };
                Modules.SysAuditTrailModule.InsertAuditTrail(newAuditTrail);

                var updateSales = sales.FirstOrDefault();
                updateSales.Amount = sales.FirstOrDefault().TrnSalesLines.Any() ? sales.FirstOrDefault().TrnSalesLines.Sum(d => d.Amount) : 0;
                db.SubmitChanges();

                return new String[] { "", "1" };
            }
            catch (Exception e)
            {
                return new String[] { e.Message, "0" };
            }
        }

        // =================
        // Update Sales Line
        // =================
        public String[] UpdateSalesLine(Int32 id, Entities.TrnSalesLineEntity objSalesLine)
        {
            try
            {
                var currentUserLogin = from d in db.MstUsers where d.Id == Convert.ToInt32(Modules.SysCurrentModule.GetCurrentSettings().CurrentUserId) select d;
                if (currentUserLogin.Any() == false)
                {
                    return new String[] { "Current login user not found.", "0" };
                }

                var salesLine = from d in db.TrnSalesLines
                                where d.Id == id
                                select d;

                if (salesLine.Any())
                {
                    var sales = from d in db.TrnSales
                                where d.Id == objSalesLine.SalesId
                                select d;

                    if (sales.Any() == false)
                    {
                        return new String[] { "Sales transaction not found.", "0" };
                    }

                    var item = from d in db.MstItems where d.Id == objSalesLine.ItemId select d;
                    if (item.Any() == false)
                    {
                        return new String[] { "Item not found.", "0" };
                    }

                    var discount = from d in db.MstDiscounts where d.Id == objSalesLine.DiscountId select d;
                    if (discount.Any() == false)
                    {
                        return new String[] { "Discount not found.", "0" };
                    }

                    var tax = from d in db.MstTaxes where d.Id == objSalesLine.TaxId select d;
                    if (tax.Any() == false)
                    {
                        return new String[] { "Tax not found.", "0" };
                    }

                    var user = from d in db.MstUsers where d.Id == Convert.ToInt32(Modules.SysCurrentModule.GetCurrentSettings().CurrentUserId) select d;
                    if (user.Any() == false)
                    {
                        return new String[] { "User not found.", "0" };
                    }

                    if (Modules.SysCurrentModule.GetCurrentSettings().AllowNegativeInventory == "False")
                    {
                        if (salesLine.FirstOrDefault().MstItem.IsInventory == true)
                        {
                            if (salesLine.FirstOrDefault().MstItem.OnhandQuantity <= 0)
                            {
                                return new String[] { "Item " + salesLine.FirstOrDefault().MstItem.ItemDescription + " has negative inventory", "0" };
                            }
                            else
                            {
                                if (salesLine.FirstOrDefault().MstItem.OnhandQuantity < objSalesLine.Quantity)
                                {
                                    return new String[] { "Item " + salesLine.FirstOrDefault().MstItem.ItemDescription + " has negative inventory", "0" };
                                }
                            }
                        }
                    }

                    String oldObject = Modules.SysAuditTrailModule.GetObjectString(salesLine.FirstOrDefault());

                    var updateSalesLine = salesLine.FirstOrDefault();
                    updateSalesLine.Quantity = objSalesLine.Quantity;
                    updateSalesLine.Price = objSalesLine.Price;
                    updateSalesLine.DiscountId = objSalesLine.DiscountId;
                    updateSalesLine.DiscountRate = objSalesLine.DiscountRate;
                    updateSalesLine.DiscountAmount = objSalesLine.DiscountAmount;
                    updateSalesLine.NetPrice = objSalesLine.NetPrice;
                    updateSalesLine.Amount = objSalesLine.Amount;
                    updateSalesLine.TaxId = objSalesLine.TaxId;
                    updateSalesLine.TaxRate = objSalesLine.TaxRate;
                    updateSalesLine.TaxAmount = objSalesLine.TaxAmount;
                    updateSalesLine.SalesLineTimeStamp = DateTime.Now.Date;
                    updateSalesLine.UserId = user.FirstOrDefault().Id;
                    updateSalesLine.Preparation = objSalesLine.Preparation;
                    db.SubmitChanges();

                    String newObject = Modules.SysAuditTrailModule.GetObjectString(salesLine.FirstOrDefault());

                    Entities.SysAuditTrailEntity newAuditTrail = new Entities.SysAuditTrailEntity()
                    {
                        UserId = currentUserLogin.FirstOrDefault().Id,
                        AuditDate = DateTime.Now,
                        TableInformation = "TrnSalesLine",
                        RecordInformation = oldObject,
                        FormInformation = newObject,
                        ActionInformation = "UpdateSalesLine"
                    };
                    Modules.SysAuditTrailModule.InsertAuditTrail(newAuditTrail);

                    var updateSales = sales.FirstOrDefault();
                    updateSales.Amount = sales.FirstOrDefault().TrnSalesLines.Any() ? sales.FirstOrDefault().TrnSalesLines.Sum(d => d.Amount) : 0;
                    db.SubmitChanges();

                    return new String[] { "", "1" };
                }
                else
                {
                    return new String[] { "Sales line not found.", "0" };
                }
            }
            catch (Exception e)
            {
                return new String[] { e.Message, "0" };
            }
        }

        // =================
        // Delete Sales Line 
        // =================
        public String[] DeleteSalesLine(Int32 id)
        {
            try
            {
                var currentUserLogin = from d in db.MstUsers where d.Id == Convert.ToInt32(Modules.SysCurrentModule.GetCurrentSettings().CurrentUserId) select d;
                if (currentUserLogin.Any() == false)
                {
                    return new String[] { "Current login user not found.", "0" };
                }

                var salesLine = from d in db.TrnSalesLines
                                where d.Id == id
                                select d;

                if (salesLine.Any())
                {
                    Int32 salesId = salesLine.FirstOrDefault().SalesId;

                    db.TrnSalesLines.DeleteOnSubmit(salesLine.FirstOrDefault());

                    String oldObject = Modules.SysAuditTrailModule.GetObjectString(salesLine.FirstOrDefault());

                    Entities.SysAuditTrailEntity newAuditTrail = new Entities.SysAuditTrailEntity()
                    {
                        UserId = currentUserLogin.FirstOrDefault().Id,
                        AuditDate = DateTime.Now,
                        TableInformation = "TrnSalesLine",
                        RecordInformation = oldObject,
                        FormInformation = "",
                        ActionInformation = "DeleteSalesLine"
                    };
                    Modules.SysAuditTrailModule.InsertAuditTrail(newAuditTrail);

                    db.SubmitChanges();

                    var sales = from d in db.TrnSales
                                where d.Id == salesId
                                select d;

                    if (sales.Any() == false)
                    {
                        return new String[] { "Sales transaction not found.", "0" };
                    }

                    var updateSales = sales.FirstOrDefault();
                    updateSales.Amount = sales.FirstOrDefault().TrnSalesLines.Any() ? sales.FirstOrDefault().TrnSalesLines.Sum(d => d.Amount) : 0;
                    db.SubmitChanges();

                    return new String[] { "", "1" };
                }
                else
                {
                    return new String[] { "Sales line not found.", "0" };
                }
            }
            catch (Exception e)
            {
                return new String[] { e.Message, "0" };
            }
        }

        // ==================
        // Barcode Sales Line
        // ==================
        public String[] BarcodeSalesLine(Int32 salesId, String barcode)
        {
            try
            {
                var sales = from d in db.TrnSales
                            where d.Id == salesId
                            select d;

                if (sales.Any() == false)
                {
                    return new String[] { "Sales transaction not found.", "0" };
                }

                var item = from d in db.MstItems where d.BarCode.Equals(barcode) select d;
                if (item.Any() == false)
                {
                    return new String[] { "Item not found.", "0" };
                }

                var discount = from d in db.MstDiscounts where d.Id == Convert.ToInt32(Modules.SysCurrentModule.GetCurrentSettings().DefaultDiscountId) select d;
                if (discount.Any() == false)
                {
                    return new String[] { "Discount not found.", "0" };
                }

                var user = from d in db.MstUsers where d.Id == Convert.ToInt32(Modules.SysCurrentModule.GetCurrentSettings().CurrentUserId) select d;
                if (user.Any() == false)
                {
                    return new String[] { "User not found.", "0" };
                }

                Decimal discountAmount = item.FirstOrDefault().Price * (discount.FirstOrDefault().DiscountRate / 100);
                Decimal netPrice = item.FirstOrDefault().Price - discountAmount;
                Decimal amount = netPrice * 1;
                Decimal taxAmount = amount * (item.FirstOrDefault().MstTax1.Rate / 100);

                Data.TrnSalesLine newSaleLine = new Data.TrnSalesLine
                {
                    SalesId = salesId,
                    ItemId = item.FirstOrDefault().Id,
                    UnitId = item.FirstOrDefault().UnitId,
                    Price = item.FirstOrDefault().Price,
                    DiscountId = discount.FirstOrDefault().Id,
                    DiscountRate = discount.FirstOrDefault().DiscountRate,
                    DiscountAmount = discountAmount,
                    NetPrice = netPrice,
                    Quantity = 1,
                    Amount = amount,
                    TaxId = item.FirstOrDefault().OutTaxId,
                    TaxRate = item.FirstOrDefault().MstTax1.Rate,
                    TaxAmount = taxAmount,
                    SalesAccountId = 159,
                    AssetAccountId = 255,
                    CostAccountId = 238,
                    TaxAccountId = 87,
                    SalesLineTimeStamp = DateTime.Now.Date,
                    UserId = user.FirstOrDefault().Id,
                    Preparation = "NA",
                    Price1 = 0,
                    Price2 = 0,
                    Price2LessTax = 0,
                    PriceSplitPercentage = 0,
                };

                db.TrnSalesLines.InsertOnSubmit(newSaleLine);
                db.SubmitChanges();

                var updateSales = sales.FirstOrDefault();
                updateSales.Amount = sales.FirstOrDefault().TrnSalesLines.Any() ? sales.FirstOrDefault().TrnSalesLines.Sum(d => d.Amount) : 0;
                db.SubmitChanges();

                return new String[] { "", "1" };
            }
            catch (Exception e)
            {
                return new String[] { e.Message, "0" };
            }
        }

        // =========================
        // Check Discount VAT Exempt
        // =========================
        public Boolean IsVATExempt(Int32 discountId)
        {
            Boolean isDiscountVATExempt = false;

            var discount = from d in db.MstDiscounts
                           where d.Id == discountId
                           select d;

            if (discount.Any())
            {
                isDiscountVATExempt = discount.FirstOrDefault().IsVatExempt;
            }

            return isDiscountVATExempt;
        }
    }
}