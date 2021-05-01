import React from "react";
import { makeStyles, Typography } from "@material-ui/core";
import { TodosAsBoolean } from "@Palavyr-Types";
import { Align } from "dashboard/layouts/positioning/Align";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import { TodoCard } from "./TodoCard";

const useStyles = makeStyles(() => ({
    list: {
        textAlign: "left",
    },
    container: {
        width: "50%",
    },
    body: {
        textAlign: "center",
    },
}));

export interface OnboardingTodoProps {
    todos: TodosAsBoolean;
}
export const OnboardingTodo = ({ todos }: OnboardingTodoProps) => {
    const cls = useStyles();

    return (
        <div style={{ display: "inline-block" }}>
            <Typography align="center" display="inline" gutterBottom variant="h4">
                Quick Start To Do list
            </Typography>
            <SpaceEvenly vertical center>
                {!todos?.isVerified && (
                    <TodoCard
                        link="/dashboard/settings/email?tab=1"
                        text={`Set your default email: ${todos?.emailAddress} - ${todos?.awaitingVerification ? "Check your email to verify you address" : "Trigger an email verification in your settings."}`}
                    />
                )}
                {!todos?.name && <TodoCard link="/dashboard/settings/companyName?tab=2" text="Set your company name" />}
                {!todos?.logoUri && <TodoCard link="/dashboard/settings/companyLogo?tab=4" text="Set your company logo in the settings page" />}
                {!todos?.phoneNumber && <TodoCard link="settings/phoneNumber?tab=3" text="Set your default contact phone number in the settings page" />}
            </SpaceEvenly>
        </div>
    );
};
