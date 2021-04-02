import { currentEnvironment, softwareVersion } from "@api-client/clientUtils";
import { Typography } from "@material-ui/core";
import { PRODUCTION } from "@Palavyr-Types";
import React from "react";

export const DevStagingStrip = () => {

    const pre = `This is ${currentEnvironment}`
    var text = softwareVersion ? pre + `: ${softwareVersion}` : pre;

    return (
        <>
            {currentEnvironment.toUpperCase() !== PRODUCTION.toUpperCase() ? (
                <div style={{ height: "75px", paddingTop: "10px", backgroundColor: "lightblue", textAlign: "center" }}>
                    <Typography variant="h5" >{text}</Typography>
                    <Typography>This is a test environment.</Typography>
                </div>
            ) : null}
        </>
    );
};
