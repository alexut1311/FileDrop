import React, { Component } from 'react';
import { Loader } from './Loader';
import { UploadPopUp } from './UploadPopUp';
import { FilePreview } from './FilePreview';
import DataGrid, {
   Column,
   Selection,
   FilterRow,
   ColumnChooser,
   HeaderFilter,
   Export,
   Scrolling,
   StateStoring,
   ColumnFixing,
   TotalItem,
   Summary,
   MasterDetail
} from 'devextreme-react/data-grid';
import ExcelJS from 'exceljs';
import saveAs from 'file-saver';
import { exportDataGrid } from 'devextreme/excel_exporter';

export class FetchFiles extends Component {
   constructor(props) {
      super(props);
      this.state = { files: [], selectedFiles: [], loading: true, uploadPopupVisible: false, areSelectedFiles: false };
      this.closeButtonOptions = {
         text: 'Close',
         onClick: this.hideInfo
      };
      this.dataGrid = null;
      this.onToolbarPreparing = this.onToolbarPreparing.bind(this);
      this.hideUploadPopup = this.hideUploadPopup.bind(this);
      this.refreshDataGrid = this.refreshDataGrid.bind(this);
      this.onExporting = this.onExporting.bind(this);
   }

   componentDidMount() {
      this.getFiles();
   }

   hideUploadPopup() {
      this.setState({
         uploadPopupVisible: false
      });
   }

   showUploadPopup = () => {
      this.setState({
         uploadPopupVisible: true
      });
   }

   downloadFiles = () => {
      document.location = this.state.selectedFiles[0].previewURL

      this.setState({
         areSelectedFiles: false,
         selectedFiles: []
      });
      if (this.dataGrid) {
         this.dataGrid.instance.deselectRows(this.dataGrid.instance.getSelectedRowKeys())
      }
   }

   onSelectionChanged = (e) => {
      this.setState({
         areSelectedFiles: e.selectedRowKeys.length !== 0,
         selectedFiles: e.component.getSelectedRowsData()
      });
      if (this.dataGrid) {
         this.dataGrid.instance.refresh();
      }
   }

   onToolbarPreparing(e) {
      e.toolbarOptions.items.push({
         widget: 'dxButton',
         location: 'before',
         visible: true,
         options: {
            icon: 'upload',
            text: 'Upload',
            disabled: false,
            elementAttr: {
               id: 'uploadButton',
            },
            onClick: this.showUploadPopup
         },
      },
         {
            widget: 'dxButton',
            location: 'before',
            visible: true,
            options: {
               icon: 'download',
               text: 'Download',
               disabled: false,
               elementAttr: {
                  id: 'downloadButton',
               },
               onClick: this.downloadFiles
            },
         }
      );
   }

   refreshDataGrid(e) {
      if (this.dataGrid) {
         this.dataGrid.instance.refresh();
      }
      this.setState({
         loading: true,
      });
      this.getFiles();
   }

   onExporting(e) {
      const workbook = new ExcelJS.Workbook();
      const worksheet = workbook.addWorksheet('Main sheet');

      exportDataGrid({
         component: e.component,
         worksheet: worksheet,
         autoFilterEnabled: true
      }).then(() => {
         workbook.xlsx.writeBuffer().then((buffer) => {
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
         });
      });
      e.cancel = true;
   }

   renderFilesTable = (files) => {
      return (
         <div>
            <UploadPopUp
               uploadPopupVisible={this.state.uploadPopupVisible}
               hideUploadPopup={this.hideUploadPopup}
               refreshDataGrid={this.refreshDataGrid}
               onExporting={this.onExporting}
            />
            <DataGrid id="gridContainer"
               ref={(ref) => this.dataGrid = ref}
               dataSource={files}
               showBorders={true}
               allowColumnReordering={true}
               allowColumnResizing={true}
               columnAutoWidth={true}
               showRowLines={true}
               rowAlternationEnabled={true}
               keyExpr="id"
               noDataText="No files"
               onToolbarPreparing={this.onToolbarPreparing}
               onSelectionChanged={this.onSelectionChanged}

            >
               <Selection
                  mode="single"
                  selectAllMode="allPages"
                  showCheckBoxesMode="always"
               />
               <FilterRow visible={true} />
               <HeaderFilter visible={true} />
               <Scrolling mode="infinite" />
               <ColumnFixing enabled={true} />
               <StateStoring enabled={true} type="localStorage" storageKey="storage" />
               <ColumnChooser enabled={true} />
               <Column dataField="fileName" caption="Name" fixed={true} sortOrder="asc"/>
               <Column dataField="fileSize" caption="Size" />
               <Column dataField="uploadDate" caption="LastModified" />
               <Column dataField="storageClass" caption="StorageClass" />
               <Summary>
                  <TotalItem
                     column="fileName"
                     summaryType="count" />
               </Summary>
               <Export enabled={true} allowExportSelectedData={true} />
               <MasterDetail
                  enabled={true}
                  component={FilePreview}
               />
            </DataGrid>
         </div>
      );
   }

   render() {
      let contents = this.state.loading ?
         <Loader />
         : this.renderFilesTable(this.state.files);

      return (
         <div>
            <h1 id="filesLabel">Files</h1>
            {contents}
         </div>
      );
   }

   async getFiles() {
      const response = await fetch('file/GetFiles');
      const data = await response.json();
      this.setState({ files: data, loading: false });
   }
}
