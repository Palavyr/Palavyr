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
import { LocaleMap, LocaleResource, SetState } from "@Palavyr-Types";
import { INVALID_PHONE, INVALID_EMAIL, INVALID_NAME } from "./UserDetailsCheck";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
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
        // backgroundColor: "rgba(255, 255, 255, 30)",
        border: "none",
        boxShadow: "none",
        shadow: "none",
        backgroundColor: theme.palette.info.main


        // zIndex: 9999,
    },
    dialogPaperCollectionForm: {
        // zIndex: 9999,
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        paddingBottom: theme.spacing(3),
        maxWidth: 420,
        backgroundColor: theme.palette.info.main,
        // backgroundColor: "rgba(255, 255, 255, 50)",
        border: "none",
        boxShadow: "none",
        shadow: "none"
    },
    dialogPaperScrollPaperCollectionForm: {
        maxHeight: "none",
        border: "none",
        boxShadow: "none",
        shadow: "none"
    },
    dialogContentCollectionForm: {
        paddingTop: 0,
        paddingBottom: 0,
        border: "none",
        boxShadow: "none",
        shadow: "none"
    },
    buttonCollectionForm: {
        margin: "0.5rem",
        textAlign: "center",
        marginTop: "1.3rem",
        border: "none",
        boxShadow: "none",
        shadow: "none"
    },
    backgropPropsClassName: {
        backgroundColor: theme.palette.info.main
    }
}));

export const CollectDetailsForm = ({ setKickoff }: CollectDetailsFormProps) => {
    const secretKey = new URLSearchParams(useLocation().search).get("key");
    const client = new PalavyrWidgetRepository(secretKey);
    const [options, setOptions] = useState<LocaleMap>([]);
    const [phonePattern, setphonePattern] = useState<string>("");
    const [detailsSet, setDetailsSet] = useState<boolean>(false);

    const { chatStarted, setChatStarted, convoId, context } = useContext(WidgetContext);

    useEffect(() => {
        (async () => {
            const { currentLocale: locale, localeMap } = await client.Widget.Get.Locale();
            setphonePattern(locale.phoneFormat);
            setOptions(localeMap);
            context.setRegion(locale.name);
        })();
    }, []);

    const cls = useStyles();
    const [status, setStatus] = useState<string | null>(null);

    const onChange = (event: any, newOption: LocaleResource) => {
        setphonePattern(newOption.phoneFormat);
        context.setRegion(newOption.name);
    };

    const onFormSubmit = async (e: { preventDefault: () => void }) => {
        e.preventDefault();
        setKickoff(true);
        setChatStarted(true);

        if (convoId) {
            await client.Widget.Post.UpdateConvoRecord({ Name: context.name, Email: context.emailAddress, PhoneNumber: context.phoneNumber, Locale: context.region, ConversationId: convoId });
        }
        context.closeUserDetails();
    };

    const formProps = {
        status,
        setStatus,
    };

    return (
        <>
            {/* <video>
                <source src="./public/background-gif.mp4" type="video/mp4" />
            </video> */}

            {/* <img src="./background-gif.mp4" alt="background" height={"100%"} width={"100%"} style={{ height: "100vh", width: "100%" }} /> */}
            <Dialog
                open={context.userDetailsVisible && chatStarted}
                className={cls.baseDialogCollectionForm}
                classes={{
                    root: cls.dialogBackgroundCollectionForm,
                    paper: cls.dialogPaperCollectionForm,
                    paperScrollPaper: cls.dialogPaperScrollPaperCollectionForm,
                    scrollPaper: cls.dialogPaperScrollPaperCollectionForm,
                }}
                BackdropProps={{ className: cls.backgropPropsClassName }}
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
        </>
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
