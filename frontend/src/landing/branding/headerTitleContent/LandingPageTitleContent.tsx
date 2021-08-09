import { TitleContent } from "@landing/components/TitleContent";
import { Button, makeStyles, Typography } from "@material-ui/core";
import React from "react";
import { SubtitleTypography } from "./components/SubtitleTypography";
import { TitleTypography } from "./components/TitleTypography";

const useStyles = makeStyles((theme) => ({
    button: {
        width: "18rem",
        alignSelf: "center",
        backgroundColor: theme.palette.background.default,
        color: theme.palette.common.black,
        "&:hover": {
            backgroundColor: theme.palette.success.light,
            color: theme.palette.common.black,
        },
    },
}));

export interface LandingPageTitleContentProps {}

export const LangingPageTitleContent = ({}: LandingPageTitleContentProps) => {
    const cls = useStyles();
    return (
        <TitleContent title={<TitleTypography>Build Your Own Chat Bot</TitleTypography>} subtitle={<SubtitleTypography>No Programing Required</SubtitleTypography>}>
            {/* <Button className={cls.button} variant="contained" onClick={openRegisterDialog}>
                <Typography variant="h6">Create a FREE account</Typography>
            </Button> */}
        </TitleContent>
    );
};
