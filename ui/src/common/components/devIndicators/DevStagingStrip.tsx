import { currentEnvironment, softwareVersion } from "@common/client/clientUtils";
import { makeStyles, Typography } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
import React from "react";
import { SinglePurposeButton } from "../SinglePurposeButton";

const useStyles = makeStyles<{}>((theme: any) => ({
    devStripContainer: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
        backgroundColor: theme.palette.warning.main,
        textAlign: "center",
    },
}));

export interface DevStagingStripProps {
    show: boolean;
    setShow: SetState<boolean>;
}

export const DevStagingStrip = ({ show, setShow }: DevStagingStripProps) => {
    const cls = useStyles();
    const text = `This is ${currentEnvironment}` + (softwareVersion ? `: ${softwareVersion}` : "");
    return (
        <>
            {show && (
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
