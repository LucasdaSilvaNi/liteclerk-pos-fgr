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
    public partial class TrnSalesListReprintForm : Form
    {
        public SysSoftwareForm sysSoftwareForm;

        public Int32 trnSalesId = 0;
        public Int32 trnCollectionId = 0;

        public TrnSalesListReprintForm(SysSoftwareForm softwareForm, Int32 salesId, Int32 collectionId)
        {
            InitializeComponent();

            sysSoftwareForm = softwareForm;

            trnSalesId = salesId;
            trnCollectionId = collectionId;
        }

        private void buttonOfficialReceipt_Click(object sender, EventArgs e)
        {
            DialogResult cancelDialogResult = MessageBox.Show("Reprint Sales?", "Easy POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cancelDialogResult == DialogResult.Yes)
            {
                DialogResult printDialogResult = printDialogReprint.ShowDialog();
                if (printDialogResult == DialogResult.OK)
                {
                    new Reports.RepOfficialReceiptReportForm(trnSalesId, trnCollectionId, true, printDialogReprint.PrinterSettings.PrinterName);
                    Close();
                }
            }
        }

        private void buttonDeliveryReceipt_Click(object sender, EventArgs e)
        {
            DialogResult cancelDialogResult = MessageBox.Show("Reprint Sales?", "Easy POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cancelDialogResult == DialogResult.Yes)
            {
                String printFilePath = "";
                DialogResult folderBrowserDialoResult = folderBrowserDialogReprint.ShowDialog();

                if (folderBrowserDialoResult == DialogResult.OK)
                {
                    printFilePath = folderBrowserDialogReprint.SelectedPath;

                    if (String.IsNullOrEmpty(printFilePath) == true)
                    {
                        MessageBox.Show("Empty file path", "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        new RepSalesReport.RepSalesReportStockWithdrawalReportForm(printFilePath + "\\", StockWithdrawalReport(trnCollectionId), true);
                        MessageBox.Show("Generate PDF Successful!", "Generate CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void buttonWithdrawalSlip_Click(object sender, EventArgs e)
        {
            DialogResult cancelDialogResult = MessageBox.Show("Reprint Sales?", "Easy POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cancelDialogResult == DialogResult.Yes)
            {
                //DialogResult printDialogResult = printDialogReprint.ShowDialog();
                //if (printDialogResult == DialogResult.OK)
                //{
                //    new Reports.RepDeliveryReceiptReportForm(trnSalesId, trnCollectionId, true, printDialogReprint.PrinterSettings.PrinterName);
                //    Close();
                //}

                String printFilePath = "";
                DialogResult folderBrowserDialoResult = folderBrowserDialogReprint.ShowDialog();

                if (folderBrowserDialoResult == DialogResult.OK)
                {
                    printFilePath = folderBrowserDialogReprint.SelectedPath;

                    if (String.IsNullOrEmpty(printFilePath) == true)
                    {
                        MessageBox.Show("Empty file path", "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        new RepSalesReport.RepSalesReportStockWithdrawalReportForm(printFilePath + "\\", StockWithdrawalReport(trnCollectionId), false);
                        MessageBox.Show("Generate PDF Successful!", "Generate CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ===============================================
        // Stock Withdrawal Report (Copied) To be modified
        // ===============================================
        public List<Entities.RepSalesReportTrnCollectionEntity> StockWithdrawalReport(Int32 id)
        {
            Data.easyposdbDataContext db = new Data.easyposdbDataContext(Modules.SysConnectionStringModule.GetConnectionString());

            var stockWithdrawalReports = from d in db.TrnCollections
                                         where d.Id == id
                                         select new Entities.RepSalesReportTrnCollectionEntity
                                         {
                                             Id = d.Id,
                                             SalesId = d.SalesId,
                                             CollectionNumber = d.CollectionNumber
                                         };

            return stockWithdrawalReports.ToList();
        }
    }
}