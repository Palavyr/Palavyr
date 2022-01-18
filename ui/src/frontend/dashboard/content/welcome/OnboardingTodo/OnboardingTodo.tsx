import React from "react";
import { makeStyles, Typography } from "@material-ui/core";
import { TodosAsBoolean } from "@Palavyr-Types";
import { SpaceEvenly } from "@common/positioning/SpaceEvenly";
import { TodoCard } from "./TodoCard";
import { HeaderStrip } from "@common/components/HeaderStrip";

const useStyles = makeStyles((theme) => ({
    list: {
        textAlign: "left",
    },
    container: {
        width: "50%",
    },
    body: {
        textAlign: "center",
    },
    sectionDiv: {
        width: "100%",
        display: "flex",
        textAlign: "center",
        justifyContent: "center",
        background: theme.palette.background.default,
    },
}));

export interface OnboardingTodoProps {
    todos: TodosAsBoolean;
}
export const OnboardingTodo = ({ todos }: OnboardingTodoProps) => {
    const cls = useStyles();

    return (
        <>
            <HeaderStrip title="Quick Start To Do List" subtitle="Don't forget to set these important settings" />
            <div className={cls.sectionDiv}>
                <SpaceEvenly vertical center>
                    {!todos?.isVerified && (
                        <TodoCard
                            link="/dashboard/settings/email?tab=0"
                            text={`Set your default email: ${todos?.emailAddress} - ${
                                todos?.awaitingVerification ? "Check your email to verify you address" : "Trigger an email verification in your settings."
                            }`}
                        />
                    )}
                    {!todos?.name && <TodoCard link="/dashboard/settings/companyName?tab=1" text="Set your company name" />}
                    {!todos?.phoneNumber && <TodoCard link="/dashboard/settings/phoneNumber?tab=2" text="Set your default contact phone number" />}
                    {!todos?.logoUri && <TodoCard link="/dashboard/settings/companyLogo?tab=3" text="Set your company logo" />}
                </SpaceEvenly>
            </div>
        </>
    );
};
