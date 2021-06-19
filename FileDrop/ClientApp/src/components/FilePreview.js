import React, { Component } from 'react';
import { CreateImgPreview } from './CreateImgPreview';
import { CreateVideoPreview } from './CreateVideoPreview';
import './preview.css';

export class FilePreview extends Component {

   constructor(props) {
      super(props);
      this.dataSource = props.data;
      this.extension = this.dataSource.data.previewURL.split('?')[0].split('/')[3].split(".")[1];
   }

   createPreview(previewURL, extension) {
      const images = ["jpg", "gif", "png"]
      const videos = ["mp4", "3gp", "ogg"]

      if (images.includes(extension)) {
         return <CreateImgPreview previewURL={previewURL} width={680} height={420} />;
      } else if (videos.includes(extension)) {
         return <CreateVideoPreview previewURL={previewURL} type={extension} width={680} height={420} />;
      }
      return "Preview not available";
   }

   render() {
      let contents = this.createPreview(this.dataSource.data.previewURL, this.extension);
      return (
         <div className="display-center">
            {contents}
         </div>
      );
   }
}
