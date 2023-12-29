import React from "react";
import { makeStyles } from "@material-ui/core";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";


const useStyles = makeStyles<{}>((theme: any) => ({
    customizetext: {
        paddingTop: "1.rem",
    },
}));

export const ChatDemoHeader = () => {
    const cls = useStyles();
    return (
        <>
            <PalavyrText gutterBottom align="center" variant="h4" className={cls.customizetext}>
                Test your chat bot!
            </PalavyrText>
        </>
    );
};
