import { Button, Dialog, DialogContent, makeStyles } from "@material-ui/core";
import React, { useCallback, useState } from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import { LocaleMap, LocaleMapItem, UserDetails } from "src/types";
import { checkUserEmail, checkUserName, checkUserPhone } from "./UserDetailsCheck";
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
    detailsSet: boolean;
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
    userDetailsDialogState: boolean;
    setUserDetailsDialogState: Dispatch<SetStateAction<boolean>>;
}

export interface BaseFormProps {
    userDetails: UserDetails;
    status: string | null;
    setStatus: Dispatch<SetStateAction<string>>;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
}

const useStyles = makeStyles(theme => ({
    dialogBackground: {
        backgroundColor: "white",
    },
    dialogPaper: {
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
    },
}));

export const CollectDetailsForm = ({ detailsSet, setDetailsSet, userDetails, setUserDetails, userDetailsDialogState, setUserDetailsDialogState }: CollectDetailsFormProps) => {
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const client = CreateClient(secretKey);

    // const [currentOption, setCurrentOption] = useState<LocaleMapItem | undefined>();
    const [options, setOptions] = useState<LocaleMap>([]);
    const [phonePattern, setphonePattern] = useState<string>("");

    const loadLocales = useCallback(async () => {
        const { data: locale } = await client.Widget.Access.getLocale();
        setphonePattern(locale.localePhonePattern);
        setOptions(locale.localeMap);
    },[])

    useEffect(() => {
        loadLocales();
    //     (async () => {
    //         const { data: locale } = await client.Widget.Access.getLocale();
    //         setphonePattern(locale.localePhonePattern);
    //         setOptions(locale.localeMap);
    //     })();
    }, []);

    const cls = useStyles();
    const [status, setStatus] = useState<string | null>(null);
    // const [phoneHasErrored, setPhoneHasErrored] = useState<boolean>(false);
    // const [emailHasErrored, setEmailHasErrored] = useState<boolean>(false);
    // const [nameHasErrored, setNameHasErrored] = useState<boolean>(false);

    // const checkUserDetailsAreSet = (userDetails: UserDetails) => {
    //     const userNameResult = checkUserName(userDetails.userName, setStatus);
    //     const userEmailResult = checkUserEmail(userDetails.userEmail, setStatus);
    //     const userPhoneResult = checkUserPhone(userDetails.userPhone, setStatus);

    //     if (phoneHasErrored) {
    //         setUserDetails({ ...userDetails, userPhone: "" });
    //     }

    //     if (!userNameResult || !userEmailResult) {
    //         return false;
    //     }
    //     return true;
    // };

    const onChange = (event: any, newOption: LocaleMapItem) => {
        // setCurrentOption(newOption);
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
                    <EmailForm {...formProps} />
                    <PhoneForm {...formProps} phonePattern={phonePattern} />
                    <LocaleSelector options={options} onChange={onChange} />
                    {detailsSet && (
                        <Button className={cls.button} endIcon={detailsSet && <CheckCircleOutlineIcon />} type="submit">
                            Begin Chat
                        </Button>
                    )}
                </form>
            </DialogContent>
        </Dialog>
    );
};
