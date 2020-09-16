import React from "react";
import { Box, Typography } from "@material-ui/core";

export type PanelRange = 0 | 1 | 2 | 3 | 4 | 5;

interface ITabPanel {
    value: PanelRange;
    index: PanelRange;
    children: React.ReactNode;
}

export const TabPanel = ({ value, index, children }: ITabPanel) => {
    return (
        <div role="tabpanel" hidden={value !== index} id={`simple-tabpanel-${index}`} aria-labelledby={`simple-tab-${index}`}>
            {value === index && (
                <Box p={3}>
                    <Typography component={"span"}>{children}</Typography>
                </Box>
            )}
        </div>
    );
};

export const areaTabProps = (index: PanelRange) => {
    return {
        id: `simple-tab-${index}`,
        "aria-controls": `simple-tabpanel-${index}`,
    };
};
