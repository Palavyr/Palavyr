import * as React from "react";
import { makeStyles, Tooltip, Typography } from "@material-ui/core";
import { SessionStorage } from "localStorage/sessionStorage";
import { DashboardContext } from "../DashboardContext";
import Fade from "@material-ui/core/Fade";
import { GeneralSettingsLoc } from "@Palavyr-Types";
import { useHistory } from "react-router-dom";

const useStyles = makeStyles((theme) => ({
    logwrapper: {
        textAlign: "center",
        paddingTop: "1rem",
        paddingBottom: "0.7rem",
        border: `1px solid ${theme.palette.info.main}`,
        margin: "1rem",
        boxShadow: theme.shadows[20],
        borderRadius: "7px",
        backgroundColor: theme.palette.success.light,
        "&:hover": {
            backgroundColor: theme.palette.success.main,
        },
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
        backgroundColor: theme.palette.primary.light,
        maxWidgth: "none",
        marginLeft: "2rem",
    },
}));

export const UserDetails = () => {
    const cls = useStyles();
    const history = useHistory();

    const email = SessionStorage.getEmailAddress();
    const googleImage = SessionStorage.getGoogleImage();
    const { subscription, setViewName } = React.useContext(DashboardContext);

    let details: JSX.Element;
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
    const userOnClick = () => {
        setViewName("General Settings");
        history.push(`/dashboard/settings/password?tab=${GeneralSettingsLoc.password}`);
    };
    return (
        <div onClick={userOnClick} className={cls.logwrapper}>
            {details}
        </div>
    );
};
