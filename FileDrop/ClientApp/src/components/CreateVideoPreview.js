import React, { Component } from 'react';

export class CreateVideoPreview extends Component {

   constructor(props) {
      super(props);
   }

   render() {
      let videoType = `video/${this.props.type}`;
      return (
         <video width={this.props.width} height={this.props.height} controls>
            <source src={this.props.previewURL} type={videoType}/>
                  Preview not avaiable.
         </video>

      );
   }
}
