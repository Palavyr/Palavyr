import * as React from "react";
import { CircularProgress, makeStyles, Tooltip, Typography } from "@material-ui/core";
import { SessionStorage } from "localStorage/sessionStorage";
import { DashboardContext } from "../DashboardContext";
import Fade from "@material-ui/core/Fade";
import { GeneralSettingsLoc } from "@Palavyr-Types";
import { useHistory } from "react-router-dom";
import { useEffect, useState } from "react";
import classNames from "classnames";
import { Align } from "../positioning/Align";
import { TOPBAR_MAX_HEIGHT } from "@constants";

const DETAILS_MAX_HEIGHT = TOPBAR_MAX_HEIGHT - 10;

const useStyles = makeStyles((theme) => ({
    logwrapper: {
        // padding: "0px",
        // margin: "0px",
        maxHeight: `${DETAILS_MAX_HEIGHT}px`,
        border: `1px solid ${theme.palette.info.main}`,
        borderRadius: "8px",
        boxShadow: theme.shadows[20],
        backgroundColor: theme.palette.success.light,
        "&:hover": {
            backgroundColor: theme.palette.success.main,
            cursor: "pointer",
        },
    },
    googleImage: {
        borderRadius: "50%",
        maxHeight: `${DETAILS_MAX_HEIGHT}px`,
        padding: ".5rem",
    },
    toolTipInternal: {
        backgroundColor: theme.palette.primary.light,
        maxWidgth: "none",
        zIndex: 9999,
    },
    itemAlign: {
        display: "inline-block",
        verticalAlign: "middle",
    },
    text: {
        marginLeft: "0.5rem",
        marginRight: "1rem",
        color: theme.palette.common.black,
    },
    default: {
        padding: "0.5rem",
        display: "flex",
    },
}));

export const UserDetails = React.memo(() => {
    const cls = useStyles();
    const history = useHistory();

    const email = SessionStorage.getEmailAddress();
    const googleImage = SessionStorage.getGoogleImage();
    const [loading, setLoading] = useState<boolean>(true);

    const { planTypeMeta, setViewName } = React.useContext(DashboardContext);

    let details: JSX.Element;
    if (googleImage && email) {
        details = (
            <Align verticalCenter>
                <img src={googleImage} alt="" className={classNames(cls.googleImage, cls.itemAlign)} />
                {planTypeMeta && <Typography className={classNames(cls.text, cls.itemAlign)}>Subscription: {planTypeMeta.planType}</Typography>}
            </Align>
        );
    } else if (email) {
        details = (
            <Align verticalCenter>
                <span className={cls.default}>
                    <Typography className={cls.text} noWrap={true}>
                        {email}
                    </Typography>
                    {planTypeMeta && <Typography className={classNames(cls.text, cls.itemAlign)}>Subscription: {planTypeMeta.planType}</Typography>}
                </span>
            </Align>
        );
    } else {
        details = <Typography>Please Log In</Typography>;
    }
    const userOnClick = () => {
        setViewName("General Settings");
        history.push(`/dashboard/settings/email?tab=${GeneralSettingsLoc.email}`);
    };

    useEffect(() => {
        setTimeout(() => {
            setLoading(false);
        }, 1200);
    }, []);

    return (
        <Tooltip TransitionComponent={Fade} title={<Typography>{email}</Typography>} placement="right" classes={{ tooltip: cls.toolTipInternal }} interactive>
            <div onClick={userOnClick} className={cls.logwrapper}>
                {loading ? <CircularProgress /> : details}
            </div>
        </Tooltip>
    );
});
