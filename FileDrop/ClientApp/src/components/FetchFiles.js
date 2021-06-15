import React, { Component } from 'react';

export class FetchFiles extends Component {
   static displayName = FetchFiles.name;

   constructor(props) {
      super(props);
      this.state = { files: [], loading: true };
   }

   componentDidMount() {
      this.populateWeatherData();
   }

   static renderFilesTable(files) {
      return (
         <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
               <tr>
                  <th>File Name</th>
                  <th>File Size</th>
               </tr>
            </thead>
            <tbody>
               {files.map(file =>
                  <tr key={file.fileName}>
                     <td>{file.fileName}</td>
                     <td>{file.fileSize}</td>
                  </tr>
               )}
            </tbody>
         </table>
      );
   }

   render() {
      let contents = this.state.loading ? <p><em>Loading...</em></p> : FetchFiles.renderFilesTable(this.state.files);

      return (
         <div>
            <h1 id="filesLabel">Files</h1>
            {contents}
         </div>
      );
   }

   async populateWeatherData() {
      const response = await fetch('file/GetFiles');
      const data = await response.json();
      this.setState({ files: data, loading: false });
   }
}
