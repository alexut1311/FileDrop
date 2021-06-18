import React, { Component } from 'react';
import { Loader } from './Loader';
import { UploadPopUp } from './UploadPopUp';
import DataGrid, {
   Column,
   Selection,
   FilterRow,
   Paging,
   ColumnChooser
} from 'devextreme-react/data-grid';
import $ from 'jquery';

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
      debugger;
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
      debugger;
      this.setState({
         areSelectedFiles: e.selectedRowKeys.length !== 0,
         selectedFiles: e.component.getSelectedRowsData()
      });
      if (this.dataGrid) {
         this.dataGrid.instance.refresh();
      }
   }

   onToolbarPreparing(e) {
      debugger;
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

   renderFilesTable = (files) => {
      return (
         <div>
            <UploadPopUp
               uploadPopupVisible={this.state.uploadPopupVisible}
               hideUploadPopup={this.hideUploadPopup}
               refreshDataGrid={this.refreshDataGrid}
            />
            <DataGrid id="gridContainer"
               ref={(ref) => this.dataGrid = ref}
               dataSource={files}
               showBorders={true}
               keyExpr="id"
               onToolbarPreparing={this.onToolbarPreparing}
               onSelectionChanged={this.onSelectionChanged}
            >
               <Selection
                  mode="single"
                  selectAllMode="allPages"
                  showCheckBoxesMode="always"
               />
               <FilterRow visible={true} />
               <Paging defaultPageSize={30} />
               <ColumnChooser enabled={true} />
               <Column dataField="fileName" caption="Name" />
               <Column dataField="fileSize" caption="Size" />
               <Column dataField="uploadDate" caption="LastModified" />
               <Column dataField="storageClass" caption="StorageClass" />
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
