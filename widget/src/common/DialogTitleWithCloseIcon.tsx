import React from "react";
import { IconButton, DialogTitle, Typography, Box, useTheme, makeStyles } from "@material-ui/core";
import CheckCircleOutlineIcon from "@material-ui/icons/CheckCircleOutline";

export interface IDialogTitleWithCloseIcon {
    onClose: any;
    title: string;
    disablePadding?: boolean;
    paddingBottom?: number;
    detailsSet: boolean;
}

const useStyles = makeStyles(theme => ({}));

export const DialogTitleWithCloseIcon = ({ paddingBottom, onClose, title, disablePadding, detailsSet }: IDialogTitleWithCloseIcon) => {
    const theme = useTheme();

    var dialogTitleStyles = {
        paddingBottom: paddingBottom ? (paddingBottom && disablePadding ? 0 : paddingBottom) : theme.spacing(3),
        paddingTop: disablePadding ? 0 : theme.spacing(2),
        width: "100%",
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
            <Box >
                {/* display="flex" justifyContent="space-between" textAlign="center">*/}
                <Typography align="center" variant="h5">
                    {title}
                </Typography>
                {/* {detailsSet && (
                    <IconButton type="submit" component="button" onClick={onClose} style={{ marginRight: -12, marginTop: -10 }} aria-label="Close">
                        <CheckCircleOutlineIcon />
                    </IconButton>
                )} */}
            </Box>
        </DialogTitle>
    );
};
