import React, { Component } from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import Link from '@material-ui/core/Link';
import Box from '@material-ui/core/Box';
import Grid from '@material-ui/core/Grid';
import PersonAddIcon from '@material-ui/icons/PersonAdd';
import Typography from '@material-ui/core/Typography';
import { LoadIndicator } from 'devextreme-react/load-indicator';
import { Redirect } from 'react-router';
import './Login.css';

export class Register extends Component {

   constructor(props) {
      super(props);
      this.state = {
         firstName: '',
         lastName: '',
         username: '',
         email: '',
         password: '',
         rePassword: '',
         password_error: false,
         shouldComponentUpdate: false,
         shouldRedirect: false,
         showLoader: false
      };
   }
   async handleSubmit(event) {
      event.preventDefault();
      let accountViewModel = {
         firstName: this.state.firstName,
         lastName: this.state.lastName,
         username: this.state.username,
         email: this.state.email,
         password: this.state.password,
         rePassword: this.state.rePassword,
      }
      this.showLoader();
      this.hideErrorMessage();
      fetch("https://localhost:44322/account/register", {
         method: 'POST',
         headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
         },
         body: JSON.stringify(accountViewModel)
         }).then((response) => response.json())
         .then((responseJson) => {
            console.log("then")
            this.hideLoader()
            if (!responseJson.isCompletedSuccesfully) {
               if (responseJson.statusCode !== 500) {
                  this.showErrorMessage(responseJson.message)
               }
            } else {
               this.setState({
                  shouldComponentUpdate: true,
                  shouldRedirect: true,
               });
               this.showSuccessMessage();
               setTimeout(
                  function () {
                     this.forceUpdate();
                  }.bind(this), 3000);
            }
         })
         .catch((error) => {
            this.hideLoader()
            console.log("catch")
            this.showErrorMessage("Something bad happened, try reloading the page.")
            console.error(error);
         });
   }

   showSuccessMessage() {
      var element = document.getElementById("successMessage");
      element.classList.remove("d-none");
   }

   showLoader() {
      var element = document.getElementById("loader");
      element.classList.remove("d-none");
   }

   hideLoader() {
      var element = document.getElementById("loader");
      element.classList.add("d-none");
   }

   showErrorMessage(message) {
      document.getElementById("errorMessage").innerText = message;
   }

   hideErrorMessage() {
      document.getElementById("errorMessage").innerText = '';
   }

   shouldComponentUpdate(nextProps, nextState) {
      return this.state.shouldComponentUpdate;
   }

   render() {
      if (this.state.shouldRedirect) {
         return <Redirect to='/' />;
      }
      return (
         <Grid container component="main" className="root">
            <CssBaseline />
            <Grid item xs={false} sm={4} md={7} className="image" />
            <Grid item xs={12} sm={8} md={5} elevation={6} className="login-background">
               <div className="paper">
                  <Avatar className="avatar">
                     <PersonAddIcon />
                  </Avatar>
                  <Typography component="h1" variant="h5">
                     Sign up
                  </Typography>
                  <form className="form" onSubmit={this.handleSubmit.bind(this)}>
                     <Grid container>
                        <Grid item xs={12} sm={5} md={5} className="margin-left-40">
                           <TextField
                              variant="outlined"
                              margin="normal"
                              required
                              fullWidth
                              id="firstName"
                              label="First name"
                              name="text"
                              autoFocus
                              onChange={e => this.setState({ firstName: e.target.value })}
                           />
                        </Grid>
                        <Grid item xs={12} sm={5} md={5} className="margin-left-40">
                           <TextField
                              variant="outlined"
                              margin="normal"
                              required
                              fullWidth
                              name="lastName"
                              label="Last name"
                              type="text"
                              id="lastName"
                              onChange={e => this.setState({ lastName: e.target.value })}
                           />
                        </Grid>
                     </Grid>
                     <Grid container>
                        <Grid item xs={12} sm={5} md={5} className="margin-left-40">
                           <TextField
                              variant="outlined"
                              margin="normal"
                              required
                              fullWidth
                              id="username"
                              label="Username"
                              name="text"
                              onChange={e => this.setState({ username: e.target.value })}
                           />
                        </Grid>
                        <Grid item xs={12} sm={5} md={5} className="margin-left-40">
                           <TextField
                              variant="outlined"
                              margin="normal"
                              required
                              fullWidth
                              name="email"
                              label="Email"
                              type="email"
                              id="email"
                              onChange={e => this.setState({ email: e.target.value })}
                           />
                        </Grid>
                     </Grid>
                     <Grid container>
                        <Grid item xs={12} sm={5} md={5} className="margin-left-40">
                           <TextField
                              variant="outlined"
                              margin="normal"
                              required
                              fullWidth
                              id="password"
                              label="Password"
                              type="password"
                              name="password"
                              onChange={e => this.setState({ password: e.target.value })}
                           />
                        </Grid>
                        <Grid item xs={12} sm={5} md={5} className="margin-left-40">
                           <TextField
                              variant="outlined"
                              margin="normal"
                              required
                              fullWidth
                              name="password"
                              label="Re-enter password"
                              type="password"
                              id="re-password"
                              error={this.state.password_error}
                              helperText={this.state.password_error_text}
                              onChange={e => this.setState({ rePassword: e.target.value, password_error: this.state.password !== e.target.value })}
                           />
                        </Grid>
                     </Grid>
                     <Button
                        type="submit"
                        variant="contained"
                        color="primary"
                        className="sign-up"
                     >
                        Sign up
                     </Button>
                     <Grid container className="sign-in-container">
                        <Grid item xs>
                        </Grid>
                        <Grid item className="sign-in">
                           <Link href="/" variant="body2" className="register-sign-in-message">
                              {"You have an account? Sign in instead."}
                           </Link>
                        </Grid>
                     </Grid>
                     <Box mt={5}>
                        <div id="loader" className="d-none">
                           <LoadIndicator className="d-flex justify-content-center loader-center " id="large-indicator" height={40} width={40} />
                        </div>
                        <p className="error" id="errorMessage"></p>
                        <div className="success d-none" id="successMessage">
                           <Typography variant="h5" color="inherit" align="center">
                              {'The acount was created. Redirecting to login in 3 seconds...'}
                           </Typography>
                        </div>
                        <Typography variant="body2" color="textSecondary" align="center">
                           {'Copyright © '}
                           <Link color="inherit" href="https://localhost:44322/">
                              FileDrop
                           </Link>{' '}
                           {new Date().getFullYear()}
                           {'.'}
                        </Typography>
                     </Box>
                  </form>
               </div>
            </Grid>
         </Grid>
      );
   }
}
