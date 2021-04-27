import * as React from "react";
import { makeStyles, Tooltip, Typography } from "@material-ui/core";
import { SessionStorage } from "localStorage/sessionStorage";
import { DashboardContext } from "../DashboardContext";
import Fade from "@material-ui/core/Fade";

const useStyles = makeStyles((theme) => ({
    logwrapper: {
        textAlign: "center",
        paddingTop: "1rem",
        paddingBottom: "0.7rem",
        border: `1px solid ${theme.palette.info.main}`,
        margin: "1rem",
        boxShadow: theme.shadows[10],
        borderRadius: "7px",
        backgroundColor: theme.palette.warning.main,
    },
    googleImage: {
        borderRadius: "50%",
        margin: "10px",
        height: "75px",
        width: "75px",
    },
    googleImageContainer: {
        display: "flex",
        width: "100%",
        justifyContent: "center",
    },
    toolTipInternal: {
        backgroundColor: theme.palette.primary.main,
        maxWidgth: "none",
        marginLeft: "2rem"
    },
}));

export const UserDetails = () => {
    const cls = useStyles();

    const email = SessionStorage.getEmailAddress();
    const googleImage = SessionStorage.getGoogleImage();
    const { subscription } = React.useContext(DashboardContext);

    let details;

    if (googleImage && email) {
        details = (
            <>
                <Typography>Logged in as:</Typography>
                <Tooltip TransitionComponent={Fade} title={<Typography variant="h5">{email}</Typography>} placement="right" classes={{ tooltip: cls.toolTipInternal }} interactive>
                    <div className={cls.googleImageContainer}>
                        <img src={googleImage} alt="" className={cls.googleImage} />
                    </div>
                </Tooltip>
                <Typography variant="h6">Subscription: {subscription}</Typography>
            </>
        );
    } else if (email) {
        details = (
            <>
                <Typography>Logged in as:</Typography>
                <Typography gutterBottom noWrap={false}>
                    {email}
                </Typography>
                <Typography variant="h6">Subscription: {subscription}</Typography>
            </>
        );
    } else {
        details = <Typography>Please Log In</Typography>;
    }

    return <div className={cls.logwrapper}>{details}</div>;
};
