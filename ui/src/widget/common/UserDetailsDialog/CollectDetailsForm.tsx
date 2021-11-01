import { Button, Dialog, DialogContent, makeStyles, Typography } from "@material-ui/core";
import React, { useState } from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import CheckCircleOutlineIcon from "@material-ui/icons/CheckCircleOutline";
import { useLocation } from "react-router-dom";
import { useEffect } from "react";
import { UserDetailsTitle } from "./UserDetailsTitle";
import { NameForm } from "./FormInputs/NameForm";
import { EmailForm } from "./FormInputs/EmailForm";
import { LocaleSelector } from "./FormInputs/LocaleSelector";
import { PhoneForm } from "./FormInputs/PhoneForm";
import { useSelector } from "react-redux";
import { GlobalState, LocaleMap, LocaleResource, SetState } from "@Palavyr-Types";
import { setRegionContext, closeUserDetails, getNameContext, getEmailAddressContext, getPhoneContext, getRegionContext } from "@store-dispatcher";
import { INVALID_PHONE, INVALID_EMAIL, INVALID_NAME } from "./UserDetailsCheck";
import { PalavyrWidgetRepository } from "@api-client/PalavyrWidgetRepository";
import { WidgetContext } from "widget/context/WidgetContext";
import { useContext } from "react";

export interface CollectDetailsFormProps {
    setKickoff: SetState<boolean>;
}

export interface BaseFormProps {
    status: string | null;
    setStatus: Dispatch<SetStateAction<string>>;
}

const useStyles = makeStyles(theme => ({
    baseDialogCollectionForm: {
        position: "absolute",
    },
    dialogBackgroundCollectionForm: {
        backgroundColor: "rgba(255, 255, 255, 30)",
        // zIndex: 9999,
    },
    dialogPaperCollectionForm: {
        // zIndex: 9999,
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingBottom: theme.spacing(3),
        maxWidth: 420,
        backgroundColor: "rgba(255, 255, 255, 50)",
    },
    dialogPaperScrollPaperCollectionForm: {
        maxHeight: "none",
    },
    dialogContentCollectionForm: {
        paddingTop: 0,
        paddingBottom: 0,
    },
    buttonCollectionForm: {
        margin: "0.5rem",
        textAlign: "center",
        marginTop: "1.3rem",
    },
}));

export const CollectDetailsForm = ({ setKickoff }: CollectDetailsFormProps) => {
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const client = new PalavyrWidgetRepository(secretKey);
    const userDetailsVisible = useSelector((state: GlobalState) => state.behaviorReducer.userDetailsVisible);

    const [options, setOptions] = useState<LocaleMap>([]);
    const [phonePattern, setphonePattern] = useState<string>("");
    const [detailsSet, setDetailsSet] = useState<boolean>(false);

    const { chatStarted, setChatStarted, convoId } = useContext(WidgetContext);

    useEffect(() => {
        (async () => {
            const { currentLocale: locale, localeMap } = await client.Widget.Get.Locale();
            setphonePattern(locale.phoneFormat);
            setOptions(localeMap);
            setRegionContext(locale.name);
        })();
    }, []);

    const cls = useStyles();
    const [status, setStatus] = useState<string | null>(null);

    const onChange = (event: any, newOption: LocaleResource) => {
        setphonePattern(newOption.phoneFormat);
        setRegionContext(newOption.name);
    };

    const onFormSubmit = async (e: { preventDefault: () => void }) => {
        e.preventDefault();
        setKickoff(true);
        setChatStarted(true);

        const name = getNameContext();
        const email = getEmailAddressContext();
        const phone = getPhoneContext();
        const locale = getRegionContext();

        if (convoId) {
            await client.Widget.Post.UpdateConvoRecord({ Name: name, Email: email, PhoneNumber: phone, Locale: locale, ConversationId: convoId });
        }
        closeUserDetails();
    };

    const formProps = {
        status,
        setStatus,
    };

    return (
        <Dialog
            open={userDetailsVisible && chatStarted}
            className={cls.baseDialogCollectionForm}
            classes={{
                root: cls.dialogBackgroundCollectionForm,
                paper: cls.dialogPaperCollectionForm,
                paperScrollPaper: cls.dialogPaperScrollPaperCollectionForm,
            }}
            disableBackdropClick
            hideBackdrop={false}
            disableEscapeKeyDown
        >
            <UserDetailsTitle title="Update your Contact Details" />
            <DialogContent className={cls.dialogContentCollectionForm}>
                <ContactForm
                    disabled={false}
                    localeOptions={options}
                    onFormSubmit={onFormSubmit}
                    formProps={{ ...formProps }}
                    setDetailsSet={setDetailsSet}
                    phonePattern={phonePattern}
                    onChange={onChange}
                    detailsSet={detailsSet}
                    submitButton={
                        <Button
                            disabled={status === INVALID_PHONE || status === INVALID_EMAIL || status === INVALID_NAME}
                            className={cls.buttonCollectionForm}
                            endIcon={detailsSet && <CheckCircleOutlineIcon />}
                            type="submit"
                        >
                            <Typography variant="h5">{chatStarted ? "Continue" : "Begin"}</Typography>
                        </Button>
                    }
                />
            </DialogContent>
        </Dialog>
    );
};

export interface ContactFormProps {
    onFormSubmit(e: { preventDefault: () => void }): void;
    formProps: any;
    setDetailsSet: any;
    phonePattern: any;
    onChange: any;
    detailsSet: boolean;
    localeOptions: any;
    submitButton: React.ReactNode;
    disabled: boolean;
}

export const ContactForm = ({ disabled, onFormSubmit, submitButton, localeOptions, formProps, setDetailsSet, phonePattern, onChange, detailsSet }: ContactFormProps) => {
    const cls = useStyles();
    return (
        <form onSubmit={onFormSubmit}>
            <NameForm {...formProps} disabled={disabled} />
            <EmailForm {...formProps} setDetailsSet={setDetailsSet} disabled={disabled} />
            <PhoneForm {...formProps} phonePattern={phonePattern} disabled={disabled} />
            <LocaleSelector options={localeOptions} onChange={onChange} disabled={disabled} />
            <div style={{ display: "flex", justifyContent: "center" }}>{submitButton}</div>
        </form>
    );
};

export interface MiniContactFormProps {
    onFormSubmit(e: { preventDefault: () => void }): void;
    formProps: any;
    setDetailsSet: SetState<boolean>;
    submitButton: React.ReactNode;
    disabled: boolean;
}
export const MiniContactForm = ({ disabled, onFormSubmit, setDetailsSet, submitButton, formProps }: MiniContactFormProps) => {
    const cls = useStyles();
    return (
        <form onSubmit={onFormSubmit}>
            <NameForm {...formProps} disabled={disabled} />
            <EmailForm {...formProps} setDetailsSet={setDetailsSet} disabled={disabled} />
            <div style={{ display: "flex", justifyContent: "right" }}>{submitButton}</div>
        </form>
    );
};
