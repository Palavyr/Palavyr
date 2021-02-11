import { Button, Dialog, DialogContent, makeStyles } from "@material-ui/core";
import React, { useCallback, useState } from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import { LocaleMap, LocaleMapItem, UserDetails } from "src/types";
import { checkUserEmail, checkUserName, checkUserPhone, INVALID_PHONE } from "./UserDetailsCheck";
import CheckCircleOutlineIcon from "@material-ui/icons/CheckCircleOutline";
import { useLocation } from "react-router-dom";
import CreateClient from "src/client/Client";
import { useEffect } from "react";
import { UserDetailsTitle } from "./UserDetailsTitle";
import { NameForm } from "./FormInputs/NameForm";
import { EmailForm } from "./FormInputs/EmailForm";
import { LocaleSelector } from "./FormInputs/LocaleSelector";
import { PhoneForm } from "./FormInputs/PhoneForm";

export interface CollectDetailsFormProps {
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
    userDetailsDialogState: boolean;
    setUserDetailsDialogState: Dispatch<SetStateAction<boolean>>;
}

export interface BaseFormProps {
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
    status: string | null;
    setStatus: Dispatch<SetStateAction<string>>;
}

const useStyles = makeStyles(theme => ({
    baseDialog: {
        zIndex: 9999,
        position: "absolute"

    },
    dialogBackground: {
        zIndex: 9999,
        backgroundColor: "white",
    },
    dialogPaper: {
        zIndex: 9999,
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingBottom: theme.spacing(3),
        maxWidth: 420,
    },
    dialogPaperScrollPaper: {
        maxHeight: "none",
    },
    dialogContent: {
        paddingTop: 0,
        paddingBottom: 0,
    },
    button: {
        margin: "0.5rem",
        textAlign: "center",
        marginTop: "1.3rem",
    },
}));

export const CollectDetailsForm = ({ userDetails, setUserDetails, userDetailsDialogState, setUserDetailsDialogState }: CollectDetailsFormProps) => {
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const client = CreateClient(secretKey);

    const [options, setOptions] = useState<LocaleMap>([]);
    const [phonePattern, setphonePattern] = useState<string>("");
    const [detailsSet, setDetailsSet] = useState<boolean>(false);

    useEffect(() => {
        (async () => {
            const { data: locale } = await client.Widget.Access.getLocale();
            setphonePattern(locale.localePhonePattern);
            setOptions(locale.localeMap);
        })();
    }, []);

    const cls = useStyles();
    const [status, setStatus] = useState<string | null>(null);

    const checkUserDetailsAreSet = (userDetails: UserDetails) => {
        const userNameResult = checkUserName(userDetails.userName, setStatus);
        const userEmailResult = checkUserEmail(userDetails.userEmail, setStatus);

        if (status === INVALID_PHONE) {
            setUserDetails({ ...userDetails, userPhone: "" });
        }

        if (!userNameResult || !userEmailResult) {
            return false;
        }
        return true;
    };

    const onChange = (event: any, newOption: LocaleMapItem) => {
        setphonePattern(newOption.phonePattern);
    };

    const onFormSubmit = async (e: { preventDefault: () => void }) => {
        e.preventDefault();
        setUserDetailsDialogState(false);
    };

    const formProps = {
        userDetails: userDetails,
        status: status,
        setStatus: setStatus,
        setUserDetails: setUserDetails,
    };

    return (
        <Dialog
            open={userDetailsDialogState}
            className={cls.baseDialog}
            classes={{
                root: cls.dialogBackground,
                paper: cls.dialogPaper,
                paperScrollPaper: cls.dialogPaperScrollPaper,
            }}
            disableBackdropClick
            hideBackdrop={false}
            disableEscapeKeyDown
        >
            <UserDetailsTitle title="Provide your contact details" />
            <DialogContent className={cls.dialogContent}>
                <form onSubmit={onFormSubmit}>
                    <NameForm {...formProps} />
                    <EmailForm {...formProps} setDetailsSet={setDetailsSet} checkUserDetailsAreSet={checkUserDetailsAreSet} />
                    <PhoneForm {...formProps} phonePattern={phonePattern} />
                    <LocaleSelector options={options} onChange={onChange} />
                    <div style={{ display: "flex", justifyContent: "center" }}>
                        <Button className={cls.button} endIcon={detailsSet && <CheckCircleOutlineIcon />} type="submit">
                            Begin
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    );
};
