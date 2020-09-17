import React from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { LandingPage } from "@landing/Landing";
import { ProtectedRoute } from "@protected-routes";
import { DashboardLayout } from "dashboard/layouts/DashboardLayout";

export const Routes = () => {
    return (
        <Router>
            <Switch>
                <Route exact path="/" component={LandingPage} />
                <ProtectedRoute exact path="/dashboard/:contentType/:areaIdentifier" component={DashboardLayout} />
                <ProtectedRoute exact path="/dashboard/:contentType/" component={DashboardLayout} />
                <ProtectedRoute exact path="/dashboard/" component={DashboardLayout} />
            </Switch>
        </Router>
    );
}
