import React, { useState, useCallback, useEffect, useContext } from "react";
import { Divider } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { Settings, VariableDetail } from "@Palavyr-Types";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { OsTypeToggle } from "../../areaSettings/enableAreas/OsTypeToggle";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { EmailConfigurationComponent } from "./EmailConfigurationComponent";
import { PalavyrRepository } from "@api-client/PalavyrRepository";

export const EmailConfiguration = () => {
    var client = new PalavyrRepository();

    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const { setIsLoading } = useContext(DashboardContext);

    const [loaded, setLoaded] = useState<boolean>(false);
    const [settings, setSettings] = useState<Partial<Settings>>({ useAreaFallbackEmail: false });
    const [variableDetails, setVariableDetails] = useState<VariableDetail[]>();

    const [useAreaEmail, setUseAreaEmail] = useState<boolean>(false);
    const [useAreaFallbackEmail, setUseAreaFallbackEmail] = useState<boolean>(false);

    const onUseAreaFallbackEmailToggle = async () => {
        const updatedUsAreaFallback = await client.Area.UpdateUseAreaFallbackEmail(!useAreaEmail, areaIdentifier);
        setUseAreaEmail(updatedUsAreaFallback);
        setUseAreaFallbackEmail(!useAreaFallbackEmail);
    };

    const loadVariableDetails = useCallback(async () => {
        const variableDetails = await client.Configuration.Email.GetVariableDetails();
        setVariableDetails(variableDetails);
        return () => {
            setLoaded(false);
        };
    }, []);

    const loadSettings = useCallback(async () => {
        const areaData = await client.Area.GetArea(areaIdentifier);
        setSettings({
            ...settings,
            useAreaFallbackEmail: areaData.useAreaFallbackEmail,
        });
        setUseAreaFallbackEmail(areaData.useAreaFallbackEmail);
    }, []);

    useEffect(() => {
        setIsLoading(true);
        loadVariableDetails();
        loadSettings();
        return () => {
            setLoaded(false);
        };
    }, [loadVariableDetails, loadSettings]);

    return (
        <>
            <AreaConfigurationHeader title="Email Response" subtitle="Use this editor to create an HTML email template that will be sent as the email response for this area." />
            {useAreaFallbackEmail !== null && <OsTypeToggle controlledState={useAreaFallbackEmail} onChange={onUseAreaFallbackEmailToggle} enabledLabel="Use Area Fallback Email" disabledLabel="Use General Fallback Email" />}
            {variableDetails && (
                <EmailConfigurationComponent
                    variableDetails={variableDetails}
                    saveEmailTemplate={async (emailTemplate: string) => await client.Configuration.Email.SaveAreaEmailTemplate(areaIdentifier, emailTemplate)}
                    saveEmailSubject={async (subject: string) => await client.Configuration.Email.SaveAreaSubject(areaIdentifier, subject)}
                    getCurrentEmailSubject={async () => await client.Configuration.Email.GetAreaSubject(areaIdentifier)}
                    getCurrentEmailTemplate={async () => await client.Configuration.Email.GetAreaEmailTemplate(areaIdentifier)}
                />
            )}
            <Divider />
            {useAreaFallbackEmail && (
                <>
                    <AreaConfigurationHeader
                        title="Fallback Email Response"
                        subtitle="Use this editor to create a fallback email response that is specific to this area. For example, this email is sent if a 'Too Complicated' node is encountered during the chat."
                    />
                    {variableDetails && (
                        <EmailConfigurationComponent
                            variableDetails={variableDetails}
                            saveEmailTemplate={async (emailTemplate: string) => await client.Configuration.Email.SaveAreaFallbackEmailTemplate(areaIdentifier, emailTemplate)}
                            saveEmailSubject={async (emailSubject: string) => await client.Configuration.Email.SaveAreaFallbackSubject(areaIdentifier, emailSubject)}
                            getCurrentEmailSubject={async () => await client.Configuration.Email.GetAreaFallbackSubject(areaIdentifier)}
                            getCurrentEmailTemplate={async () => await client.Configuration.Email.GetAreaFallbackEmailTemplate(areaIdentifier)}
                        />
                    )}
                </>
            )}
        </>
    );
};
