import React, { useState } from "react";
import { Alert } from "@material-ui/lab";
import Typography from "@material-ui/core/Typography";
import Divider from "@material-ui/core/Divider";
import { makeStyles } from "@material-ui/core";
import HelpIcon from '@material-ui/icons/Help';


interface IAlertInfoStatement {
    title: string;
    details?: string;
    children?: React.ReactNode;
    defaultOpen?: boolean;
    severity?: "info" | "success" | "warning" | "error" | undefined;
    /*
        Margin in rem
    */
    margin?: number;
    fullwidth?: boolean;
    titleSize?: "h2" | "h3" | "h4" | "h5" | "h6";
}

type StyleProps = {
    margin: number;
    padding: number;
}

const useStyle = makeStyles(theme => ({
    div: (props: StyleProps) => ({
        margin: `${props.margin}rem`,
        border: "2px solid darkgrey",
        borderRadius: "8px",
    }),
    icon: {
        margin: "1rem",
        "&:hover": {
            color: "gray"
        }
    }
}))


export const Statement = ({ title, details, children, defaultOpen, fullwidth = false, severity = "info", margin = 2, titleSize = "h4" }: IAlertInfoStatement) => {

    const classes = useStyle({ margin })
    const [isVisible, setIsVisible] = useState<boolean>(defaultOpen ? true : false);

    return (
        <>
            <div style={{ display: "flex", justifyContent: "flex-end" }}>
                {!isVisible && <HelpIcon className={classes.icon} onClick={() => setIsVisible(true)} />}
            </div>
            {
                isVisible &&
                (
                    <div className={classes.div}>
                        <Alert severity={severity} onClick={() => setIsVisible(false)}>
                            <Typography paragraph variant={titleSize}>
                                {title}
                            </Typography>
                            <Divider light variant={fullwidth ? 'fullWidth' : undefined} />
                            <Typography paragraph>
                                {details}
                            </Typography>
                            <Typography paragraph>
                                {children}
                            </Typography>
                        </Alert>
                    </div>
                )
            }
        </>
    )
}
