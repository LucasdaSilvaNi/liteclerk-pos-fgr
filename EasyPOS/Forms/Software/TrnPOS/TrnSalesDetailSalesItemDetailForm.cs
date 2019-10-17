﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyPOS.Forms.Software.TrnPOS
{
    public partial class TrnSalesDetailSalesItemDetailForm : Form
    {
        public TrnSalesDetailForm trnSalesDetailForm;
        public Entities.TrnSalesLineEntity trnSalesLineEntity;

        public TrnSalesDetailSalesItemDetailForm(TrnSalesDetailForm salesDetailForm, Entities.TrnSalesLineEntity salesLineEntity)
        {
            InitializeComponent();

            trnSalesDetailForm = salesDetailForm;
            trnSalesLineEntity = salesLineEntity;

            GetDiscountList();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void GetDiscountList()
        {
            Controllers.TrnPOSSalesLineController trnPOSSalesLineController = new Controllers.TrnPOSSalesLineController();
            if (trnPOSSalesLineController.DropdownListDiscount().Any())
            {
                comboBoxSalesLineDiscount.DataSource = trnPOSSalesLineController.DropdownListDiscount();
                comboBoxSalesLineDiscount.ValueMember = "Id";
                comboBoxSalesLineDiscount.DisplayMember = "Discount";

                GetSalesLineItemDetail();
            }
        }

        private void GetSalesLineItemDetail()
        {
            textBoxItemDescription.Text = trnSalesLineEntity.ItemDescription;
            textBoxSalesLineQuantity.Text = trnSalesLineEntity.Quantity.ToString("#,##0.00");
            textBoxSalesLineUnit.Text = trnSalesLineEntity.Unit;
            textBoxSalesLinePrice.Text = trnSalesLineEntity.Price.ToString("#,##0.00");
            comboBoxSalesLineDiscount.SelectedValue = trnSalesLineEntity.DiscountId;
            textBoxSalesLineNetPrice.Text = trnSalesLineEntity.NetPrice.ToString("#,##0.00");
            textBoxSalesLineAmount.Text = trnSalesLineEntity.Amount.ToString("#,##0.00");
            textBoxSalesLineVAT.Text = trnSalesLineEntity.Tax;
            textBoxSalesLineVATRate.Text = trnSalesLineEntity.TaxRate.ToString("#,##0.00");
            textBoxSalesLineRemarks.Text = trnSalesLineEntity.Preparation;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Entities.TrnSalesLineEntity newSalesLineEntity = new Entities.TrnSalesLineEntity()
            {
                Id = trnSalesLineEntity.Id,
                SalesId = trnSalesLineEntity.SalesId,
                ItemId = trnSalesLineEntity.ItemId,
                ItemDescription = trnSalesLineEntity.ItemDescription,
                UnitId = trnSalesLineEntity.UnitId,
                Unit = trnSalesLineEntity.Unit,
                Price = trnSalesLineEntity.Price,
                DiscountId = Convert.ToInt32(comboBoxSalesLineDiscount.SelectedValue),
                Discount = trnSalesLineEntity.Discount,
                DiscountRate = Convert.ToDecimal(textBoxSalesLineDiscountRate.Text),
                DiscountAmount = Convert.ToDecimal(textBoxSalesLineDiscountAmount.Text),
                NetPrice = Convert.ToDecimal(textBoxSalesLineNetPrice.Text),
                Quantity = Convert.ToDecimal(textBoxSalesLineQuantity.Text),
                Amount = Convert.ToDecimal(textBoxSalesLineAmount.Text),
                TaxId = trnSalesLineEntity.TaxId,
                Tax = trnSalesLineEntity.Tax,
                TaxRate = trnSalesLineEntity.TaxRate,
                TaxAmount = Convert.ToDecimal(textBoxSalesLineVATAmount.Text),
                SalesAccountId = trnSalesLineEntity.SalesAccountId,
                AssetAccountId = trnSalesLineEntity.AssetAccountId,
                CostAccountId = trnSalesLineEntity.CostAccountId,
                TaxAccountId = trnSalesLineEntity.TaxAccountId,
                SalesLineTimeStamp = trnSalesLineEntity.SalesLineTimeStamp,
                UserId = trnSalesLineEntity.UserId,
                Preparation = textBoxSalesLineRemarks.Text,
                Price1 = 0,
                Price2 = 0,
                Price2LessTax = 0,
                PriceSplitPercentage = 0,
            };

            Controllers.TrnPOSSalesLineController trnPOSSalesLineController = new Controllers.TrnPOSSalesLineController();
            if (newSalesLineEntity.Id == 0)
            {
                String[] addSales = trnPOSSalesLineController.AddSalesLine(newSalesLineEntity);
                if (addSales[1].Equals("0") == false)
                {
                    trnSalesDetailForm.GetSalesLineList();
                    Close();
                }
                else
                {
                    MessageBox.Show(addSales[0], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                String[] addSales = trnPOSSalesLineController.UpdatealesLine(trnSalesLineEntity.Id, newSalesLineEntity);
                if (addSales[1].Equals("0") == false)
                {
                    trnSalesDetailForm.GetSalesLineList();
                    Close();
                }
                else
                {
                    MessageBox.Show(addSales[0], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBoxSalesLineDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSalesLineDiscount.SelectedItem == null)
            {
                return;
            }

            var selectedItemDiscount = (Entities.MstDiscount)comboBoxSalesLineDiscount.SelectedItem;
            if (selectedItemDiscount != null)
            {
                textBoxSalesLineDiscountRate.Text = selectedItemDiscount.DiscountRate.ToString("#,##0.00");
                ComputeAmount();
            }
        }

        public void ComputeAmount()
        {
            Decimal quantity = Convert.ToDecimal(textBoxSalesLineQuantity.Text);
            Decimal price = trnSalesLineEntity.Price;
            Decimal discountRate = Convert.ToDecimal(textBoxSalesLineDiscountRate.Text);
            Decimal taxRate = trnSalesLineEntity.TaxRate;

            Decimal discountAmount = 0;
            if (discountRate > 0)
            {
                discountAmount = price * (discountRate / 100);
            }

            Decimal netPrice = price - discountAmount;
            Decimal amount = netPrice * quantity;

            Decimal taxAmount = 0;
            if (taxRate > 0)
            {
                taxAmount = amount * (taxRate / 100);
            }

            textBoxSalesLineDiscountAmount.Text = discountAmount.ToString("#,##0.00");
            textBoxSalesLineNetPrice.Text = netPrice.ToString("#,##0.00");
            textBoxSalesLineAmount.Text = amount.ToString("#,##0.00");
            textBoxSalesLineVATAmount.Text = taxAmount.ToString("#,##0.00");
        }

        private void textBoxSalesLineQuantity_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxSalesLineQuantity.Text))
            {
                textBoxSalesLineQuantity.Text = "0.00";
            }

            ComputeAmount();
        }

        private void textBoxSalesLineQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBoxSalesLineQuantity_Leave(object sender, EventArgs e)
        {
            textBoxSalesLineQuantity.Text = Convert.ToDecimal(textBoxSalesLineQuantity.Text).ToString("#,##0.00");
        }
    }
}