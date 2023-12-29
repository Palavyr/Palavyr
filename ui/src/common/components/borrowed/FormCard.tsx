import React from "react";
import { DialogContent, Box, makeStyles, Card } from "@material-ui/core";
import { LoginAndRegisterButtons } from "./DialogTitleWithCloseIcon";

export interface IFormDialog {
    onFormSubmit: any;
    content: React.ReactElement;
    actions: React.ReactElement;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    dialogPaper: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingBottom: theme.spacing(3),
        maxWidth: 420,
    },
    actions: {
        marginTop: theme.spacing(2),
    },
    dialogPaperScrollPaper: {
        maxHeight: "none",
    },
    dialogContent: {
        paddingTop: 0,
        paddingBottom: 0,
    },
    card: {
        boxShadow: "none",
    },
}));

export const FormCard = ({ onFormSubmit, content, actions }: IFormDialog) => {
    const cls = useStyles();

    return (
        <Card className={cls.card}>
            <LoginAndRegisterButtons />
            <DialogContent className={cls.dialogContent}>
                <form onSubmit={onFormSubmit}>
                    <div>{content}</div>
                    <Box width="100%" className={cls.actions}>
                        {actions}
                    </Box>
                </form>
            </DialogContent>
        </Card>
    );
};
