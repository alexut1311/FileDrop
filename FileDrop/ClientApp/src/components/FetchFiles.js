import React, { Component } from 'react';
import { Loader } from './Loader';
import DataGrid, {
   Column,
   Selection,
   FilterRow,
   Paging,
   ColumnChooser
} from 'devextreme-react/data-grid';

export class FetchFiles extends Component {
   static displayName = FetchFiles.name;

   constructor(props) {
      super(props);
      this.state = { files: [], loading: true };
      this.onToolbarPreparing = this.onToolbarPreparing.bind(this);
   }

   componentDidMount() {
      this.getFiles();
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
            onClick: function () {
               console.log("upload")
            }
         },
      });
   }

   renderFilesTable = (files)=> {
      return (
         <DataGrid id="gridContainer"
            dataSource={files}
            showBorders={true}
            keyExpr="id"
            onToolbarPreparing={this.onToolbarPreparing}
         >
            <Selection
               mode="multiple"
               selectAllMode="allPages"
               showCheckBoxesMode="always"
            />
            <FilterRow visible={true} />
            <Paging defaultPageSize={10} />
            <ColumnChooser enabled={true} />
            <Column dataField="fileName" caption="Name" />
            <Column dataField="fileSize" caption="Size" />
         </DataGrid>
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
