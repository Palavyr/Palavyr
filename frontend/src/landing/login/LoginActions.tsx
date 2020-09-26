import * as React from 'react';
import { Button, Typography, makeStyles } from '@material-ui/core';
import classNames from 'classnames';
import { ButtonCircularProgress } from '@common/components/borrowed/ButtonCircularProgress';


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
}));


export const LoginActions = ({ isLoading, openChangePasswordDialog }: ILoginActions) => {

    const classes = useStyles();

    return (
        <>
            <Button
                type="submit"
                fullWidth
                variant="contained"
                color="secondary"
                disabled={isLoading}
                size="large"
            >
                Login
          {isLoading && <ButtonCircularProgress />}
            </Button>
            <Typography
                align="center"
                className={classNames(
                    classes.forgotPassword,
                    isLoading ? classes.disabledText : null
                )}
                color="primary"
                onClick={isLoading ? null : openChangePasswordDialog}
                tabIndex={0}
                role="button"
                onKeyDown={(event) => {
                    // For screenreaders listen to space and enter events
                    if (
                        (!isLoading && event.keyCode === 13) ||
                        event.keyCode === 32
                    ) {
                        openChangePasswordDialog();
                    }
                }}
            >
                Forgot Password?
        </Typography>
        </>
    )
}