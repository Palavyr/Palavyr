import React from "react";
import { withRouter } from "react-router-dom";
import ReactGA from "react-ga";

const RouteChangeTracker = ({ history }) => {

    history.listen((location: Location, action: any) => {
        ReactGA.set({ page: location.pathname });
        ReactGA.pageview(location.pathname);

    });

    return <></>;
};

export default withRouter(RouteChangeTracker);
