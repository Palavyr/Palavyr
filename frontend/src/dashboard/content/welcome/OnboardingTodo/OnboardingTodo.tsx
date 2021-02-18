import { makeStyles, Table, TableBody, TableContainer, TableHead, Typography } from "@material-ui/core";
import { TodosAsBoolean } from "@Palavyr-Types";
import { AlignCenter } from "dashboard/layouts/positioning/AlignCenter";
import React, {  } from "react";
import { DoCompanyLogo } from "./DoCompanyLogo";
import { DoCompanyName } from "./DoCompanyName";
import { DoDefaultEmail } from "./DoDefaultEmail";
import { DoPhoneNumber } from "./DoPhoneNumber";

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
        <AlignCenter>
            <Typography display="inline" style={{ padding: "2rem", fontSize: "24pt", fontWeight: "bolder" }}>
                Quick Start To Do list
            </Typography>
            <TableContainer className={cls.container}>
                <Table>
                    <TableHead>Complete the following tasks:</TableHead>
                    <TableBody className={cls.body}>
                        {!todos?.isVerified && <DoDefaultEmail emailAddress={todos?.emailAddress} awaitingVerification={todos?.awaitingVerification} />}
                        {!todos?.name && <DoCompanyName />}
                        {!todos?.logoUri && <DoCompanyLogo />}
                        {!todos?.phoneNumber && <DoPhoneNumber />}
                    </TableBody>
                </Table>
            </TableContainer>
        </AlignCenter>
    );
};