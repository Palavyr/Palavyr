import { DividerWithText } from '@common/components/DividerWithText';
import { Typography } from '@material-ui/core';
import { Align } from 'dashboard/layouts/positioning/Align';
import { SpaceEvenly } from 'dashboard/layouts/positioning/SpaceEvenly';
import React from 'react'
import OctopusLogo from "./octopusLogo.svg";


export const DeployedWith = () => {
    return (
        <Align>
            <div style={{ width: "100%", marginTop: "2rem" }}>
                <DividerWithText
                    textComponent={
                        <Typography style={{ flexGrow: 1, transform: "translate(0px, 7px) scale(1)" }} align="center">
                            Proudly deployed using
                        </Typography>
                    }
                />
                <br></br>
                <SpaceEvenly center>
                    <div style={{ width: "50%" }}>
                        <OctopusLogo style={{ cursor: "pointer" }} onClick={() => window.open("https://www.octopus.com", "_blank")} />
                    </div>
                </SpaceEvenly>
            </div>
        </Align>
    );
};
