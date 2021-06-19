import React, { Component } from 'react';

export class CreateImgPreview extends Component {

   constructor(props) {
      super(props);
   }

   render() {
      return (
         <img src={this.props.previewURL} alt="Preview not avaiable." width={this.props.width} height={this.props.height} />
      );
   }
}
