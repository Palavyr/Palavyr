import React from "react";
import { Route, Redirect, useLocation } from "react-router-dom";
import auth from "../auth/Auth";

export const ProtectedRoute = ({ component: Component, ...rest }) => {

    const location = useLocation();

    return (
        <Route
            {...rest}
            render={(props) => {
                if (auth.isAuthenticated()) {
                    return <Component {...props} />;
                } else {
                    return <Redirect to="/login" from={location.pathname} />;
                }
            }}
        />
    );
};
