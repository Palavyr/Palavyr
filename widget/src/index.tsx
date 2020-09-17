import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import { App } from './App';
import { CssBaseline, MuiThemeProvider } from '@material-ui/core';
import theme from "./theme";


ReactDOM.render(
  <React.StrictMode>
    <MuiThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        {/* Remove This path for production */}
        {/* <Route path="/" render={() => <Redirect to="/widget/abc123" />} /> */}

        {/* This is the prod path */}
        <Route exact path="/widget/:secretKey" component={App} />
      </Router>

    </MuiThemeProvider>
  </React.StrictMode>,
  document.getElementById('root')
);
