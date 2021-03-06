import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
   static displayName = Layout.name;

   render() {
      return (
         <div>
            <NavMenu isUserLoggedIn={this.props.isUserLoggedIn} userLogout={this.props.userLogout}/>
            <Container className="grid-table">
               {this.props.children}
            </Container>
         </div>
      );
   }
}
