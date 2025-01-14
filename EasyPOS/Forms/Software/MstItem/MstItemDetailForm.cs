﻿using EasyPOS.Interfaces.Forms;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyPOS.Forms.Software.MstItem
{
    public partial class MstItemDetailForm : Form, IUpdateListDataSource
    {
        public SysSoftwareForm sysSoftwareForm;
        private Modules.SysUserRightsModule sysUserRights;

        IUpdateListDataSource FormWithUpdate;
        public Entities.MstItemEntity mstItemEntity;

        public static Int32 pageSize = 50;
        public static Int32 pageNumber = 1;

        public static List<Entities.DgvMstItemPriceListEntity> itemPriceListData = new List<Entities.DgvMstItemPriceListEntity>();
        public PagedList<Entities.DgvMstItemPriceListEntity> itemPriceListPageList = new PagedList<Entities.DgvMstItemPriceListEntity>(itemPriceListData, pageNumber, pageSize);
        public BindingSource itemPriceListDataSource = new BindingSource();

        public static List<Entities.DgvMstItemComponentListEntity> itemComponentListData = new List<Entities.DgvMstItemComponentListEntity>();
        public PagedList<Entities.DgvMstItemComponentListEntity> itemComponentListPageList = new PagedList<Entities.DgvMstItemComponentListEntity>(itemComponentListData, pageNumber, pageSize);
        public BindingSource itemComponentListDataSource = new BindingSource();

        public Boolean hasComponents = false;

        public List<Entities.SysLanguageEntity> sysLanguageEntities = new List<Entities.SysLanguageEntity>();

        public MstItemDetailForm(SysSoftwareForm softwareForm, IUpdateListDataSource formWithUpdate, Entities.MstItemEntity itemEntity)
        {
            InitializeComponent();

            sysSoftwareForm = softwareForm;

            buttonClose.Text = SetLabel(buttonClose.Text);
            buttonLock.Text = SetLabel(buttonLock.Text);
            buttonUnlock.Text = SetLabel(buttonUnlock.Text);
            buttonAddItemPrice.Text = SetLabel(buttonAddItemPrice.Text);
            buttonItemComponentAdd.Text = SetLabel(buttonItemComponentAdd.Text);
            label1.Text = SetLabel(label1.Text);
            label2.Text = SetLabel(label2.Text);
            label3.Text = SetLabel(label3.Text);
            label4.Text = SetLabel(label4.Text);
            label5.Text = SetLabel(label5.Text);
            label6.Text = SetLabel(label6.Text);
            label7.Text = SetLabel(label7.Text);
            label8.Text = SetLabel(label8.Text);
            label9.Text = SetLabel(label9.Text);
            label10.Text = SetLabel(label10.Text);
            label11.Text = SetLabel(label11.Text);
            label12.Text = SetLabel(label12.Text);
            label13.Text = SetLabel(label13.Text);
            label14.Text = SetLabel(label14.Text);
            label15.Text = SetLabel(label15.Text);
            label16.Text = SetLabel(label16.Text);
            label17.Text = SetLabel(label17.Text);
            label18.Text = SetLabel(label18.Text);
            label19.Text = SetLabel(label19.Text);
            label20.Text = SetLabel(label20.Text);
            label21.Text = SetLabel(label21.Text);
            label22.Text = SetLabel(label22.Text);
            dataGridViewItemPriceList.Columns[4].HeaderText = SetLabel(dataGridViewItemPriceList.Columns[4].HeaderText);
            dataGridViewItemPriceList.Columns[5].HeaderText = SetLabel(dataGridViewItemPriceList.Columns[5].HeaderText);
            dataGridViewItemPriceList.Columns[3].HeaderText = SetLabel(dataGridViewItemPriceList.Columns[3].HeaderText);
            dataGridViewItemComponentList.Columns[6].HeaderText = SetLabel(dataGridViewItemComponentList.Columns[6].HeaderText);
            dataGridViewItemComponentList.Columns[8].HeaderText = SetLabel(dataGridViewItemComponentList.Columns[8].HeaderText);
            dataGridViewItemComponentList.Columns[9].HeaderText = SetLabel(dataGridViewItemComponentList.Columns[9].HeaderText);
            dataGridViewItemComponentList.Columns[10].HeaderText = SetLabel(dataGridViewItemComponentList.Columns[10].HeaderText);
            dataGridViewItemComponentList.Columns[11].HeaderText = SetLabel(dataGridViewItemComponentList.Columns[11].HeaderText);
            dataGridViewItemComponentList.Columns[13].HeaderText = SetLabel(dataGridViewItemComponentList.Columns[13].HeaderText);
            tabPage1.Text = SetLabel(tabPage1.Text);
            tabPage2.Text = SetLabel(tabPage2.Text);
            tabPage3.Text = SetLabel(tabPage3.Text);
            buttonItemComponentListPageListFirst.Text = SetLabel(buttonItemComponentListPageListFirst.Text);
            buttonItemComponentListPageListLast.Text = SetLabel(buttonItemComponentListPageListLast.Text);
            buttonItemComponentListPageListPrevious.Text = SetLabel(buttonItemComponentListPageListPrevious.Text);
            buttonItemComponentListPageListNext.Text = SetLabel(buttonItemComponentListPageListNext.Text);
            buttonItemPriceListPageListFirst.Text = SetLabel(buttonItemPriceListPageListFirst.Text);
            buttonItemPriceListPageListLast.Text = SetLabel(buttonItemPriceListPageListLast.Text);
            buttonItemPriceListPageListPrevious.Text = SetLabel(buttonItemPriceListPageListPrevious.Text);
            buttonItemPriceListPageListNext.Text = SetLabel(buttonItemPriceListPageListNext.Text);

            sysUserRights = new Modules.SysUserRightsModule("MstItemDetail");
            if (sysUserRights.GetUserRights() == null)
            {
                MessageBox.Show("No rights!", "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FormWithUpdate = formWithUpdate;
                mstItemEntity = itemEntity;

                GetItemList();
                textBoxBarcode.Focus();
            }
            var id = mstItemEntity.Id;

            Controllers.MstItemController itemController = new Controllers.MstItemController();
            var detail = itemController.DetailItem(id);

            if (detail != null)
            {
                sysSoftwareForm.displayTimeStamp(detail.EntryUserUserName, detail.EntryDateTime + " " + detail.EntryTime, detail.UpdatedUserUserName, detail.UpdateDateTime + " " + detail.UpdateTime);
            }
        }
        public string SetLabel(string label)
        {
            Controllers.SysLanguageController sysLabel = new Controllers.SysLanguageController();
            var language = Modules.SysCurrentModule.GetCurrentSettings().Language;
            sysLanguageEntities = sysLabel.ListLanguage("");
            if (sysLanguageEntities.Any())
            {

                if (sysLabel.ListLanguage("").Any())
                {

                    foreach (var displayedLabel in sysLanguageEntities)
                    {
                        if (language != "English")
                        {
                            if (displayedLabel.Label == label)
                            {
                                return displayedLabel.DisplayedLabel;
                            }

                        }
                        else
                        {
                            if (displayedLabel.Label == label)
                            {
                                return displayedLabel.Label;
                            }
                        }
                    }
                }
            }
            return label;
        }

        public void GetUnitList()
        {
            Controllers.MstItemController mstItemController = new Controllers.MstItemController();
            if (mstItemController.DropdownListItemUnit().Any())
            {
                comboBoxUnit.DataSource = mstItemController.DropdownListItemUnit();
                comboBoxUnit.ValueMember = "Id";
                comboBoxUnit.DisplayMember = "Unit";

                GetSupplierList();
            }
        }

        public void GetSupplierList()
        {

            Controllers.MstItemController mstItemController = new Controllers.MstItemController();
            if (mstItemController.DropdownListItemSupplier().Any())
            {
                comboBoxDefaultSupplier.DataSource = mstItemController.DropdownListItemSupplier();
                comboBoxDefaultSupplier.ValueMember = "Id";
                comboBoxDefaultSupplier.DisplayMember = "Supplier";

                GetTaxList();
            }
        }

        public void GetTaxList()
        {
            Controllers.MstItemController mstItemController = new Controllers.MstItemController();
            if (mstItemController.DropdownListItemTax().Any())
            {
                comboBoxSalesVAT.DataSource = mstItemController.DropdownListItemTax();
                comboBoxSalesVAT.ValueMember = "Id";
                comboBoxSalesVAT.DisplayMember = "Tax";

                GetCategoryList();
            }
        }
        public void GetCategoryList()
        {
            Controllers.MstItemController mstItemController = new Controllers.MstItemController();
            if (mstItemController.DropdownListItemCategory().Any())
            {
                List<Entities.MstItemEntity> newItemList = new List<Entities.MstItemEntity>();

                foreach (var obj in mstItemController.DropdownListItemCategory())
                {
                    newItemList.Add(new Entities.MstItemEntity
                    {
                        Category = obj.Category
                    });
                };
                comboBoxCategory.DataSource = newItemList;
                comboBoxCategory.ValueMember = "Category";
                comboBoxCategory.DisplayMember = "Category";
            }
            //if (mstItemController.DropdownListItemCategory(mstItemEntity.Id).Any())
            //{
            //    comboBoxCategory.DataSource = mstItemController.DropdownListItemCategory(mstItemEntity.Id);
            //    comboBoxCategory.ValueMember = "Id";
            //    comboBoxCategory.DisplayMember = "Category";
            //}
            GetItemDetail();
        }
        public void GetItemList()
        {
            Controllers.MstItemController mstItemController = new Controllers.MstItemController();
            if (mstItemController.DropdownListChildItemList(mstItemEntity.Id).Any())
            {
                List<Entities.MstItemEntity> itemList = new List<Entities.MstItemEntity>();

                itemList.Add(new Entities.MstItemEntity()
                {
                    Id = 0,
                    ItemDescription = ""
                });

                foreach (var childItem in mstItemController.DropdownListChildItemList(mstItemEntity.Id))
                {
                    itemList.Add(new Entities.MstItemEntity()
                    {
                        Id = childItem.Id,
                        ItemDescription = childItem.ItemDescription
                    });
                }

                comboBoxChildItem.DataSource = itemList;
                comboBoxChildItem.ValueMember = "Id";
                comboBoxChildItem.DisplayMember = "ItemDescription";

            }
            GetUnitList();
        }

        public void GetItemDetail()
        {
            UpdateComponents(mstItemEntity.IsLocked);

            textBoxItemCode.Text = mstItemEntity.ItemCode;
            textBoxBarcode.Text = mstItemEntity.BarCode;
            textBoxSize.Text = mstItemEntity.Size;
            textBoxBrand.Text = mstItemEntity.Brand;
            textBoxDescription.Text = mstItemEntity.ItemDescription;
            textBoxAlias.Text = mstItemEntity.Alias;
            comboBoxCategory.Text = mstItemEntity.Category;
            comboBoxUnit.SelectedValue = mstItemEntity.UnitId;
            comboBoxDefaultSupplier.SelectedValue = mstItemEntity.DefaultSupplierId;
            textBoxCost.Text = mstItemEntity.Cost.ToString("#,##0.00");
            textBoxMarkUp.Text = mstItemEntity.MarkUp.ToString("#,##0.00");
            textBoxPrice.Text = mstItemEntity.Price.ToString("#,##0.00");
            textBoxStockLevelQuantity.Text = mstItemEntity.ReorderQuantity.ToString("#,##0.00");
            textBoxOnHandQuantity.Text = mstItemEntity.OnhandQuantity.ToString("#,##0.00");
            checkBoxIsInventory.Checked = mstItemEntity.IsInventory;
            checkBoxIsPackage.Checked = mstItemEntity.IsPackage;

            DateTime expiryDate = DateTime.Today;
            if (String.IsNullOrEmpty(mstItemEntity.ExpiryDate) == false)
            {
                expiryDate = Convert.ToDateTime(mstItemEntity.ExpiryDate);
            }

            dateTimePickerExpiryDate.Value = expiryDate;
            textBoxLotNumber.Text = mstItemEntity.LotNumber;
            textBoxRemarks.Text = mstItemEntity.Remarks;
            textBoxGenericName.Text = mstItemEntity.GenericName;
            comboBoxSalesVAT.SelectedValue = mstItemEntity.OutTaxId;
            comboBoxChildItem.SelectedValue = mstItemEntity.ChildItemId != null ? mstItemEntity.ChildItemId : 0;
            textBoxConversionValue.Text = mstItemEntity.cValue.ToString("#,##0.00");

            CreateItemPriceListDataGridView();

            CreateItemComponentListDataGridView();
        }

        public void UpdateComponents(Boolean isLocked)
        {
            if (sysUserRights.GetUserRights().CanLock == false)
            {
                buttonLock.Enabled = false;
            }
            else
            {
                buttonLock.Enabled = !isLocked;
            }

            if (sysUserRights.GetUserRights().CanUnlock == false)
            {
                buttonUnlock.Enabled = false;
            }
            else
            {
                buttonUnlock.Enabled = isLocked;
            }

            textBoxBarcode.Enabled = !isLocked;
            textBoxSize.Enabled = !isLocked;
            textBoxBrand.Enabled = !isLocked;
            //textBoxDescription.Enabled = !isLocked;
            //textBoxAlias.Enabled = !isLocked;
            comboBoxCategory.Enabled = !isLocked;
            comboBoxUnit.Enabled = !isLocked;
            comboBoxDefaultSupplier.Enabled = !isLocked;
            textBoxCost.Enabled = !isLocked;
            textBoxMarkUp.Enabled = !isLocked;
            textBoxPrice.Enabled = !isLocked;
            textBoxStockLevelQuantity.Enabled = !isLocked;
            buttonCompute.Enabled = !isLocked;
            buttonComputeMarkup.Enabled = !isLocked;

            if (hasComponents)
            {
                checkBoxIsInventory.Enabled = false;
            }
            else
            {
                checkBoxIsInventory.Enabled = !isLocked;
            }

            checkBoxIsPackage.Enabled = !isLocked;
            dateTimePickerExpiryDate.Enabled = !isLocked;
            textBoxLotNumber.Enabled = !isLocked;
            textBoxRemarks.Enabled = !isLocked;
            //textBoxGenericName.Enabled = !isLocked;
            comboBoxSalesVAT.Enabled = !isLocked;
            comboBoxChildItem.Enabled = !isLocked;
            textBoxConversionValue.Enabled = !isLocked;

            textBoxBarcode.Focus();

            if (sysUserRights.GetUserRights().CanAdd == false)
            {
                buttonAddItemPrice.Enabled = false;
                buttonItemComponentAdd.Enabled = false;
            }
            else
            {
                buttonAddItemPrice.Enabled = !isLocked;
                buttonItemComponentAdd.Enabled = !isLocked;
            }

            if (sysUserRights.GetUserRights().CanEdit == false)
            {
                dataGridViewItemPriceList.Columns[0].Visible = false;
                dataGridViewItemComponentList.Columns[0].Visible = false;
            }
            else
            {
                dataGridViewItemPriceList.Columns[0].Visible = !isLocked;
                dataGridViewItemComponentList.Columns[0].Visible = !isLocked;

            }

            if (sysUserRights.GetUserRights().CanDelete == false)
            {
                dataGridViewItemPriceList.Columns[1].Visible = false;
                dataGridViewItemComponentList.Columns[1].Visible = false;
            }
            else
            {
                dataGridViewItemPriceList.Columns[1].Visible = !isLocked;
                dataGridViewItemComponentList.Columns[1].Visible = !isLocked;
            }
        }

        private void buttonLock_Click(object sender, EventArgs e)
        {
            try
            {
                Controllers.MstItemController mstItemController = new Controllers.MstItemController();

                Entities.MstItemEntity newItemEntity = new Entities.MstItemEntity()
                {
                    ItemCode = textBoxItemCode.Text,
                    BarCode = textBoxBarcode.Text,
                    Size = textBoxSize.Text,
                    Brand = textBoxBrand.Text,
                    ItemDescription = textBoxSize.Text + " " + textBoxBrand.Text,
                    Alias = textBoxSize.Text + " " + textBoxBrand.Text,
                    GenericName = textBoxSize.Text + " " + textBoxBrand.Text,
                    Category = comboBoxCategory.Text,
                    OutTaxId = Convert.ToInt32(comboBoxSalesVAT.SelectedValue),
                    UnitId = Convert.ToInt32(comboBoxUnit.SelectedValue),
                    DefaultSupplierId = Convert.ToInt32(comboBoxDefaultSupplier.SelectedValue),
                    Cost = Convert.ToDecimal(textBoxCost.Text),
                    MarkUp = Convert.ToDecimal(textBoxMarkUp.Text),
                    Price = Convert.ToDecimal(textBoxPrice.Text),
                    ReorderQuantity = Convert.ToDecimal(textBoxStockLevelQuantity.Text),
                    OnhandQuantity = Convert.ToDecimal(textBoxOnHandQuantity.Text),
                    IsInventory = checkBoxIsInventory.Checked,
                    IsPackage = checkBoxIsPackage.Checked,
                    ExpiryDate = Convert.ToDateTime(dateTimePickerExpiryDate.Value).ToShortDateString(),
                    LotNumber = textBoxLotNumber.Text,
                    Remarks = textBoxRemarks.Text,
                    ChildItemId = Convert.ToInt32(comboBoxChildItem.SelectedValue),
                    cValue = Convert.ToDecimal(textBoxConversionValue.Text)
                };

                String[] lockItem = mstItemController.LockItem(mstItemEntity.Id, newItemEntity);
                if (lockItem[1].Equals("0") == false)
                {
                    mstItemEntity.IsLocked = true;

                    UpdateComponents(true);
                    FormWithUpdate.UpdateListDataSource();
                }
                else
                {
                    mstItemEntity.IsLocked = false;

                    UpdateComponents(false);
                    MessageBox.Show(lockItem[0], "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                textBoxDescription.Text = newItemEntity.ItemDescription;
                textBoxAlias.Text = newItemEntity.Alias;
                textBoxGenericName.Text = newItemEntity.GenericName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonUnlock_Click(object sender, EventArgs e)
        {
            Controllers.MstItemController mstItemController = new Controllers.MstItemController();

            String[] unlockItem = mstItemController.UnlockItem(mstItemEntity.Id);
            if (unlockItem[1].Equals("0") == false)
            {
                mstItemEntity.IsLocked = false;

                UpdateComponents(false);
                FormWithUpdate.UpdateListDataSource();
            }
            else
            {
                mstItemEntity.IsLocked = true;

                UpdateComponents(true);
                MessageBox.Show(unlockItem[0], "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (textBoxAlias.Enabled == false)
            {
                sysSoftwareForm.RemoveTabPage();
            }
            else
            {
                DialogResult closeDialogResult = MessageBox.Show("Save Changes?", "Easy POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (closeDialogResult == DialogResult.Yes)
                {
                    Controllers.MstItemController mstItemController = new Controllers.MstItemController();

                    Entities.MstItemEntity newItemEntity = new Entities.MstItemEntity()
                    {
                        ItemCode = textBoxItemCode.Text,
                        BarCode = textBoxBarcode.Text,
                        Size = textBoxSize.Text,
                        Brand = textBoxBrand.Text,
                        ItemDescription = textBoxSize.Text + " " + textBoxBrand.Text,
                        Alias = textBoxSize.Text + " " + textBoxBrand.Text,
                        GenericName = textBoxSize.Text + " " + textBoxBrand.Text,
                        Category = comboBoxCategory.Text,
                        OutTaxId = Convert.ToInt32(comboBoxSalesVAT.SelectedValue),
                        UnitId = Convert.ToInt32(comboBoxUnit.SelectedValue),
                        DefaultSupplierId = Convert.ToInt32(comboBoxDefaultSupplier.SelectedValue),
                        Cost = Convert.ToDecimal(textBoxCost.Text),
                        MarkUp = Convert.ToDecimal(textBoxMarkUp.Text),
                        Price = Convert.ToDecimal(textBoxPrice.Text),
                        ReorderQuantity = Convert.ToDecimal(textBoxStockLevelQuantity.Text),
                        OnhandQuantity = Convert.ToDecimal(textBoxOnHandQuantity.Text),
                        IsInventory = checkBoxIsInventory.Checked,
                        IsPackage = checkBoxIsPackage.Checked,
                        ExpiryDate = Convert.ToDateTime(dateTimePickerExpiryDate.Value).ToShortDateString(),
                        LotNumber = textBoxLotNumber.Text,
                        Remarks = textBoxRemarks.Text,
                        ChildItemId = Convert.ToInt32(comboBoxChildItem.SelectedValue),
                        cValue = Convert.ToDecimal(textBoxConversionValue.Text)
                    };

                    String[] lockItem = mstItemController.SaveItem(mstItemEntity.Id, newItemEntity);
                    if (lockItem[1].Equals("0") == false)
                    {
                        sysSoftwareForm.RemoveTabPage();
                        FormWithUpdate.UpdateListDataSource();
                    }
                    else
                    {
                        MessageBox.Show(lockItem[0], "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    sysSoftwareForm.RemoveTabPage();
                }
            }

        }

        private void textBoxCost_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxMarkUp_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxStockLevelQuantity_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxOnHandQuantity_KeyPress(object sender, KeyPressEventArgs e)
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

        public void UpdateListDataSource()
        {
            SetItemPriceListDataSourceAsync();
        }

        public async void SetItemPriceListDataSourceAsync()
        {
            List<Entities.DgvMstItemPriceListEntity> getItemPriceListData = await GetItemPriceListDataTask();
            if (getItemPriceListData.Any())
            {
                itemPriceListData = getItemPriceListData;
                itemPriceListPageList = new PagedList<Entities.DgvMstItemPriceListEntity>(itemPriceListData, pageNumber, pageSize);

                if (itemPriceListPageList.PageCount == 1)
                {
                    buttonItemPriceListPageListFirst.Enabled = false;
                    buttonItemPriceListPageListPrevious.Enabled = false;
                    buttonItemPriceListPageListNext.Enabled = false;
                    buttonItemPriceListPageListLast.Enabled = false;
                }
                else if (pageNumber == 1)
                {
                    buttonItemPriceListPageListFirst.Enabled = false;
                    buttonItemPriceListPageListPrevious.Enabled = false;
                    buttonItemPriceListPageListNext.Enabled = true;
                    buttonItemPriceListPageListLast.Enabled = true;
                }
                else if (pageNumber == itemPriceListPageList.PageCount)
                {
                    buttonItemPriceListPageListFirst.Enabled = true;
                    buttonItemPriceListPageListPrevious.Enabled = true;
                    buttonItemPriceListPageListNext.Enabled = false;
                    buttonItemPriceListPageListLast.Enabled = false;
                }
                else
                {
                    buttonItemPriceListPageListFirst.Enabled = true;
                    buttonItemPriceListPageListPrevious.Enabled = true;
                    buttonItemPriceListPageListNext.Enabled = true;
                    buttonItemPriceListPageListLast.Enabled = true;
                }

                textBoxItemPriceListPageNumber.Text = pageNumber + " / " + itemPriceListPageList.PageCount;
                itemPriceListDataSource.DataSource = itemPriceListPageList;
            }
            else
            {
                buttonItemPriceListPageListFirst.Enabled = false;
                buttonItemPriceListPageListPrevious.Enabled = false;
                buttonItemPriceListPageListNext.Enabled = false;
                buttonItemPriceListPageListLast.Enabled = false;

                pageNumber = 1;

                itemPriceListData = new List<Entities.DgvMstItemPriceListEntity>();
                itemPriceListDataSource.Clear();
                textBoxItemPriceListPageNumber.Text = "1 / 1";
            }
        }

        public Task<List<Entities.DgvMstItemPriceListEntity>> GetItemPriceListDataTask()
        {
            Controllers.MstItemPriceController mstItemPriceController = new Controllers.MstItemPriceController();
            List<Entities.MstItemPriceEntity> listItemPrice = mstItemPriceController.ListItemPrice(mstItemEntity.Id);
            if (listItemPrice.Any())
            {
                var itemPrices = from d in listItemPrice
                                 select new Entities.DgvMstItemPriceListEntity
                                 {
                                     ColumnItemPriceListButtonEdit = SetLabel("Edit"),
                                     ColumnItemPriceListButtonDelete = SetLabel("Delete"),
                                     ColumnItemPriceListId = d.Id,
                                     ColumnItemPriceListPriceDescription = d.PriceDescription,
                                     ColumnItemPriceListPrice = d.Price.ToString("#,##0.00"),
                                     ColumnItemPriceListTriggerQuantity = d.TriggerQuantity.ToString("#,##0.00")
                                 };

                return Task.FromResult(itemPrices.ToList());
            }
            else
            {
                return Task.FromResult(new List<Entities.DgvMstItemPriceListEntity>());
            }
        }

        public void CreateItemPriceListDataGridView()
        {
            UpdateListDataSource();

            dataGridViewItemPriceList.Columns[0].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#01A6F0");
            dataGridViewItemPriceList.Columns[0].DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#01A6F0");
            dataGridViewItemPriceList.Columns[0].DefaultCellStyle.ForeColor = Color.White;

            dataGridViewItemPriceList.Columns[1].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F34F1C");
            dataGridViewItemPriceList.Columns[1].DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#F34F1C");
            dataGridViewItemPriceList.Columns[1].DefaultCellStyle.ForeColor = Color.White;

            dataGridViewItemPriceList.DataSource = itemPriceListDataSource;
        }

        private void textBoxItemPriceListFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateListDataSource();
            }
        }

        private void dataGridViewItemPriceList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GetItemPriceListCurrentSelectedCell(e.RowIndex);
            }

            if (e.RowIndex > -1 && dataGridViewItemPriceList.CurrentCell.ColumnIndex == dataGridViewItemPriceList.Columns["ColumnItemPriceListButtonEdit"].Index)
            {
                Entities.MstItemPriceEntity selectedItemPrice = new Entities.MstItemPriceEntity()
                {
                    Id = Convert.ToInt32(dataGridViewItemPriceList.Rows[e.RowIndex].Cells[dataGridViewItemPriceList.Columns["ColumnItemPriceListId"].Index].Value),
                    ItemId = mstItemEntity.Id,
                    PriceDescription = dataGridViewItemPriceList.Rows[e.RowIndex].Cells[dataGridViewItemPriceList.Columns["ColumnItemPriceListPriceDescription"].Index].Value.ToString(),
                    Price = Convert.ToDecimal(dataGridViewItemPriceList.Rows[e.RowIndex].Cells[dataGridViewItemPriceList.Columns["ColumnItemPriceListPrice"].Index].Value),
                    TriggerQuantity = Convert.ToDecimal(dataGridViewItemPriceList.Rows[e.RowIndex].Cells[dataGridViewItemPriceList.Columns["ColumnItemPriceListTriggerQuantity"].Index].Value)
                };

                MstItemPriceDetailForm sysSystemTablesItemPriceDetailForm = new MstItemPriceDetailForm(this, selectedItemPrice);
                sysSystemTablesItemPriceDetailForm.ShowDialog();
            }

            if (e.RowIndex > -1 && dataGridViewItemPriceList.CurrentCell.ColumnIndex == dataGridViewItemPriceList.Columns["ColumnItemPriceListButtonDelete"].Index)
            {
                DialogResult deleteDialogResult = MessageBox.Show("Delete ItemPrice?", "Easy POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (deleteDialogResult == DialogResult.Yes)
                {
                    Controllers.MstItemPriceController mstItemPriceController = new Controllers.MstItemPriceController();

                    String[] deleteItemPrice = mstItemPriceController.DeleteItemPrice(Convert.ToInt32(dataGridViewItemPriceList.Rows[e.RowIndex].Cells[2].Value));
                    if (deleteItemPrice[1].Equals("0") == false)
                    {
                        Int32 currentPageNumber = pageNumber;

                        pageNumber = 1;
                        UpdateListDataSource();
                    }
                    else
                    {
                        MessageBox.Show(deleteItemPrice[0], "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void GetItemPriceListCurrentSelectedCell(Int32 rowIndex)
        {

        }

        private void buttonItemPriceListPageListFirst_Click(object sender, EventArgs e)
        {
            itemPriceListPageList = new PagedList<Entities.DgvMstItemPriceListEntity>(itemPriceListData, 1, pageSize);
            itemPriceListDataSource.DataSource = itemPriceListPageList;

            buttonItemPriceListPageListFirst.Enabled = false;
            buttonItemPriceListPageListPrevious.Enabled = false;
            buttonItemPriceListPageListNext.Enabled = true;
            buttonItemPriceListPageListLast.Enabled = true;

            pageNumber = 1;
            textBoxItemPriceListPageNumber.Text = pageNumber + " / " + itemPriceListPageList.PageCount;
        }

        private void buttonItemPriceListPageListPrevious_Click(object sender, EventArgs e)
        {
            if (itemPriceListPageList.HasPreviousPage == true)
            {
                itemPriceListPageList = new PagedList<Entities.DgvMstItemPriceListEntity>(itemPriceListData, --pageNumber, pageSize);
                itemPriceListDataSource.DataSource = itemPriceListPageList;
            }

            buttonItemPriceListPageListNext.Enabled = true;
            buttonItemPriceListPageListLast.Enabled = true;

            if (pageNumber == 1)
            {
                buttonItemPriceListPageListFirst.Enabled = false;
                buttonItemPriceListPageListPrevious.Enabled = false;
            }

            textBoxItemPriceListPageNumber.Text = pageNumber + " / " + itemPriceListPageList.PageCount;
        }

        private void buttonItemPriceListPageListNext_Click(object sender, EventArgs e)
        {
            if (itemPriceListPageList.HasNextPage == true)
            {
                itemPriceListPageList = new PagedList<Entities.DgvMstItemPriceListEntity>(itemPriceListData, ++pageNumber, pageSize);
                itemPriceListDataSource.DataSource = itemPriceListPageList;
            }

            buttonItemPriceListPageListFirst.Enabled = true;
            buttonItemPriceListPageListPrevious.Enabled = true;

            if (pageNumber == itemPriceListPageList.PageCount)
            {
                buttonItemPriceListPageListNext.Enabled = false;
                buttonItemPriceListPageListLast.Enabled = false;
            }

            textBoxItemPriceListPageNumber.Text = pageNumber + " / " + itemPriceListPageList.PageCount;
        }

        private void buttonItemPriceListPageListLast_Click(object sender, EventArgs e)
        {
            itemPriceListPageList = new PagedList<Entities.DgvMstItemPriceListEntity>(itemPriceListData, itemPriceListPageList.PageCount, pageSize);
            itemPriceListDataSource.DataSource = itemPriceListPageList;

            buttonItemPriceListPageListFirst.Enabled = true;
            buttonItemPriceListPageListPrevious.Enabled = true;
            buttonItemPriceListPageListNext.Enabled = false;
            buttonItemPriceListPageListLast.Enabled = false;

            pageNumber = itemPriceListPageList.PageCount;
            textBoxItemPriceListPageNumber.Text = pageNumber + " / " + itemPriceListPageList.PageCount;
        }

        private void buttonAddItemPrice_Click(object sender, EventArgs e)
        {
            Entities.MstItemPriceEntity newItemPrice = new Entities.MstItemPriceEntity()
            {
                Id = 0,
                ItemId = mstItemEntity.Id,
                PriceDescription = "",
                Price = 0,
                TriggerQuantity = 0,
            };

            MstItemPriceDetailForm sysSystemTablesItemPriceDetailForm = new MstItemPriceDetailForm(this, newItemPrice);
            sysSystemTablesItemPriceDetailForm.ShowDialog();
        }

        private void buttonItemComponentAdd_Click(object sender, EventArgs e)
        {
            Entities.MstItemComponentEntity newItemComponent = new Entities.MstItemComponentEntity()
            {
                Id = 0,
                ItemId = mstItemEntity.Id,
                ComponentItemId = 0,
                Unit = "",
                Quantity = 0,
                Cost = 0,
                Amount = 0,
                IsPrinted = false,
                OnHandQuantity = 0
            };

            MstItemComponentDetailForm mstItemDetailItemComponentDetailForm = new MstItemComponentDetailForm(this, newItemComponent);
            mstItemDetailItemComponentDetailForm.ShowDialog();
        }

        public Task<List<Entities.DgvMstItemComponentListEntity>> GetItemComponentListDataTask()
        {
            Controllers.MstItemComponentController mstItemComponentController = new Controllers.MstItemComponentController();
            List<Entities.MstItemComponentEntity> listItemComponent = mstItemComponentController.ItemComponentList(mstItemEntity.Id);
            if (listItemComponent.Any())
            {
                hasComponents = true;
                checkBoxIsInventory.Enabled = false;

                var itemComponent = from d in listItemComponent
                                    select new Entities.DgvMstItemComponentListEntity
                                    {
                                        ColumnItemComponentButtonEdit = SetLabel("Edit"),
                                        ColumnItemComponentButtonDelete = SetLabel("Delete"),
                                        ColumnItemComponentId = d.Id,
                                        ColumnItemComponentItemId = d.ItemId,
                                        ColumnItemComponenItemDescription = d.ItemDescription,
                                        ColumnItemComponentComponentItemId = d.ComponentItemId,
                                        ColumnItemComponentComponentItemDescription = d.ComponentItemDescription,
                                        ColumnItemComponenUnitId = d.UnitId,
                                        ColumnItemComponenUnit = d.Unit,
                                        ColumnItemComponenQuantity = d.Quantity.ToString("#,##0.00"),
                                        ColumnItemComponenCost = d.Cost.ToString("#,##0.00"),
                                        ColumnItemComponenAmount = d.Amount.ToString("#,##0.00"),
                                        ColumnItemComponenIsPrinted = d.IsPrinted,
                                        ColumnItemComponenOnHandQty = d.OnHandQuantity.ToString("#,##0.00"),
                                    };

                return Task.FromResult(itemComponent.ToList());
            }
            else
            {
                hasComponents = false;
                if (mstItemEntity.IsLocked == false)
                {
                    checkBoxIsInventory.Enabled = true;
                }
                else
                {

                }

                return Task.FromResult(new List<Entities.DgvMstItemComponentListEntity>());
            }
        }

        public async void SetItemComponentListDataSourceAsync()
        {
            List<Entities.DgvMstItemComponentListEntity> getItemComponentListData = await GetItemComponentListDataTask();
            if (getItemComponentListData.Any())
            {
                itemComponentListData = getItemComponentListData;
                itemComponentListPageList = new PagedList<Entities.DgvMstItemComponentListEntity>(itemComponentListData, pageNumber, pageSize);

                if (itemComponentListPageList.PageCount == 1)
                {
                    buttonItemComponentListPageListFirst.Enabled = false;
                    buttonItemComponentListPageListPrevious.Enabled = false;
                    buttonItemComponentListPageListNext.Enabled = false;
                    buttonItemComponentListPageListLast.Enabled = false;
                }
                else if (pageNumber == 1)
                {
                    buttonItemComponentListPageListFirst.Enabled = false;
                    buttonItemComponentListPageListPrevious.Enabled = false;
                    buttonItemComponentListPageListNext.Enabled = true;
                    buttonItemComponentListPageListLast.Enabled = true;
                }
                else if (pageNumber == itemComponentListPageList.PageCount)
                {
                    buttonItemComponentListPageListFirst.Enabled = true;
                    buttonItemComponentListPageListPrevious.Enabled = true;
                    buttonItemComponentListPageListNext.Enabled = false;
                    buttonItemComponentListPageListLast.Enabled = false;
                }
                else
                {
                    buttonItemComponentListPageListFirst.Enabled = true;
                    buttonItemComponentListPageListPrevious.Enabled = true;
                    buttonItemComponentListPageListNext.Enabled = true;
                    buttonItemComponentListPageListLast.Enabled = true;
                }

                textBoxItemComponentListPageNumber.Text = pageNumber + " / " + itemComponentListPageList.PageCount;
                itemComponentListDataSource.DataSource = itemComponentListPageList;
            }
            else
            {
                buttonItemComponentListPageListFirst.Enabled = false;
                buttonItemComponentListPageListPrevious.Enabled = false;
                buttonItemComponentListPageListNext.Enabled = false;
                buttonItemComponentListPageListLast.Enabled = false;

                pageNumber = 1;

                itemComponentListData = new List<Entities.DgvMstItemComponentListEntity>();
                itemComponentListDataSource.Clear();
                textBoxItemComponentListPageNumber.Text = "1 / 1";
            }
        }

        public void CreateItemComponentListDataGridView()
        {
            UpdateItemComponentListDataSource();

            dataGridViewItemComponentList.Columns[0].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#01A6F0");
            dataGridViewItemComponentList.Columns[0].DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#01A6F0");
            dataGridViewItemComponentList.Columns[0].DefaultCellStyle.ForeColor = Color.White;

            dataGridViewItemComponentList.Columns[1].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#F34F1C");
            dataGridViewItemComponentList.Columns[1].DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#F34F1C");
            dataGridViewItemComponentList.Columns[1].DefaultCellStyle.ForeColor = Color.White;

            dataGridViewItemComponentList.DataSource = itemComponentListDataSource;
        }

        public void UpdateItemComponentListDataSource()
        {
            SetItemComponentListDataSourceAsync();
        }

        private void dataGridViewItemComponentList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                GetItemComponentListCurrentSelectedCell(e.RowIndex);
            }

            if (e.RowIndex > -1 && dataGridViewItemComponentList.CurrentCell.ColumnIndex == dataGridViewItemComponentList.Columns["ColumnItemComponentButtonEdit"].Index)
            {
                Entities.MstItemComponentEntity selectedItemComponent = new Entities.MstItemComponentEntity()
                {
                    Id = Convert.ToInt32(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponentId"].Index].Value),
                    ItemId = Convert.ToInt32(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponentItemId"].Index].Value),
                    ComponentItemId = Convert.ToInt32(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponentComponentItemId"].Index].Value),
                    UnitId = Convert.ToInt32(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponenUnitId"].Index].Value),
                    Unit = dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponenUnit"].Index].Value.ToString(),
                    Quantity = Convert.ToDecimal(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponenQuantity"].Index].Value),
                    Cost = Convert.ToDecimal(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponenCost"].Index].Value),
                    Amount = Convert.ToDecimal(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponenAmount"].Index].Value),
                    IsPrinted = Convert.ToBoolean(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponenIsPrinted"].Index].Value),
                    OnHandQuantity = Convert.ToDecimal(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[dataGridViewItemComponentList.Columns["ColumnItemComponenOnHandQty"].Index].Value),
                };

                MstItemComponentDetailForm mstItemDetailItemComponentDetailForm = new MstItemComponentDetailForm(this, selectedItemComponent);
                mstItemDetailItemComponentDetailForm.ShowDialog();
            }

            if (e.RowIndex > -1 && dataGridViewItemComponentList.CurrentCell.ColumnIndex == dataGridViewItemComponentList.Columns["ColumnItemComponentButtonDelete"].Index)
            {
                DialogResult deleteDialogResult = MessageBox.Show("Delete Item Component?", "Easy POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (deleteDialogResult == DialogResult.Yes)
                {
                    Controllers.MstItemComponentController mstItemComponentController = new Controllers.MstItemComponentController();

                    String[] deleteItemComponent = mstItemComponentController.DeleteItemComponent(Convert.ToInt32(dataGridViewItemComponentList.Rows[e.RowIndex].Cells[2].Value));
                    if (deleteItemComponent[1].Equals("0") == false)
                    {
                        Int32 currentPageNumber = pageNumber;

                        pageNumber = 1;
                        UpdateItemComponentListDataSource();
                    }
                    else
                    {
                        MessageBox.Show(deleteItemComponent[0], "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void GetItemComponentListCurrentSelectedCell(Int32 id) { }

        private void buttonItemComponentListPageListFirst_Click(object sender, EventArgs e)
        {
            itemComponentListPageList = new PagedList<Entities.DgvMstItemComponentListEntity>(itemComponentListData, 1, pageSize);
            itemComponentListDataSource.DataSource = itemComponentListPageList;

            buttonItemComponentListPageListFirst.Enabled = false;
            buttonItemComponentListPageListPrevious.Enabled = false;
            buttonItemComponentListPageListNext.Enabled = true;
            buttonItemComponentListPageListLast.Enabled = true;

            pageNumber = 1;
            textBoxItemComponentListPageNumber.Text = pageNumber + " / " + itemComponentListPageList.PageCount;
        }

        private void buttonItemComponentListPageListPrevious_Click(object sender, EventArgs e)
        {
            if (itemComponentListPageList.HasPreviousPage == true)
            {
                itemComponentListPageList = new PagedList<Entities.DgvMstItemComponentListEntity>(itemComponentListData, --pageNumber, pageSize);
                itemComponentListDataSource.DataSource = itemComponentListPageList;
            }

            buttonItemComponentListPageListNext.Enabled = true;
            buttonItemComponentListPageListLast.Enabled = true;

            if (pageNumber == 1)
            {
                buttonItemComponentListPageListFirst.Enabled = false;
                buttonItemComponentListPageListPrevious.Enabled = false;
            }

            textBoxItemComponentListPageNumber.Text = pageNumber + " / " + itemComponentListPageList.PageCount;
        }

        private void buttonItemComponentListPageListNext_Click(object sender, EventArgs e)
        {
            if (itemComponentListPageList.HasNextPage == true)
            {
                itemComponentListPageList = new PagedList<Entities.DgvMstItemComponentListEntity>(itemComponentListData, ++pageNumber, pageSize);
                itemComponentListDataSource.DataSource = itemComponentListPageList;
            }

            buttonItemComponentListPageListFirst.Enabled = true;
            buttonItemComponentListPageListPrevious.Enabled = true;

            if (pageNumber == itemComponentListPageList.PageCount)
            {
                buttonItemComponentListPageListNext.Enabled = false;
                buttonItemComponentListPageListLast.Enabled = false;
            }

            textBoxItemComponentListPageNumber.Text = pageNumber + " / " + itemComponentListPageList.PageCount;
        }

        private void buttonItemComponentListPageListLast_Click(object sender, EventArgs e)
        {
            itemComponentListPageList = new PagedList<Entities.DgvMstItemComponentListEntity>(itemComponentListData, itemComponentListPageList.PageCount, pageSize);
            itemComponentListDataSource.DataSource = itemComponentListPageList;

            buttonItemComponentListPageListFirst.Enabled = true;
            buttonItemComponentListPageListPrevious.Enabled = true;
            buttonItemComponentListPageListNext.Enabled = false;
            buttonItemComponentListPageListLast.Enabled = false;

            pageNumber = itemComponentListPageList.PageCount;
            textBoxItemComponentListPageNumber.Text = pageNumber + " / " + itemComponentListPageList.PageCount;
        }

        private void checkBoxIsInventory_CheckedChanged(object sender, EventArgs e)
        {
            mstItemEntity.IsInventory = checkBoxIsInventory.Checked;

            if (checkBoxIsInventory.Checked == true)
            {
                buttonItemComponentAdd.Enabled = false;
            }
            else
            {
                if (mstItemEntity.IsLocked == false)
                {
                    buttonItemComponentAdd.Enabled = true;
                }
                else
                {
                    buttonItemComponentAdd.Enabled = false;
                }
            }
        }
        private void ComputePriceMarkUp()
        {
            try
            {
                if (String.IsNullOrEmpty(textBoxMarkUp.Text) == false && String.IsNullOrEmpty(textBoxPrice.Text) == false)
                {

                    Decimal MarkUp = Convert.ToDecimal(textBoxMarkUp.Text);
                    Decimal Cost = Convert.ToDecimal(textBoxCost.Text);
                    Decimal Price = Convert.ToDecimal(textBoxPrice.Text);
                    Decimal newPrice = 0;

                    if (Cost > 0)
                    {
                        newPrice = Cost + (Cost * (MarkUp / 100));
                    }
                    else
                    {
                        newPrice = Convert.ToDecimal(textBoxPrice.Text);
                    }


                    textBoxPrice.Text = newPrice.ToString("#,##0.00");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ComputeMarkUpPrice()
        {
            try
            {
                if (String.IsNullOrEmpty(textBoxMarkUp.Text) == false && String.IsNullOrEmpty(textBoxPrice.Text) == false)
                {
                    Decimal Cost = Convert.ToDecimal(textBoxCost.Text);
                    Decimal Price = Convert.ToDecimal(textBoxPrice.Text);
                    Decimal newMarkUp = 0;
                    if (Cost > 0)
                    {
                        newMarkUp = ((Price - Cost) / Cost) * 100;
                    }

                    textBoxMarkUp.Text = newMarkUp.ToString("#,##0.00");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxMarkUp_Leave(object sender, EventArgs e)
        {

        }

        private void textBoxPrice_Leave(object sender, EventArgs e)
        {
            //if (String.IsNullOrEmpty(textBoxPrice.Text))
            //{
            //    textBoxPrice.Text = "0.00";
            //}
            //else
            //{
            //    ComputeMarkUpPrice();
            //    textBoxPrice.Text = Convert.ToDecimal(textBoxPrice.Text).ToString();
            //}
        }

        private void textBoxConversionValue_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxMarkUp_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (String.IsNullOrEmpty(textBoxMarkUp.Text))
            //    {
            //        textBoxMarkUp.Text = "0.00";
            //    }
            //    else
            //    {
            //        ComputePriceMarkUp();
            //        textBoxMarkUp.Text = Convert.ToDecimal(textBoxMarkUp.Text).ToString();
            //        textBoxPrice.Focus();
            //        textBoxPrice.SelectAll();
            //    }
            //}
        }

        private void buttonCompute_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(textBoxCost.Text) == 0)
            {
                MessageBox.Show("Cannot compute for price. Please fill in cost!", "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ComputePriceMarkUp();
                textBoxMarkUp.Text = Convert.ToDecimal(textBoxMarkUp.Text).ToString();
                textBoxPrice.Focus();
                textBoxPrice.SelectAll();
            }
        }

        private void buttonComputeMarkUpPrice_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(textBoxCost.Text) == 0)
            {
                MessageBox.Show("Cannot compute for price. Please fill in cost!", "Easy POS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ComputeMarkUpPrice();
                textBoxMarkUp.Text = Convert.ToDecimal(textBoxMarkUp.Text).ToString();
            }
        }
    }
}
