import 'devextreme/dist/css/dx.common.css';
import 'devextreme/dist/css/dx.light.css';
import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { FetchFiles } from './components/FetchFiles';
import { Login } from './components/Login';
import { Register } from './components/Register';
import Cookies from 'js-cookie';

import './custom.css'

export default class App extends Component {
   static displayName = App.name;
   state = {
      userJWToken: Cookies.get('fileDropAuthenticationToken'),
      userRefreshToken: Cookies.get('fileDropAuthenticationRefreshToken'),
      isUserLoggedIn: false
   }

   componentDidMount() {
      fetch('animals api path here')
         .then(result => result.json())
         .then((data) => {
            this.setState({ animals: data })
         })
         .catch(console.log)
   }

   render() {
      if (!this.state.isUserLoggedIn) {
         return (
            <div>
               <Route exact path='/' component={Login} />
               <Route path='/Register' component={Register} />
            </div>
         );
      }
      return (
         <Layout>
            <Route exact path='/' component={FetchFiles} />
         </Layout>
      );
   }
}
