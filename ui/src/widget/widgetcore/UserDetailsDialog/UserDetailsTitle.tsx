import React, { useContext } from "react";
import { DialogTitle, Typography, Box, useTheme } from "@material-ui/core";
import { WidgetContext } from "@widgetcore/context/WidgetContext";

export interface IDialogTitleWithCloseIcon {
    title: string;
    disablePadding?: boolean;
    paddingBottom?: number;
}

export const UserDetailsTitle = ({ paddingBottom, title, disablePadding }: IDialogTitleWithCloseIcon) => {
    const theme = useTheme();
    const { preferences } = useContext(WidgetContext);

    var dialogTitleStyles = {
        paddingBottom: paddingBottom ? (paddingBottom && disablePadding ? 0 : paddingBottom) : theme.spacing(2),
        paddingTop: disablePadding ? 0 : theme.spacing(2),
        width: "100%",
        color: preferences.chatFontColor,

    };

    if (disablePadding) {
        dialogTitleStyles = {
            ...dialogTitleStyles,
            ...{
                paddingLeft: 0,
                paddingRight: 0,
            },
        };
    }

    return (
        <DialogTitle style={dialogTitleStyles} disableTypography>
            <Box>
                <Typography align="center" variant="h5">
                    {title}
                </Typography>
            </Box>
        </DialogTitle>
    );
};
