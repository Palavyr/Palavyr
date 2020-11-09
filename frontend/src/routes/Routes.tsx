import React from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { LandingPage } from "@landing/Landing";
import { ProtectedRoute } from "@protected-routes";
import { DashboardLayout } from "dashboard/layouts/DashboardLayout";
import { Success } from "dashboard/content/purchse/success/Success";
import { Cancel } from "dashboard/content/purchse/cancel/Cancel";

export const Routes = () => {
    return (
        <Router>
            <Route exact path="/" component={LandingPage} />
            <ProtectedRoute exact path="/dashboard/" component={DashboardLayout} />
            <ProtectedRoute exact path="/dashboard/:contentType/" component={DashboardLayout} />
            <ProtectedRoute exact path="/dashboard/:contentType/:areaIdentifier" component={DashboardLayout} />
            {/* <ProtectedRoute exact path="/dashboard/:contentType/:successId" component={DashboardLayout} /> */}
            {/* <ProtectedRoute exact path="/dashboard/purchase/:productType/:productId" component={DashboardLayout} /> */}
            {/* <ProtectedRoute exact path="/dashboard/:contentType/payment/canceled" component={Success} /> */}
        </Router>
    );
}


// http://localhost:8080/dashboard/subscribe/payment/success?session_id=cs_test_ayJ8PxQkKgEXhtRY2IbctMLYoqOATIse5OLbYRyMWuk6ShqB9kkUbMRM