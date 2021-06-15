import React, { Component } from 'react';
import { LoadIndicator } from 'devextreme-react/load-indicator';

export class Loader extends Component {

   constructor(props) {
      super(props);
   }

   render() {
      return (
         <div>
            <LoadIndicator className="d-flex justify-content-center loader-center " id="large-indicator" height={60} width={60} />

            <p className="d-flex justify-content-center">Please wait while the files are being loaded.</p>

         </div>
      );
   }
}
