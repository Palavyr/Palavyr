import * as React from "react";
import { Button, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

export interface ILoginActions {
    isLoading: boolean;
    openChangePasswordDialog: any;
}

const useStyles = makeStyles(theme => ({
    forgotPassword: {
        marginTop: theme.spacing(2),
        color: theme.palette.primary.main,
        cursor: "pointer",

        "&:enabled:hover": {
            color: theme.palette.primary.dark,
        },

        "&:enabled:focus": {
            color: theme.palette.primary.dark,
        },
    },
    disabledText: {
        cursor: "auto",
        color: theme.palette.text.disabled,
    },
    submitButton: {
        color: theme.palette.common.white,
        borderRadius: "4px",
        backgroundColor: theme.palette.secondary.main,
        "&:hover": {
            backgroundColor: theme.palette.primary.dark,
            color: "white",
            borderRadius: "4px",
        },
    },
}));

export const LoginActions = ({ isLoading, openChangePasswordDialog }: ILoginActions) => {
    const cls = useStyles();
    return (
        <>
            <Button className={cls.submitButton} type="submit" fullWidth variant="contained" size="large">
                Submit
                {isLoading && <ButtonCircularProgress />}
            </Button>
            <PalavyrText
                align="center"
                className={classNames(cls.forgotPassword, isLoading ? cls.disabledText : null)}
                color="primary"
                onClick={isLoading ? null : openChangePasswordDialog}
                tabIndex={0}
                role="button"
                onKeyDown={event => {
                    // For screenreaders listen to space and enter events
                    if ((!isLoading && event.keyCode === 13) || event.keyCode === 32) {
                        openChangePasswordDialog();
                    }
                }}
            >
                Forgot Password?
            </PalavyrText>
        </>
    );
};
