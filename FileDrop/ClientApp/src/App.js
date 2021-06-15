import 'devextreme/dist/css/dx.common.css';
import 'devextreme/dist/css/dx.light.css';
import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { FetchFiles } from './components/FetchFiles';

import './custom.css'

export default class App extends Component {
   static displayName = App.name;

   render() {
      return (
         <Layout>
            <Route exact path='/' component={FetchFiles} />
         </Layout>
      );
   }
}
