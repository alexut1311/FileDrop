import React, { Component } from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import Link from '@material-ui/core/Link';
import Box from '@material-ui/core/Box';
import Grid from '@material-ui/core/Grid';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import './Login.css';

export class Login extends Component {

   constructor(props) {
      super(props);
   }

   async handleSubmit(event) {
      event.preventDefault();
      debugger;
      let accountViewModel = {
         userName: document.getElementById("username").value,
         password: document.getElementById("password").value
      }
      const response = await fetch("https://localhost:44322/account/login", {
         method:'POST',
         headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
         },
         body: JSON.stringify(accountViewModel)
      });
      const data = await response.json();
   }
   
   render() {
      return (
         <Grid container component="main" className="root">
            <CssBaseline />
            <Grid item xs={false} sm={4} md={7} className="image" />
            <Grid item xs={12} sm={8} md={5} elevation={6} className="login-background">
               <div className="paper">
                  <Avatar className="avatar">
                     <LockOutlinedIcon />
                  </Avatar>
                  <Typography component="h1" variant="h5">
                     Sign in
                  </Typography>
                  <form className="form" onSubmit={this.handleSubmit.bind(this)}>
                     <TextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        id="username"
                        label="Username"
                        name="text"
                        autoFocus
                     />
                     <TextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        name="password"
                        label="Password"
                        type="password"
                        id="password"
                     />
                     <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        color="primary"
                        className="submit"
                     >
                        Sign In
                     </Button>
                     <Grid container>
                        <Grid item xs>
                        </Grid>
                        <Grid item>
                           <Link href="/Register" variant="body2">
                              {"Don't have an account? Sign Up"}
                           </Link>
                        </Grid>
                     </Grid>
                     <Box mt={5}>
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
