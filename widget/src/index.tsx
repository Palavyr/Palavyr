import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import { App } from './App';
import { CssBaseline, MuiThemeProvider } from '@material-ui/core';
import theme from "./theme";
import { StatusCheck } from './StatusCheck';

// type ErrorProps = {

// }

// type ErrorState = {
//   hasError: boolean;
// }

// class ErrorBoundary extends React.Component<ErrorProps, ErrorState> {
//   constructor(props: ErrorProps) {
//     super(props);
//     this.state = { hasError: false };
//   }

//   static getDerivedStateFromError(error) {
//     // Update state so the next render will show the fallback UI.
//     return { hasError: true };
//   }

//   componentDidCatch(error, errorInfo) {
//     // You can also log the error to an error reporting service
//     // logErrorToMyService(error, errorInfo);
//   }

//   render() {
//     if (this.state.hasError) {
//       // You can render any custom fallback UI
//       return <h1>Something went wrong.</h1>;
//     }

//     return this.props.children;
//   }
// }


ReactDOM.render(
  <React.StrictMode>
    {/* <ErrorBoundary> */}
      <MuiThemeProvider theme={theme}>
        <CssBaseline />
        <Router>
          <Route exact path="/widget" component={App} />
          <Route exact path="/widget/status-check" render={StatusCheck} />
        </Router>
      </MuiThemeProvider>
    {/* </ErrorBoundary> */}
  </React.StrictMode>,
  document.getElementById('root')
);
