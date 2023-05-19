import React from "react";
import { Button, makeStyles } from "@material-ui/core";
import classNames from "classnames";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    button: {
        marginLeft: "0.6rem",
        marginRight: "0.6rem",
        background: "white",
        "&:hover": {
            background: theme.palette.success.main,
        },
    },
    saveButton: {
        border: `1px solid ${theme.palette.primary}`,
        borderRadius: "10px",
        background: `${theme.palette.primary}`,
    },
    buttonContainer: {
        width: "100%",
        display: "flex",
        flexDirection: "row",
        justifyContent: "center",
        margin: "1.2rem",
    },
}));

export interface EnquiryBehaviorButtonsProps {
    toggleSelectAll: () => void;
    markAsSeen: () => Promise<void>;
    markAsUnseen: () => Promise<void>;
    deleteSelected: () => Promise<void>;
}

export const EnquiryBehaviorButtons = ({ toggleSelectAll, markAsSeen, markAsUnseen, deleteSelected }: EnquiryBehaviorButtonsProps) => {
    const cls = useStyles();
    return (
        <div className={cls.buttonContainer}>
            <Button type={"button"} variant="outlined" className={classNames(cls.button, cls.saveButton)} onClick={toggleSelectAll} size="medium">
                Select / Unselect All
            </Button>
            <Button type={"button"} variant="outlined" className={classNames(cls.button, cls.saveButton)} onClick={async () => await markAsSeen()} size="medium">
                Mark as Seen
            </Button>

            <Button type={"button"} variant="outlined" className={classNames(cls.button, cls.saveButton)} onClick={async () => await markAsUnseen()} size="medium">
                Mark As Unseen
            </Button>
            <Button type={"button"} variant="outlined" className={classNames(cls.button, cls.saveButton)} onClick={() => deleteSelected()} size="medium">
                Delete Selected
            </Button>
        </div>
    );
};
