import { currentEnvironment } from "@api-client/clientUtils";
import { Typography } from "@material-ui/core";
import { PRODUCTION } from "@Palavyr-Types";
import React from "react";

export const DevStagingStrip = () => {
    return (
        <>
            {currentEnvironment.toUpperCase() !== PRODUCTION.toUpperCase() ? (
                <div style={{ height: "75px", paddingTop: "10px", backgroundColor: "lightblue", textAlign: "center" }}>
                    <Typography variant="h3" >This is {currentEnvironment}</Typography>
                </div>
            ) : null}
        </>
    );
};
