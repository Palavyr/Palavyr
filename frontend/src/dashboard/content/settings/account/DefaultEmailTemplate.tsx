import React, { useCallback, useContext, useEffect, useState } from "react";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { makeStyles } from "@material-ui/core";
import { VariableDetail } from "@Palavyr-Types";
import { EmailConfigurationComponent } from "dashboard/content/responseConfiguration/uploadable/emailTemplates/EmailConfigurationComponent";
import { Align } from "dashboard/layouts/positioning/Align";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const useStyles = makeStyles(() => ({
    root: {
        width: "100%",
        height: "100%",
        marginTop: "1rem",
    },
}));

export const DefaultEmailTemplate = () => {
    const client = new PalavyrRepository();
    const cls = useStyles();

    const { setIsLoading } = useContext(DashboardContext);
    const [variableDetails, setVariableDetails] = useState<VariableDetail[]>();

    const loadVariableDetails = useCallback(async () => {
        const variableDetails = await client.Configuration.Email.GetVariableDetails();
        setVariableDetails(variableDetails);
    }, []);

    useEffect(() => {
        setIsLoading(true);
        loadVariableDetails();
        setIsLoading(false);
    }, [loadVariableDetails]);

    return (
        <div className={cls.root}>
            <Align>
                <AreaConfigurationHeader title="Default Email Response" subtitle="Use this editor to create an HTML email template that will be sent as the email response for this area." />
            </Align>
            {variableDetails && (
                <EmailConfigurationComponent
                    variableDetails={variableDetails}
                    saveEmailTemplate={client.Configuration.Email.SaveDefaultFallbackEmailTemplate}
                    saveEmailSubject={async (subject: string) => await client.Configuration.Email.SaveDefaultFallbackSubject(subject)}
                    getCurrentEmailSubject={client.Configuration.Email.GetDefaultFallbackSubject}
                    getCurrentEmailTemplate={client.Configuration.Email.GetDefaultFallbackEmailTemplate}
                />
            )}
        </div>
    );
};
