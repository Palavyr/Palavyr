import * as React from "react";
import { makeStyles, Divider } from "@material-ui/core";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { OnboardingTodo } from "../OnboardingTodo/OnboardingTodo";
import { useContext, useEffect, useState } from "react";
import { TodosAsBoolean } from "@Palavyr-Types";
import { allClear, convertTodos } from "../OnboardingTodo/onboardingUtils";
import { SessionStorage } from "@localStorage/sessionStorage";
import { CacheIds } from "@api-client/FrontendAxiosClient";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { QuickStartCard } from "./QuickStartCard";
import { useHistory } from "react-router-dom";

const useStyles = makeStyles(theme => ({
    background: {
        paddingTop: "3rem",
        background: theme.palette.background.default,
    },
    wrapper: {
        position: "relative",
        height: "100%",
        textAlign: "center",
        background: theme.palette.background.default,
    },

    sectionHeadDiv: {
        width: "100%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
    listItems: {
        widgth: "40%",
        margin: "0 auto",
        padding: "1rem",
    },
    alert: {
        border: "1px solid black",
        marginTop: "1rem",
        marginBottom: "1rem",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },

    alertTitle: {
        fontSize: "16pt",
        fontWeight: "bold",
    },
    headCard: {
        display: "flex",
        justifyContent: "center",
    },
}));

export const QuickStartGuide = () => {
    const { repository, checkAreaCount, setViewName } = useContext(DashboardContext);
    const history = useHistory();
    setViewName("Welcome!");

    const [todos, setTodos] = useState<TodosAsBoolean>();
    const [loading, setLoading] = useState<boolean>(true);

    const loadTodos = React.useCallback(async () => {
        const name = await repository.Settings.Account.getCompanyName();
        const logoUri = await repository.Settings.Account.getCompanyLogo();
        const { phoneNumber, locale } = await repository.Settings.Account.getPhoneNumber();
        const { emailAddress, isVerified, awaitingVerification } = await repository.Settings.Account.getEmail();

        const todos = {
            name,
            emailAddress,
            isVerified,
            awaitingVerification,
            logoUri,
            phoneNumber,
            locale,
        };

        const todosAsBoolean = convertTodos(todos);
        setTodos(todosAsBoolean);
    }, []);

    useEffect(() => {
        SessionStorage.clearCacheValue(CacheIds.Enquiries);
        loadTodos();
        setLoading(false);
    }, []);

    const sendToGetWidgetPage = () => {
        history.push("/dashboard/getwidget");
    };

    return (
        <>
            {todos && !allClear(todos) && <OnboardingTodo todos={todos} />}
            <Divider />
            <AreaConfigurationHeader divider title="Quick Start Guide" subtitle="Follow the steps to learn about how Palavyr works and what you should do to get started." />
            <div style={{ height: "2rem" }} />
            <QuickStartCard
                rowNumber={1}
                title="Create your first intent"
                content="Each intent that shows up in the chatbot is created individually. When you create intents for your chatbot, consider how your business is modelled and how your customers are likely to interact with it."
                onClick={checkAreaCount}
            />
            <QuickStartCard rowNumber={2} title="Configure your new intent" content="In your intent configuration, simply follow the tabs in the order they are provided (from left to right)." />
            <QuickStartCard
                rowNumber={3}
                title="Add the widget to your site"
                content="When you are ready to use your configured widget, simply paste the provided code into your website's html."
                onClick={sendToGetWidgetPage}
            />
        </>
    );
};
