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
   constructor(props) {
      super(props);
      this.state = {
         userJWToken: Cookies.get('fileDropAuthenticationToken'),
         userRefreshToken: Cookies.get('fileDropAuthenticationRefreshToken'),
         isUserLoggedIn: false
      }
      this.userLogin = this.userLogin.bind(this);
      this.userLogout = this.userLogout.bind(this);

   }
   
   componentDidMount() {
      if (this.state.userJWToken && this.state.userRefreshToken) {
         this.setState({
            isUserLoggedIn: true
         });
      }
      fetch("https://localhost:44322/account/isUserLoggedIn", {
         method: 'GET',
         headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
         },
      }).then((response) => response.json())
         .then((responseJson) => {
            this.setState({
               isUserLoggedIn: responseJson
            });
         })
         .catch((error) => {
            console.error(error);
         });
   }

   userLogin() {
      this.setState({
         isUserLoggedIn: true
      });
   }

   userLogout() {
      Cookies.remove('fileDropAuthenticationToken');
      Cookies.remove('fileDropAuthenticationRefreshToken');
      this.setState({
         userJWToken: undefined,
         userRefreshToken: undefined,
         isUserLoggedIn: false
      });
   }

   render() {
      if (!this.state.isUserLoggedIn) {
         return (
            <div>
               <Route exact path='/' component={() => (<Login userLogin={this.userLogin} />)} />
               <Route path='/Register' component={Register} />
            </div>
         );
      }
      return (
         <Layout isUserLoggedIn={this.state.isUserLoggedIn} userLogout={this.userLogout}>
            <Route exact path='/' component={FetchFiles} />
         </Layout>
      );
   }
}
