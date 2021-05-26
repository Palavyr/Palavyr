import React from "react";
import { Route, Redirect, useLocation } from "react-router-dom";
import auth from "../auth/Auth";

export const ProtectedRoute = ({ component: Component, ...rest }) => {

    const location = useLocation();

    const confirmationLoc = "/dashboard/confirm";
    const unauthLoc = "/";
    const welcome = "/dashboard/welcome";

    return (
        <Route
            {...rest}
            render={(props) => {

                if (auth.accountIsAuthenticated && auth.accountIsActive) {
                    if (location.pathname == confirmationLoc) {
                        return <Redirect to={welcome} />
                    }
                    return <Component {...props} />;
                }

                if (auth.accountIsAuthenticated && !auth.accountIsActive) {
                    if (location.pathname == confirmationLoc) {
                        return <Component {...props} />;
                    } else {
                        return <Redirect to={confirmationLoc} />;
                    }
                }

                if (!auth.accountIsAuthenticated) {
                    return <Redirect to={unauthLoc} from={location.pathname} />;
                }
                return <Redirect to={unauthLoc} from={location.pathname} />;
            }}
        />
    );
};
