import React from "react";
import { IconButton, DialogTitle, Typography, Box, useTheme } from "@material-ui/core";
import CloseIcon from "@material-ui/icons/Close";
import CheckCircleOutlineIcon from '@material-ui/icons/CheckCircleOutline';

export interface IDialogTitleWithCloseIcon {
    onClose: any,
    title: string,
    disablePadding?: boolean
    disabled?: boolean,
    paddingBottom?: number,
    detailsSet: boolean,
}

export const DialogTitleWithCloseIcon = ({ paddingBottom, onClose, disabled, title, disablePadding, detailsSet }: IDialogTitleWithCloseIcon) => {

    const theme = useTheme();

    var dialogTitleStyles = {
        paddingBottom: paddingBottom ? paddingBottom && disablePadding ? 0 : paddingBottom : theme.spacing(3),
        paddingTop: disablePadding ? 0 : theme.spacing(2),
        width: "100%"
    }

    if (disablePadding) {
        dialogTitleStyles = {
            ...dialogTitleStyles,
            ...{
                paddingLeft: 0,
                paddingRight: 0,
            }
        }
    }

    return (
        <DialogTitle style={dialogTitleStyles} disableTypography>
            <Box display="flex" justifyContent="space-between">
                <Typography variant="h5">{title}</Typography>
                <IconButton
                    onClick={onClose}
                    style={{ marginRight: -12, marginTop: -10 }}
                    disabled={disabled}
                    aria-label="Close"
                >
                    {detailsSet && <CheckCircleOutlineIcon />}
                </IconButton>
            </Box>
        </DialogTitle>
    );
}
