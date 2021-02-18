import { Button, Dialog, DialogContent, makeStyles } from "@material-ui/core";
import React, { useState } from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import { LocaleMap, LocaleMapItem } from "src/types";
import CheckCircleOutlineIcon from "@material-ui/icons/CheckCircleOutline";
import { useLocation } from "react-router-dom";
import { useEffect } from "react";
import { UserDetailsTitle } from "./UserDetailsTitle";
import { NameForm } from "./FormInputs/NameForm";
import { EmailForm } from "./FormInputs/EmailForm";
import { LocaleSelector } from "./FormInputs/LocaleSelector";
import { PhoneForm } from "./FormInputs/PhoneForm";
import CreateClient from "src/client/Client";
import { closeUserDetails, setRegionContext } from "src/widgetCore/store/dispatcher";
import { INVALID_EMAIL, INVALID_NAME, INVALID_PHONE } from "./UserDetailsCheck";
import { useSelector } from "react-redux";
import { GlobalState } from "src/widgetCore/store/types";

export interface CollectDetailsFormProps {
    chatStarted: boolean;
    setChatStarted: Dispatch<SetStateAction<boolean>>;
}

export interface BaseFormProps {
    status: string | null;
    setStatus: Dispatch<SetStateAction<string>>;
}

const useStyles = makeStyles(theme => ({
    baseDialog: {
        zIndex: 9999,
        position: "absolute",
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

export const CollectDetailsForm = ({ chatStarted, setChatStarted }: CollectDetailsFormProps) => {
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const client = CreateClient(secretKey);
    const userDetailsVisible = useSelector((state: GlobalState) => state.behavior.userDetailsVisible);

    const [options, setOptions] = useState<LocaleMap>([]);
    const [phonePattern, setphonePattern] = useState<string>("");
    const [detailsSet, setDetailsSet] = useState<boolean>(false);

    useEffect(() => {
        (async () => {
            const { data: locale } = await client.Widget.Access.getLocale();
            setphonePattern(locale.localePhonePattern);
            setOptions(locale.localeMap);
            setRegionContext(locale.localeId);
        })();
    }, []);

    const cls = useStyles();
    const [status, setStatus] = useState<string | null>(null);

    const onChange = (event: any, newOption: LocaleMapItem) => {
        setphonePattern(newOption.phonePattern);
        setRegionContext(newOption.localeId);
    };

    const onFormSubmit = (e: { preventDefault: () => void }) => {
        e.preventDefault();
        setChatStarted(true);
        closeUserDetails();
    };

    const formProps = {
        status,
        setStatus,
    };

    return (
        <Dialog
            open={userDetailsVisible}
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
                    <EmailForm {...formProps} setDetailsSet={setDetailsSet} />
                    <PhoneForm {...formProps} phonePattern={phonePattern} />
                    <LocaleSelector options={options} onChange={onChange} />
                    <div style={{ display: "flex", justifyContent: "center" }}>
                        <Button disabled={status === INVALID_PHONE || status === INVALID_EMAIL || status === INVALID_NAME} className={cls.button} endIcon={detailsSet && <CheckCircleOutlineIcon />} type="submit">
                            {chatStarted ? "Continue" : "Begin"}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    );
};