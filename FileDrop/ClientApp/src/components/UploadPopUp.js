import React, { Component} from 'react';
import { Modal, Button } from 'react-bootstrap';
import FileUploader from 'devextreme-react/file-uploader';
import $ from 'jquery';

export class UploadPopUp extends Component {

   constructor(props) {
      super(props);
      this.state = {
         multiple: true,
         uploadMode: 'useButtons',
         accept: '*',
         selectedFiles: []
      };
      this.onUploaded = this.onUploaded.bind(this);

   }

   onSelectedFilesChanged(e) {
      $("#modal-footer").append(e.element.getElementsByClassName("dx-fileuploader-upload-button dx-button-has-text")[0])
   }

   onUploaded(e) {
      this.props.hideUploadPopup();
      this.props.refreshDataGrid();
   }

   render() {
      return (
         <div>
            <Modal id="uploadModal" show={this.props.uploadPopupVisible} onHide={this.props.hideUploadPopup}
               size="lg"
               aria-labelledby="contained-modal-title-vcenter"
               centered>
               <Modal.Header closeButton>
                  <Modal.Title>Upload files</Modal.Title>
               </Modal.Header>

               <Modal.Body>
                  <div className="widget-container">
                     <FileUploader multiple={this.state.multiple} accept={this.state.accept} uploadMode={this.state.uploadMode}
                        uploadUrl="https://localhost:44322/file/uploadFile" onValueChanged={this.onSelectedFilesChanged} onUploaded={this.onUploaded}/>
                     <div className="content" style={{ display: this.state.selectedFiles.length > 0 ? 'block' : 'none' }}>
                        <div>
                           <h4>Selected Files</h4>
                           {
                              this.state.selectedFiles.map((file, i) => {
                                 return <div className="selected-item" key={i}>
                                    <span>{`Name: ${file.name}`}<br /></span>
                                    <span>{`Size ${file.size}`}<br /></span>
                                    <span>{`Type ${file.size}`}<br /></span>
                                    <span>{`Last Modified Date: ${file.lastModifiedDate}`}</span>
                                 </div>;
                              })
                           }
                        </div>
                     </div>
                  </div>
               </Modal.Body>

               <Modal.Footer id="modal-footer">
                  <Button variant="secondary" onClick={this.props.hideUploadPopup}>
                     Close
                  </Button>
               </Modal.Footer>
            </Modal>
         </div>
      );
   }
}
