import { currentEnvironment, isDevelopmentStage, softwareVersion } from "@api-client/clientUtils";
import { Typography } from "@material-ui/core";
import React from "react";

export const DevStagingStrip = () => {
    const text = `This is ${currentEnvironment}` + (softwareVersion ?  `: ${softwareVersion}` : "");
    const isDev = isDevelopmentStage();
    console.log("Is Dev: "  + isDev);

    return (
        <>
            {isDev && (
                <div style={{ height: "75px", paddingTop: "10px", backgroundColor: "lightblue", textAlign: "center" }}>
                    <Typography variant="h5">{text}</Typography>
                    <Typography>
                        This is a test environment. If you are not developing, please go to <a href="http://www.palavyr.com">www.palavyr.com</a>
                    </Typography>
                </div>
            )}
        </>
    );
};
