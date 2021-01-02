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
          <Route exact path="/widget" component={App} />
        </Router>
      </MuiThemeProvider>
  </React.StrictMode>,
  document.getElementById('root')
);
