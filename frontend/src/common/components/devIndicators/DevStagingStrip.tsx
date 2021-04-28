import { currentEnvironment, isDevelopmentStage, softwareVersion } from "@api-client/clientUtils";
import { makeStyles, Typography } from "@material-ui/core";
import React, { useState } from "react";
import { SinglePurposeButton } from "../SinglePurposeButton";

const useStyles = makeStyles((theme) => ({
    devStripContainer: {
        // height: "75px",
        paddingTop: "10px",
        backgroundColor: theme.palette.warning.main,
        textAlign: "center",
    },
}));
export const DevStagingStrip = () => {
    const cls = useStyles();
    const text = `This is ${currentEnvironment}` + (softwareVersion ? `: ${softwareVersion}` : "");
    const isDev = isDevelopmentStage();
    const [show, setShow] = useState<boolean>(isDev);
    return (
        <>
            {isDev && show && false && (
                <>
                    <div className={cls.devStripContainer}>
                        <Typography variant="h5">{text}</Typography>
                        <Typography>
                            This is a test environment. If you are not developing, please go to <a href="http://www.palavyr.com">www.palavyr.com</a>
                        </Typography>
                        <SinglePurposeButton variant="contained" color="primary" buttonText="Hide" onClick={() => setShow(!show)} />
                    </div>
                </>
            )}
        </>
    );
};
